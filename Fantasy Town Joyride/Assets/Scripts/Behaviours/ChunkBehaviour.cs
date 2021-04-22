using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Behaviours
{
    public class ChunkBehaviour : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Invoke(nameof(ResetChunk), 5);
        }

        private void ResetChunk()
        {
            this.gameObject.SetActive(false);
            this.gameObject.transform.position = Vector3.zero;
        }
    }
}
