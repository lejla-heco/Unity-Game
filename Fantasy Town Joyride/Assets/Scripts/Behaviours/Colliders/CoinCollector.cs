using System;
using System.Collections;
using System.Collections.Generic;
using Spacecraft.Core.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spacecraft
{
	public class CoinCollector : TrackedEntity
	{
		private int Delay;
		private int Points;
		private int ThisGamesPoints = 0;
		[SerializeField] private AudioClip CoinPickUpSound;
		[SerializeField] private TextMeshProUGUI CollectedCoins;
		private void Start()
		{
			Points = 0; //PlayerPrefs.GetInt("CollectedMoney", 0);
			Delay = 2;
			CollectedCoins.text = "Coins: " + Points.ToString();
		}

		private void Update()
		{
			if (IsPaused) PlayerPrefs.SetInt("CollectedMoney", Points + ThisGamesPoints);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("BigGem"))
			{
				PlayCoinPickUpSound(other);
				other.gameObject.SetActive(false);
				ThisGamesPoints += 2;
				Reactivate(other);

			}

			if (other.gameObject.CompareTag("SmallGem"))
			{
				PlayCoinPickUpSound(other);
				other.gameObject.SetActive(false);
				ThisGamesPoints += 1;
				Reactivate(other);
			}

			int Result = Points + ThisGamesPoints;
			CollectedCoins.text = "Coins: " + Result.ToString();
		}

		IEnumerator Reactivate(Collider other)
		{
			yield return new WaitForSeconds(Delay);
			other.gameObject.SetActive(true);
		}

		private void PlayCoinPickUpSound(Collider other)
		{
			AudioSource.PlayClipAtPoint(CoinPickUpSound, other.gameObject.transform.position);
		}
	}
}
