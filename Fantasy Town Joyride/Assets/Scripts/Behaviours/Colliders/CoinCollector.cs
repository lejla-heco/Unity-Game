using System.Collections;
using Spacecraft.Core.Entities;
using TMPro;
using UnityEngine;

namespace Spacecraft.Behaviours.Colliders
{
    public class CoinCollector : TrackedEntity
    {
        [SerializeField] private AudioClip CoinPickUpSound;
        [SerializeField] private TextMeshProUGUI CollectedCoins;

        private void Start()
        {
            Points = 0;
            CollectedCoins.text = "Coins: " + Points;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("BigGem"))
            {
                PlayCoinPickUpSound(other);
                other.gameObject.SetActive(false);
                Points += 2;
                Reactivate(other);
            }

            if (other.gameObject.CompareTag("SmallGem"))
            {
                PlayCoinPickUpSound(other);
                other.gameObject.SetActive(false);
                Points += 1;
                Reactivate(other);
            }

            int Result = Points;
            CollectedCoins.text = "Coins: " + Result;
        }

        IEnumerator Reactivate(Collider other)
        {
            yield return new WaitForSeconds(2);
            other.gameObject.SetActive(true);
        }

        private void PlayCoinPickUpSound(Collider other)
        {
            AudioSource.PlayClipAtPoint(CoinPickUpSound, other.gameObject.transform.position);
        }
    }
}