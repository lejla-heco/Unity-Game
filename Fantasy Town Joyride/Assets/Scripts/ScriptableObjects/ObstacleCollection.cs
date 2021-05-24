using System.Collections.Generic;
using UnityEngine;
namespace Spacecraft.ScriptableObjects
{
	[CreateAssetMenu(fileName = "ObstacleCollection", menuName = "Fantasy Town Joyride/Obstacle Collection", order = 0)]
	public class ObstacleCollection : ScriptableObject
	{
		[SerializeField]
		public List<Obstacle> Items;
	}
}
