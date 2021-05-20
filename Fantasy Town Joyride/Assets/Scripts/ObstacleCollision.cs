using System.Collections;
using Spacecraft.Controllers.Core.LevelGenerator;
using System.Collections.Generic;
using Spacecraft.Controllers.Core.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Spacecraft
{
    public class ObstacleCollision : TrackedEntity
    {
        //public ParticleSystem ps;
        //[SerializeField]
        //private SpacecraftController Movement;
        public static bool gameOver = false;
        private int lives = 3;
        //public Text livesText;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                LoseLife();
                Debug.Log("Ima zivota = " + lives);
                
            }
            else if (other.gameObject.CompareTag("GasTank"))
            {
                Die();
            }
        }
       

        public void LoseLife()
        {
            lives--;
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
