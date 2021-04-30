
using System.Collections;
using System.Collections.Generic;
using Spacecraft.Controllers.Core;
using Spacecraft.Controllers.Core.Entities;
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

        private float NewHorizontalValue;
        private ShipAnimatorController ShipAnimator { get; set; }

        private float NewVerticalValue;
        [SerializeField] private float SlideLength;

        private float x;
        private GameObject player;

        //angles:
        private Quaternion TiltAngleRight = Quaternion.Euler(0, 0, -30);
        private Quaternion TiltAngleLeft = Quaternion.Euler(0, 0, 30);
        private Quaternion IdleAngle = Quaternion.Euler(0, 0, 0);

        [SerializeField] private Quaternion CurrentAngle;
        private CharacterController CharacterControl;

        static KeyCode up;
        static KeyCode down;
        static KeyCode right;
        static KeyCode left;
        
        [SerializeField] private AudioClip SlideSound;

        static public void SetDefaultControls()
        {
            up = KeyCode.UpArrow;
            down = KeyCode.DownArrow;
            right = KeyCode.RightArrow;
            left = KeyCode.LeftArrow;
        }


        private void Start()
        {
            SetDefaultControls();
            transform.position = Vector3.zero;
            CurrentAngle = IdleAngle;
            player = transform.GetChild(0).gameObject;
            ShipAnimator = player.GetComponent<ShipAnimatorController>();
            CharacterControl = GetComponent<CharacterController>();
            InputManager.OnInputChanged += OnInputChanged;
        }


        private void OnInputChanged()
        {
            if (IsPaused)
            {

                Debug.Log("Umro je");
                return;
            }

            if (Input.GetKeyDown(left))
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
            else if (Input.GetKeyDown(right))
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


            if (Input.GetKeyDown(up))
            {
                ShipAnimator.TriggerMoveUp();
            }

            if (Input.GetKeyDown(down))
            {
                ShipAnimator.TriggerMoveDown();
            }


            x = Mathf.Lerp(x, NewHorizontalValue, Time.deltaTime * LaneChangeSpeed);

            Vector3 movePlayer = new Vector3(x - transform.position.x, 0, ForwardSpeed * Time.deltaTime);
            CharacterControl.Move(movePlayer);


            if (Input.GetKeyDown(left)) CurrentAngle = TiltAngleLeft;
            if (Input.GetKeyDown(right)) CurrentAngle = TiltAngleRight;
            if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) CurrentAngle = IdleAngle;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, CurrentAngle, 0.1f);


        }

        private void PlaySlideSound()
        {
            AudioSource.PlayClipAtPoint(SlideSound, player.transform.position);
        }
    }
}