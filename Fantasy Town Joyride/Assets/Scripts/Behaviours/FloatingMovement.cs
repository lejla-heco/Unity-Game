using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Behaviours
{
    public class FloatingMovement : MonoBehaviour
    {
        private Vector3 StartingPosition;
        [SerializeField] private int Speed = 2;
        void Start()
        {
            StartingPosition.x = transform.position.x;
            StartingPosition.y = transform.position.y;
            StartingPosition.z = transform.position.z;
        }

        void Update()
        {
            transform.position = new Vector3(StartingPosition.x, StartingPosition.y + Mathf.Sin(Speed * Time.time), StartingPosition.z );
        }
    }
}
