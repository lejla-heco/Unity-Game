using System.Collections;
using Spacecraft.Controllers.Core.LevelGenerator;
using System.Collections.Generic;
using Spacecraft.Controllers.Core.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Spacecraft
{
    public class ObstracleCollision : TrackedEntity
    {
        //public ParticleSystem ps;
        //[SerializeField]
        //private SpacecraftController Movement;
        public static bool gameOver = false;
        private static int lives = 3;
        //public Text livesText;
        
        private void OnTriggerEnter(Collider other)
        {
            SpacecraftTag spacecraftTag = other.GetComponent<SpacecraftTag>();
            if (spacecraftTag != null)
            {
                LoseLife();
                Debug.Log("Ima zivota = " + lives);
                
            }
        }
       

        public void LoseLife()
        {
            lives--;
            //livesText.text = "Lives: " + lives;
            if (lives == 0)
            {
                Die();
            }
        }
        private void Die()
        {
            gameOver = true;
            IsPaused = true;

        }
    }
}
