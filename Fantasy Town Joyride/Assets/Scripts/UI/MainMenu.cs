using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Spacecraft.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private RawImage load1;
        [SerializeField] private RawImage load2;
        [SerializeField] private RawImage load3;
        [SerializeField] private RawImage load4;
        [SerializeField] private RawImage load5;
        [SerializeField] private RawImage load6;
        public void PlayGame()
        {
            StartCoroutine(AsynchronousLoad("Scenes/GameScene"));
        }

        public void OpenGarage()
        {
            StartCoroutine(AsynchronousLoad("Scenes/Garage"));
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        IEnumerator AsynchronousLoad(string scene)
        {
            yield return null;
            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;
            while (!ao.isDone)
            {
                float progress = Mathf.Clamp01(ao.progress / 0.9f);
                CheckProgressValue(progress);
                // Loading completed
                if (ao.progress == 0.9f)
                {
                    load5.gameObject.SetActive(false);
                    load6.gameObject.SetActive(true);
                    ao.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private void CheckProgressValue(float progress)
        {
            if (progress >= 0.2f && progress < 0.4f)
            {
                load1.gameObject.SetActive(false);
                load2.gameObject.SetActive(true);
            }
            else if (progress >= 0.4f && progress < 0.6f)
            {
                load2.gameObject.SetActive(false);
                load3.gameObject.SetActive(true);
            }
            else if (progress >= 0.6f && progress < 0.8f)
            {
                load3.gameObject.SetActive(false);
                load4.gameObject.SetActive(true);
            }
            else if (progress >= 0.8f && progress < 0.9f)
            {
                load4.gameObject.SetActive(false);
                load5.gameObject.SetActive(true);
            }
        }
        
    }
}