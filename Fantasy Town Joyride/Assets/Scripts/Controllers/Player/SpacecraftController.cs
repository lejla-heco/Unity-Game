using System;
using System.Collections;
using System.Collections.Generic;
using Spacecraft.Consts;
using Spacecraft.Controllers.Core;
using Spacecraft.Controllers.Core.Entities;
using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;
namespace Spacecraft.Controllers.Player
{
	[System.Serializable]
	public enum LANE
	{
		Left,
		Middle,
		Right
	}

	public class SpacecraftController : TrackedEntity
	{

		[SerializeField] private InputManagement InputManager;
		[SerializeField] [Range(0.1f, 100f)] private float ForwardSpeed;
		[SerializeField] private LANE CurrentPosition = LANE.Middle;
		[SerializeField] private AudioClip SlideSound;
		[SerializeField] private float SlideLength;
		[SerializeField] private LevelGenerator LevelGenerator;
		private ShipAnimatorController ShipAnimator { get; set; }

		private float x;
		private GameObject Level;
		private float NewHorizontalValue;
		private const int DefaultShipYPosition = 1;
		private Vector3 PlayerVelocity;
		private float JumpHeight = 1.0f;
		private float LaneChangeSpeed = 3f;

		//angles:
		private Quaternion TiltAngleRight = Quaternion.Euler(0, 0, -30);
		private Quaternion TiltAngleLeft = Quaternion.Euler(0, 0, 30);
		private Quaternion IdleAngle = Quaternion.Euler(0, 0, 0);

		[SerializeField] private Quaternion CurrentAngle;
		private CharacterController CharacterControl;

		private void Start()
		{
			transform.position = Vector3.zero;
			CurrentAngle = IdleAngle;
			ShipAnimator = transform.GetChild(0).gameObject.GetComponent<ShipAnimatorController>();
			CharacterControl = GetComponent<CharacterController>();
			InputManager.OnInputChanged += OnInputChanged;

			if (Level == null) // we will use this wrapper to control Z axis of player and map
			{
				Level = GameObject.Find("Level");
			}
		}


		private void OnInputChanged()
		{
			// if (IsPaused)
			// {
			// 	Debug.Log("Umro je");
			// 	return;
			// }

			if (Input.GetButtonUp("Horizontal")) CurrentAngle = IdleAngle;

			if (Input.GetButtonDown("Horizontal"))
			{
				bool IsMovingLeft = Input.GetAxis("Horizontal") < 0;

				switch (CurrentPosition)
				{
					case LANE.Middle:
						NewHorizontalValue = SlideLength * (IsMovingLeft ? -1 : 1);
						CurrentPosition = NewHorizontalValue < 0 ? LANE.Left : LANE.Right;
						CurrentAngle = IsMovingLeft ? TiltAngleLeft : TiltAngleRight;
						break;
					case LANE.Right:
						CurrentAngle = IdleAngle;
						if (!IsMovingLeft) break;
						NewHorizontalValue = 0;
						CurrentPosition = LANE.Middle;
						CurrentAngle = TiltAngleLeft;
						break;
					case LANE.Left:
						CurrentAngle = IdleAngle;
						if (IsMovingLeft) break;
						NewHorizontalValue = 0;
						CurrentPosition = LANE.Middle;
						CurrentAngle = TiltAngleRight;
						break;
				}

				PlaySlideSound();
			}

			var IsGrounded = DefaultShipYPosition == (int)Math.Round(transform.position.y);
			if (IsGrounded && PlayerVelocity.y < 0) // check if player is grounded and jump value goes below 0 and set it to 0
			{
				PlayerVelocity.y = 0f;
			}

			if (Input.GetButtonDown("Vertical"))
			{
				if (Input.GetAxis("Vertical") > 0 && IsGrounded)
				{
					PlayerVelocity.y += Mathf.Sqrt(JumpHeight * -1.0f * GameConsts.GravityValue);
					ShipAnimator.TriggerMoveUp();
				}
				else
				{
					ShipAnimator.TriggerMoveDown();
				}

				PlaySlideSound();
			}


			x = Mathf.Lerp(x, NewHorizontalValue, LaneChangeSpeed * Time.deltaTime);
			PlayerVelocity.x = x - transform.position.x;

			PlayerVelocity.y = 0; //+= GameConsts.GravityValue * Time.deltaTime;
			PlayerVelocity.z = ForwardSpeed * Time.deltaTime;

			CharacterControl.Move(
				PlayerVelocity
			);

			transform.rotation = Quaternion.Slerp(transform.rotation, CurrentAngle, 0.1f);

			if (transform.position.z > (GameConsts.HowManyUnitsUntilWorldResets + GameConsts.ChunkGenerationOffset)) // this code resets player and the map
			{
				ResetObjectsToOrigin();
			}
		}

		private void PlaySlideSound()
		{
			AudioSource.PlayClipAtPoint(SlideSound, transform.position);
		}

		private void ResetObjectsToOrigin()
		{
			int activeChildren = 0;
			// move all active child objects
			for (int i = 0; i < Level.transform.childCount; i++)
			{
				Transform child = Level.transform.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					var localPosition = child.localPosition;
					child.localPosition = new Vector3(localPosition.x, localPosition.y, (activeChildren * GameConsts.ChunkLength) + GameConsts.ChunkGenerationOffset);
					activeChildren++;
				}
			}
			// return player to 0
			// TODO ovdje zapne malo igrac zbog vremenske razlike kada se vrate chunkovi mape i kada se vrati igrac
			var position = transform.position;
			transform.position = new Vector3(position.x, position.y, 0 + GameConsts.ChunkGenerationOffset);
			LevelGenerator.ResetChunkNumbers(GameConsts.InitialChunksNumber); // reset chunk numbers for calc
		}
	}
}
