using System;
using System.Collections;
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
				transform.parent.gameObject.SetActive(false);
			}
		}


	}
}
