using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Spacecraft.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Obstacle", menuName = "Fantasy Town Joyride/Obstacle")]
	public class Obstacle : ScriptableObject
	{
		[SerializeField]
		public GameObject Data;

		[SerializeField]
		private int DefaultZPosition;

		public GameObject GetObject()
		{
			return Instantiate(Data);
		}
	}
}
