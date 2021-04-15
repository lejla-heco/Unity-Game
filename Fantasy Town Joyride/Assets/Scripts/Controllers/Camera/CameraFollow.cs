using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Controllers.Camera
{
public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    Transform target;
    void Update()
    {
        transform.position = target.position;
    }
}
}