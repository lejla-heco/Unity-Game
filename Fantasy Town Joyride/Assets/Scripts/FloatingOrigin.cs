using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spacecraft
{
[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    public float threshold = 100.0f;
    public LevelLayoutGenerator layoutGenerator;

    void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;

        if (cameraPosition.z > threshold)
        {

            for (int z = 0; z < SceneManager.sceneCount; z++)
            {
                foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects())
                {
                    g.transform.position -= cameraPosition;
                }
            }

            Vector3 originDelta = Vector3.zero - cameraPosition;
            layoutGenerator.UpdateSpawnOrigin(originDelta);
        }

    }
}
}