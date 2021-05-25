using UnityEngine;
namespace Spacecraft.ScriptableObjects.Gems
{
	[CreateAssetMenu(fileName = "Gem", menuName = "Fantasy Town Joyride/Gem")]
	public class Gem : ScriptableObject
	{
		[SerializeField]
		private GameObject Object;

		[SerializeField]
		private int Points;

		public GameObject GetObject()
		{
			return Object;
		}

		public int GetPoints()
		{
			return Points;
		}
	}
}
