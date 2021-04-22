
using System.Collections;
using System.Collections.Generic;
using Spacecraft.Controllers.Core;
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
   

    public class SpacecraftController : MonoBehaviour
    {
        [SerializeField] private InputManagement InputManager;
        [SerializeField] [Range(0.1f, 100f)] private float ForwardSpeed;
        [SerializeField] [Range(0.1f, 20f)] private float LaneChangeSpeed;
        [SerializeField] private LANE currentPosition = LANE.Middle;

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
            if (Input.GetKeyDown(left))
            {
                if (currentPosition == LANE.Middle)
                {
                    NewHorizontalValue = -SlideLength;
                    currentPosition = LANE.Left;
                }
                else if (currentPosition == LANE.Right)
                {
                    NewHorizontalValue = 0;
                    currentPosition = LANE.Middle;

                }

            }
            else if (Input.GetKeyDown(right))
            {
                if (currentPosition == LANE.Middle)
                {
                    NewHorizontalValue = SlideLength;
                    currentPosition = LANE.Right;
                }
                else if (currentPosition == LANE.Left)
                {
                    NewHorizontalValue = 0;
                    currentPosition = LANE.Middle;

                }
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
    }
}