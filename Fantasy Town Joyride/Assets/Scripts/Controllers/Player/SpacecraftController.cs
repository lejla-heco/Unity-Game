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
		[SerializeField] [Range(0.1f, 20f)] private float LaneChangeSpeed;
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
		private float jumpHeight = 1.0f;
		private float gravityValue = -9.81f;

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
					ShipAnimator.TriggerMoveUp();
					PlayerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
				}
				else
				{
					ShipAnimator.TriggerMoveDown();
				}

				PlaySlideSound();
			}

			Debug.Log(transform.position.y);

			PlayerVelocity.x = Mathf.Lerp(x, NewHorizontalValue, LaneChangeSpeed * Time.deltaTime) - transform.position.x;
			PlayerVelocity.y += gravityValue * Time.deltaTime;
			PlayerVelocity.z = ForwardSpeed;

			CharacterControl.Move(
				PlayerVelocity * Time.deltaTime
			);

			transform.rotation = Quaternion.Slerp(transform.rotation, CurrentAngle, 0.1f);

			if (transform.position.z > GameConsts.HowManyUnitsUntilWorldResets) // this code resets player and map
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
			var position = transform.position;
			transform.position = new Vector3(position.x, position.y, 0);
			LevelGenerator.ResetChunkNumbers(GameConsts.InitialChunksNumber);
		}
	}
}
