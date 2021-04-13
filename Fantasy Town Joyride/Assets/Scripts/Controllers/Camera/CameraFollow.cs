using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Controllers.Camera
{
public class CameraFollow : MonoBehaviour
{
    private Rigidbody RigidbodyComponent;

    [SerializeField] private float ForwardSpeed;

    void Start()
    {
        RigidbodyComponent = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RigidbodyComponent.MovePosition(RigidbodyComponent.position + transform.forward * ForwardSpeed * Time.deltaTime);
    }
}
}