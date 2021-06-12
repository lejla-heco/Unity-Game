using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spacecraft.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            // SceneManager.LoadScene("Scenes/GameScene");
            StartCoroutine(AsynchronousLoad("Scenes/GameScene"));
        }


        IEnumerator AsynchronousLoad(string scene)
        {
            yield return null;
            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;
            while (!ao.isDone)
            {
                // [0, 0.9] > [0, 1]
                float progress = Mathf.Clamp01(ao.progress / 0.9f);
                Debug.Log("Loading progress: " + (progress * 100) + "%");
                // Loading completed
                if (ao.progress == 0.9f)
                {
                    Debug.Log("Press a key to start");
                    // if (Input.anyKey)
                    ao.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}