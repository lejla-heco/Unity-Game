using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft
{
    public class PickUpEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip PickUpSound;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("SpacecraftObject"))
            {
                PlayCoinPickUpSound();
            }
        }
        private void PlayCoinPickUpSound()
        {
            AudioSource.PlayClipAtPoint(PickUpSound, this.gameObject.transform.position);
        }
        
    }
}
