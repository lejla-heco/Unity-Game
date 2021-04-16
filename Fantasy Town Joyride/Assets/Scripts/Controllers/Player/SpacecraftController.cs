
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
    [System.Serializable]
    public enum VER
    {
        Up,
        Middle,
        Down
    }

    public class SpacecraftController : MonoBehaviour
    {
        [SerializeField] private InputManagement InputManager;
        [SerializeField] [Range(0.1f, 100f)] private float ForwardSpeed;
        [SerializeField] [Range(0.1f, 20f)] private float LaneChangeSpeed;
        [SerializeField] private LANE currentPosition = LANE.Middle;
        [SerializeField] private VER currentPositionVer = VER.Middle;

        private float NewHorizontalValue;
        private float NewVerticalValue;
        [SerializeField] private float SlideLength;
        [SerializeField] private float SlideLengthVer;

        private float x;
        private float y;
        private bool CanMove;

        //angles:
        private Quaternion TiltAngleLeft = Quaternion.Euler(0, 0, -30);
        private Quaternion TitlAngleRight = Quaternion.Euler(0, 0, 30);
        private Quaternion IdleAngle = Quaternion.Euler(0, 0, 0);

        [SerializeField] private Quaternion CurrentAngle;
        private CharacterController CharacterControl;
        private void Start() 
        {
            transform.position = Vector3.zero; 
            CurrentAngle = IdleAngle;//0,0,0
            CanMove = true;
            CharacterControl = GetComponent<CharacterController>();
            InputManager.OnInputChanged += OnInputChanged;
          


        }



        private void OnInputChanged(float horizontal, float vertical)// promjene trake 
        {
            if (horizontal < 0)//lijevo 
            {
                if (currentPosition == LANE.Middle && CanMove)
                {
                    NewHorizontalValue = -SlideLength;
                    currentPosition = LANE.Left;
                }
                else if (currentPosition == LANE.Right && CanMove)
                {
                    NewHorizontalValue = 0;
                    currentPosition = LANE.Middle;

                }


                CanMove = false;
                StartCoroutine(ActivateMoving());
            }
            else if (horizontal > 0)// desno 
            {
                if (currentPosition == LANE.Middle && CanMove)
                {
                    NewHorizontalValue = SlideLength;
                    currentPosition = LANE.Right;
                }
                else if (currentPosition == LANE.Left && CanMove)
                {
                    NewHorizontalValue = 0;
                    currentPosition = LANE.Middle;

                }
                CanMove = false;
                StartCoroutine(ActivateMoving());
            }
            else if (vertical < 0)// dole
            {
                if (currentPositionVer == VER.Middle && CanMove)
                {
                    NewVerticalValue = -SlideLengthVer;
                    currentPositionVer = VER.Down;
                }
                else if (currentPositionVer == VER.Up && CanMove)
                {
                    NewVerticalValue = 0;
                    currentPositionVer = VER.Middle;
                }
                CanMove = false;
                StartCoroutine(ActivateMoving());
            }
            else if (vertical > 0)//gore
            {
                if (currentPositionVer == VER.Middle && CanMove)
                {
                    NewVerticalValue = SlideLengthVer;
                    currentPositionVer = VER.Up;
                }
                else if (currentPositionVer == VER.Down && CanMove)
                {
                    NewVerticalValue = 0;
                    currentPositionVer = VER.Middle;
                }
                CanMove = false;
                StartCoroutine(ActivateMoving());
            }

            x = Mathf.Lerp(x, NewHorizontalValue, Time.deltaTime * LaneChangeSpeed);// suptilan prelazak izmedju traka
            y = Mathf.Lerp(y, NewVerticalValue, Time.deltaTime * LaneChangeSpeed);

            Vector3 movePlayer = new Vector3(x - transform.position.x, y - transform.position.y, 0); //ForwardSpeed * Time.deltaTime);
            CharacterControl.Move(movePlayer);

            // za uglove, promjenit za ose 
            if (Input.GetKeyDown(KeyCode.LeftArrow)) CurrentAngle = TiltAngleLeft;
            if (Input.GetKeyDown(KeyCode.RightArrow)) CurrentAngle = TitlAngleRight;
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) CurrentAngle = IdleAngle;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, CurrentAngle, 0.1f);
           

        }

        IEnumerator ActivateMoving()// povratni tip interfejs 
        {
            yield return new WaitForSeconds(1f);
            CanMove = true;
        }
    }
}