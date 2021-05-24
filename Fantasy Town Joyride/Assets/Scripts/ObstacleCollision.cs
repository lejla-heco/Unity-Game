using System.Collections;
using Spacecraft.Controllers.Core.LevelGenerator;
using System.Collections.Generic;
using Spacecraft.Controllers.Core.Entities;
using UnityEditor.UI;
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

		[SerializeField] private RawImage firstLife;
		[SerializeField] private RawImage secondLife;
		[SerializeField] private RawImage thirdLife;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Barrel") || other.gameObject.CompareTag("Hidrant") ||
			    other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("StreetSign"))
			{
				LoseLife();
				Debug.Log("Ima zivota = " + lives);

			}
			else if (other.gameObject.CompareTag("GasTank"))
			{
				firstLife.color = Color.black;
				secondLife.color = Color.black;
				thirdLife.color = Color.black;
				Die();
			}
		}


		public void LoseLife()
		{
			lives--;
			if (lives == 2) firstLife.color = Color.black;
			else if (lives == 1) secondLife.color = Color.black;
			else if (lives == 0)
			{
				thirdLife.color = Color.black;
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
