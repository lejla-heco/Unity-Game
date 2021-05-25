using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Spacecraft.ScriptableObjects.Obstacles
{
	[CreateAssetMenu(fileName = "Obstacle", menuName = "Fantasy Town Joyride/Obstacle")]
	public class Obstacle : ScriptableObject
	{
		[SerializeField]
		private GameObject Data;

		[SerializeField]
		private Vector3 Rotation;

		[SerializeField]
		private int MinLevelForObstacleToAppear = 0;

		public GameObject GetObject()
		{
			return Data;
		}

		public Quaternion GetRotation()
		{
			return Quaternion.Euler(Rotation);
		}

		public int GetMinLevelForObstacleToAppear()
		{
			return MinLevelForObstacleToAppear;
		}
	}
}
