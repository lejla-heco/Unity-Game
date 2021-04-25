using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spacecraft
{
public class FloatingOrigin : MonoBehaviour
{
    public float Threshold = 100.0f;
    public LevelGenerator LayoutGenerator;

    void LateUpdate()
    {
        float CameraParentZ = this.transform.parent.gameObject.transform.position.z;
        Vector3 CameraPosition = gameObject.transform.position;
        
        if (CameraPosition.z > Threshold)
        {
            var Scene = SceneManager.GetActiveScene();
            Vector3 PositionZ = new Vector3(0, 0, CameraParentZ);
            foreach (GameObject g in Scene.GetRootGameObjects())
            {
                g.transform.position -= PositionZ;
            }
            LayoutGenerator.UpdateChunkCount();
        }

    }
}
}