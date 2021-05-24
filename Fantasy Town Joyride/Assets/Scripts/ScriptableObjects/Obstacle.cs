using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Spacecraft.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Obstacle", menuName = "Fantasy Town Joyride/Obstacle")]
	public class Obstacle : ScriptableObject
	{
		[SerializeField]
		private GameObject Data;

		[SerializeField]
		private Vector3 Rotation;

		public GameObject GetObject()
		{
			return Data;
		}

		public Quaternion GetRotation()
		{
			return Quaternion.Euler(Rotation);
		}
	}
}
