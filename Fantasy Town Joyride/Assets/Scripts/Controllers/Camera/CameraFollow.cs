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