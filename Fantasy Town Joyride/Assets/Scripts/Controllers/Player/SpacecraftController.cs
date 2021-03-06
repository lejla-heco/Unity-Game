using System;
using Spacecraft.Consts;
using Spacecraft.Core.Entities;
using Spacecraft.Core.LevelGenerator;
using Spacecraft.ScriptableObjects;
using UnityEngine;

namespace Spacecraft.Controllers.Player
{
    [Serializable]
    public enum LANE
    {
        Left,
        Middle,
        Right
    }

    public class SpacecraftController : TrackedEntity
    {
        [SerializeField] [Range(0.1f, 100f)] private float ForwardSpeed;
        [SerializeField] [Range(0.1f, 100f)] private float LaneChangeSpeed = 6f;
        [SerializeField] private LANE CurrentPosition = LANE.Middle;
        [SerializeField] private LANE PreviousPosition = LANE.Middle;
        [SerializeField] private AudioClip SlideSound;
        [SerializeField] private float SlideLength;
        [SerializeField] private LevelGenerator LevelGenerator;
        private bool FirstReset = true;

        private ShipAnimatorController ShipAnimator { get; set; }

        private GameObject Level;
        private float NewHorizontalValue;
        private const int DefaultShipYPosition = 1;
        private Vector3 PlayerVelocity;
        private bool ResetOriginFlag = false;

        //angles:
        private Quaternion TiltAngleRight = Quaternion.Euler(0, 0, -30);
        private Quaternion TiltAngleLeft = Quaternion.Euler(0, 0, 30);
        private Quaternion IdleAngle = Quaternion.Euler(0, 0, 0);

        [SerializeField] private Quaternion CurrentAngle;
        private CharacterController CharacterControl;

        private void Awake()
        {
            //           Application.targetFrameRate = 20;
            CurrentAngle = IdleAngle;
            ShipAnimator = transform.GetChild(0).gameObject.GetComponent<ShipAnimatorController>();
            CharacterControl = GetComponent<CharacterController>();

            if (Level == null) // we will use this wrapper to control Z axis of player and map
            {
                Level = GameObject.FindGameObjectWithTag("LevelWrapper");
            }
        }


        private void Update()
        {
            if (IsGameOver)
            {
                return;
            }

            if (Input.GetButtonUp("Horizontal")) CurrentAngle = IdleAngle;

            if (Input.GetButtonDown("Horizontal"))
            {
                bool IsMovingLeft = Input.GetAxis("Horizontal") < 0;

                switch (CurrentPosition)
                {
                    case LANE.Middle:
                        NewHorizontalValue = SlideLength * (IsMovingLeft ? -1 : 1);
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = NewHorizontalValue < 0 ? LANE.Left : LANE.Right;
                        CurrentAngle = IsMovingLeft ? TiltAngleLeft : TiltAngleRight;
                        break;
                    case LANE.Right:
                        CurrentAngle = IdleAngle;
                        if (!IsMovingLeft) break;
                        NewHorizontalValue = 0;
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = LANE.Middle;
                        CurrentAngle = TiltAngleLeft;
                        break;
                    case LANE.Left:
                        CurrentAngle = IdleAngle;
                        if (IsMovingLeft) break;
                        NewHorizontalValue = 0;
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = LANE.Middle;
                        CurrentAngle = TiltAngleRight;
                        break;
                }

                PlaySlideSound();
            }


            if (Input.GetButtonDown("Vertical"))
            {
                var IsGrounded = DefaultShipYPosition == (int) Math.Round(transform.position.y);

                if (Input.GetAxis("Vertical") > 0 && IsGrounded)
                {
                    ShipAnimator.TriggerMoveUp();
                }
                else
                {
                    ShipAnimator.TriggerMoveDown();
                }

                PlaySlideSound();
            }

            var Course = WhichWay(PreviousPosition, CurrentPosition);
            if (transform.position.x * Course < Math.Abs(NewHorizontalValue))
            {
                PlayerVelocity.x = LaneChangeSpeed * Course;
            }
            else
            {
                PlayerVelocity.x = 0;
            }

            PlayerVelocity.z = ForwardSpeed;

            if (transform.position.z >
                (GameConsts.HowManyUnitsUntilWorldResets +
                 GameConsts.ChunkGenerationOffset)) // this code resets player and the map
            {
                ResetOriginFlag = true;
            }
        }

        private void FixedUpdate()
        {
            if (IsGameOver)
            {
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, CurrentAngle, 0.1f);

            CharacterControl.Move(
                PlayerVelocity * Time.fixedDeltaTime
            );

            if (ResetOriginFlag)
            {
                ResetObjectsToOrigin();
                ResetOriginFlag = false;
            }
        }

        private int WhichWay(LANE Previous, LANE Current)
        {
            if ((Previous == LANE.Middle && Current == LANE.Right) || Previous == LANE.Left)
            {
                return 1;
            }

            if ((Previous == LANE.Middle && Current == LANE.Left) || Previous == LANE.Right)
            {
                return -1;
            }

            return 0;
        }

        private void PlaySlideSound()
        {
            AudioSource.PlayClipAtPoint(SlideSound, transform.position);
        }

        private void ResetObjectsToOrigin()
        {
            int ActiveChildren = 0;
            // move all active child objects
            for (int i = 0; i < Level.transform.childCount; i++)
            {
                Transform child = Level.transform.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    var LocalPosition = child.localPosition;
                    child.localPosition = new Vector3(LocalPosition.x, LocalPosition.y,
                        (ActiveChildren * GameConsts.ChunkLength) + GameConsts.ChunkGenerationOffset);
                    ActiveChildren++;
                }

                if (FirstReset)
                {
                    FirstReset = false;
                    GameConsts.HowManyUnitsUntilWorldResets = 300;
                }
            }

            // return player to 0
            var Position = transform.position;
            transform.position = new Vector3(Position.x, Position.y, 0 + GameConsts.ChunkGenerationOffset);
            LevelGenerator.ResetChunkNumbers(GameConsts.InitialChunksNumber); // reset chunk numbers for calc
        }
    }
}