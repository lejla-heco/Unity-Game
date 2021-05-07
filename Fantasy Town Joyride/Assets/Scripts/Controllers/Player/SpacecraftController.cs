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
		private GameObject Player;
		private GameObject Level;
		private float NewHorizontalValue;

		//angles:
		private Quaternion TiltAngleRight = Quaternion.Euler(0, 0, -30);
		private Quaternion TiltAngleLeft = Quaternion.Euler(0, 0, 30);
		private Quaternion IdleAngle = Quaternion.Euler(0, 0, 0);

		[SerializeField] private Quaternion CurrentAngle;
		private CharacterController CharacterControl;

		const KeyCode Up = KeyCode.UpArrow;
		const KeyCode Down = KeyCode.DownArrow;
		const KeyCode Right = KeyCode.RightArrow;
		const KeyCode Left = KeyCode.LeftArrow;

		private void Start()
		{
			Player = transform.GetChild(0).gameObject; // why do we need this?

			transform.position = Vector3.zero;
			CurrentAngle = IdleAngle;
			ShipAnimator = Player.GetComponent<ShipAnimatorController>();
			CharacterControl = GetComponent<CharacterController>();
			InputManager.OnInputChanged += OnInputChanged;

			if (Level == null) // we will use this wrapper to control Z axis of player and map
			{
				Level = GameObject.Find("Level");
			}
		}


		private void OnInputChanged()
		{

			if (IsPaused)
			{
				Debug.Log("Umro je");
				return;
			}

			if (Input.GetKeyDown(Left))
			{
				if (CurrentPosition == LANE.Middle)
				{
					NewHorizontalValue = -SlideLength;
					CurrentPosition = LANE.Left;
				}
				else if (CurrentPosition == LANE.Right)
				{
					NewHorizontalValue = 0;
					CurrentPosition = LANE.Middle;

				}
				PlaySlideSound();
			}
			else if (Input.GetKeyDown(Right))
			{
				if (CurrentPosition == LANE.Middle)
				{
					NewHorizontalValue = SlideLength;
					CurrentPosition = LANE.Right;
				}
				else if (CurrentPosition == LANE.Left)
				{
					NewHorizontalValue = 0;
					CurrentPosition = LANE.Middle;

				}
				PlaySlideSound();
			}


			if (Input.GetKeyDown(Up))
			{
				ShipAnimator.TriggerMoveUp();
				PlaySlideSound();
			}

			if (Input.GetKeyDown(Down))
			{
				ShipAnimator.TriggerMoveDown();
				PlaySlideSound();
			}


			x = Mathf.Lerp(x, NewHorizontalValue, Time.deltaTime * LaneChangeSpeed);

			Vector3 movePlayer = new Vector3(x - transform.position.x, 0, ForwardSpeed * Time.deltaTime);
			CharacterControl.Move(movePlayer);


			if (Input.GetKeyDown(Left)) CurrentAngle = TiltAngleLeft;
			if (Input.GetKeyDown(Right)) CurrentAngle = TiltAngleRight;
			if (Input.GetKeyUp(Left) || Input.GetKeyUp(Right)) CurrentAngle = IdleAngle;
			transform.rotation = Quaternion.Slerp(this.transform.rotation, CurrentAngle, 0.1f);

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
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
			LevelGenerator.ResetChunkNumbers(activeChildren);

			Debug.Log("Active Children for reset: " + activeChildren);
		}
	}
}
