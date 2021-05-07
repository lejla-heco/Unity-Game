using System;
using System.Collections;
using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
	public class TriggerExit : MonoBehaviour
	{
		public float Delay = 1f;

		public delegate void ExitAction();
		public static event ExitAction OnChunkExited;

		private bool Exited = false;

		private GameObject Player;

		private void Start()
		{
			Player = GameObject.Find("Player");
		}

		private void OnTriggerExit(Collider other)
		{
			Debug.Log("On Trigger Exi - Player Position: " + Player.transform.position.z);
			SpacecraftTag spacecraftTag = other.GetComponent<SpacecraftTag>();
			if (spacecraftTag != null)
			{
				if (!Exited)
				{
					Exited = true;
					OnChunkExited();
					StartCoroutine(WaitAndDeactivate());
				}
			}
		}

		IEnumerator WaitAndDeactivate()
		{
			yield return new WaitForSeconds(Delay);

			//transform.root.gameObject.SetActive(false);
			transform.parent.gameObject.SetActive(false);
			// Debug.Log("I should destroy current exiting game object now");
			// Destroy(gameObject);
		}



	}
}
