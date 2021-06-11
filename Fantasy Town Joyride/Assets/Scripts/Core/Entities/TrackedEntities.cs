using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Spacecraft.Core.Entities

{
    public class TrackedEntity : MonoBehaviour
    {
        public static bool IsPaused = false;
        public static bool IsProtected = false;
        public static bool IsGameOver = false;

        public int Lives = 3;
    }
}