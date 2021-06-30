using System;
using System.Collections;
using System.Security.Cryptography;
using Spacecraft.Consts;
using Spacecraft.Controllers.Player;
using UnityEngine;

namespace Spacecraft.Core.LevelGenerator
{
    public class TriggerExit : MonoBehaviour
    {
        public delegate void ExitAction();

        public static event ExitAction OnChunkExited;


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnChunkExited();

                if (transform.parent.gameObject.tag == "Chunk0")
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}