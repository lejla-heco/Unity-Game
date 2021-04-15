using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.LevelGenerator
{
    public class TriggerExit : MonoBehaviour
    {
        public float delay = 1f;
    
        public delegate void ExitAction();
        public static event ExitAction OnChunkExited;

        private bool exited = false;

        private void OnTriggerExit(Collider other)
        {
            SpacecraftTag spacecraftTag = other.GetComponent<SpacecraftTag>();
            if (spacecraftTag != null)
            {
                if (!exited)
                {
                    exited = true;
                    OnChunkExited();
                    StartCoroutine(WaitAndDeactivate());
                }


            }
        }

        IEnumerator WaitAndDeactivate()
        {
            yield return new WaitForSeconds(delay);

            //transform.root.gameObject.SetActive(false);
            this.transform.parent.gameObject.SetActive(false);
        }



    }
}