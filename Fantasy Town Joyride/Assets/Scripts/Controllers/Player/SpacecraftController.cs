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
        [SerializeField] [Range(0.1f, 10f)] private float LaneChangeSpeed;
        [SerializeField] private LANE currentPosition = LANE.Middle;
    
        private float NewHorizontalValue;
        [SerializeField] private float SlideLength;

        private float x;
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
            CurrentAngle = IdleAngle;
            CanMove = true;
            CharacterControl = GetComponent<CharacterController>();
            InputManager.OnInputChanged += OnInputChanged;
        }
        
        private void OnInputChanged(float horizontal)
        {
            if (horizontal < 0)
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
            else if (horizontal > 0)
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

            x = Mathf.Lerp(x, NewHorizontalValue, Time.deltaTime * LaneChangeSpeed);
            Vector3 movePlayer = new Vector3(x - transform.position.x, 0, 0/*ForwardSpeed * Time.deltaTime*/);
            CharacterControl.Move(movePlayer);
            
            if (Input.GetKeyDown(KeyCode.LeftArrow)) CurrentAngle = TiltAngleLeft;
            if (Input.GetKeyDown(KeyCode.RightArrow)) CurrentAngle = TitlAngleRight;
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) CurrentAngle = IdleAngle;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, CurrentAngle, 0.1f);

        }

        IEnumerator ActivateMoving()
        {
            yield return new WaitForSeconds(1f);
            CanMove = true;
        }
    }
}