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

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				if (!Exited)
				{
					Exited = true;
					OnChunkExited();
					WaitAndDeactivate();
				}
			}
		}

		void WaitAndDeactivate()
		{
			transform.parent.gameObject.SetActive(false);
		}

	}
}
