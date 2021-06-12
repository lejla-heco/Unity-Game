using System;
using UnityEngine;


namespace Spacecraft.Core.Entities

{
    public class TrackedEntity : MonoBehaviour
    {
        public static bool IsProtected = false;
        public static bool IsGameOver = false;
        public static int Points;
        public int Lives = 3;

        public void InitializeGameStats()
        {
            Points = 0;
            IsGameOver = false;
            IsProtected = false;
            Lives = 3;
        }

        public int LoseLife(int howManyLives = 1)
        {
            Lives -= howManyLives;
            if (Lives <= 0)
            {
                Die();
            }

            return Lives;
        }


        private void Die()
        {
            IsGameOver = true;
        }
    }
}