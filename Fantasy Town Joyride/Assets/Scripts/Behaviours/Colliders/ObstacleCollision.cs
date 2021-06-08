using System;
using System.Collections;
using Spacecraft.Core.LevelGenerator;
using System.Collections.Generic;
using Spacecraft.Core.Entities;
using UnityEditor.AssetImporters;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Spacecraft
{
	public class ObstacleCollision : TrackedEntity
	{
		private int Lives = 3;
		[SerializeField] private GameObject CollisionEffect;
		[SerializeField] private GameObject ExplosionEffect;

		[SerializeField] private RawImage FirstLife;
		[SerializeField] private RawImage SecondLife;
		[SerializeField] private RawImage ThirdLife;

		private Renderer Renderer;
		private Color[] RegularColors;
		[SerializeField] private Material Mat1;

		private void Start()
		{
			Renderer = GetComponent<Renderer>();
			RegularColors = new Color[Renderer.materials.Length];
			
			for(int i = 0; i < Renderer.materials.Length; i++)
				RegularColors[i] = Renderer.materials[i].color;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Barrel") || other.gameObject.CompareTag("Hidrant") ||
			    other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("StreetSign") || other.gameObject.CompareTag("RoadBlock"))
			{
				LoseLife();
			}
			else if (other.gameObject.CompareTag("GasTank"))
			{
				FirstLife.color = Color.black;
				SecondLife.color = Color.black;
				ThirdLife.color = Color.black;
				Die();
			}
		}


		public void LoseLife()
		{
			if (IsProtected) return;
			
			Lives--;
			if (Lives == 2)
			{
				FirstLife.color = Color.black;
				ActivateEffect(CollisionEffect);
				StartCoroutine(Flasher());
			}
			else if (Lives == 1)
			{
				SecondLife.color = Color.black;
				ActivateEffect(CollisionEffect);
				StartCoroutine(Flasher());
			}
			else if (Lives == 0)
			{
				ThirdLife.color = Color.black;
				Die();
			}
		}
		private void Die()
		{
			if (IsProtected) return;
			ActivateEffect(ExplosionEffect);
			IsPaused = true;
		}

		private void ActivateEffect(GameObject effect)
		{
			effect.SetActive(true);
			StartCoroutine(Deactivate(effect));
		}

		IEnumerator Deactivate(GameObject effect)
		{
			yield return new WaitForSeconds(0.5f);
			effect.SetActive(false);
		}
		
		IEnumerator Flasher() 
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < Renderer.materials.Length; j++)
				{
					Renderer.materials[j].color = Mat1.color;
				}
				yield return new WaitForSeconds(.1f);
				for (int j = 0; j < Renderer.materials.Length; j++)
				{
					Renderer.materials[j].color = RegularColors[j];
				}
				yield return new WaitForSeconds(.1f);
			}
		}
		
	}
}
