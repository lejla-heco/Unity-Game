using Spacecraft.Core.Entities;
using UnityEngine.SceneManagement;

namespace Spacecraft.UI
{
    public class GameOverMenu : TrackedEntity
    {
        public void Restart()
        {
            SceneManager.LoadScene("GameScene");
            InitializeGameStats();
        }


        public void Exit()
        {
            SceneManager.LoadScene("MainMenu");
            InitializeGameStats();
        }
    }
}