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
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("ExitTrigger"))
			{
					OnChunkExited();
					StartCoroutine(WaitAndDeactivate(other.gameObject));
					Debug.Log("generisi novi");
			}
		}

		IEnumerator WaitAndDeactivate(GameObject other)
		{
			yield return new WaitForSeconds(Delay);
			other.transform.parent.gameObject.SetActive(false);
		}
		
	}
}
