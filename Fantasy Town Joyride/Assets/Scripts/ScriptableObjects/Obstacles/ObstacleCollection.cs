using System.Collections.Generic;
using Spacecraft.Consts;
using UnityEngine;

namespace Spacecraft.ScriptableObjects.Obstacles
{
	[CreateAssetMenu(fileName = "ObstacleCollection", menuName = "Fantasy Town Joyride/Obstacle Collection", order = 0)]
	public class ObstacleCollection : ScriptableObject
	{
		[SerializeField]
		public List<Obstacle> Items;


		public Obstacle GetRandomObstacle(int Level)
		{
			Obstacle? Obs = null;
			while (Obs == null)
			{
				var Index = GameConsts.Rnd.Next(Items.Count);
				var Temp = Items[Index];
				if (Level >= Temp.GetMinLevelForObstacleToAppear())
				{
					Obs = Temp;
				}
			}
			return Obs;
		}

	}
}
