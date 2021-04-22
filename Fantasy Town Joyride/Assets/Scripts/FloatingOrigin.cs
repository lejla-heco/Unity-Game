using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spacecraft
{
[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    public float Threshold = 100.0f;
    public LevelGenerator LayoutGenerator;

    void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;

        if (cameraPosition.z > Threshold)
        {

            for (int z = 0; z < SceneManager.sceneCount; z++)
            {
                foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects())
                {
                    g.transform.position -= cameraPosition;
                }
            }

            Vector3 originDelta = Vector3.zero - cameraPosition;
            //LayoutGenerator.UpdateSpawnOrigin(originDelta);
        }

    }
}
}