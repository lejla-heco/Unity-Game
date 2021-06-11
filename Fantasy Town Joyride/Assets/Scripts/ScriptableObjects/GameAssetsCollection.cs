using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Spacecraft.Consts;
using Spacecraft.ScriptableObjects.Chunks;
using Spacecraft.ScriptableObjects.Gems;
using Spacecraft.ScriptableObjects.Obstacles;
using UnityEngine;

#nullable enable
namespace Spacecraft.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameAssetsCollection", menuName = "Fantasy Town Joyride/Game Assets Collection")]
    public class GameAssetsCollection : ScriptableObject
    {
        [SerializeField] public GameObject DefaultLevelChunk;

        [SerializeField] public GameObject PickUp;

        [SerializeField] public List<Obstacle> Items;

        [SerializeField] public List<Gem> Gems;

        public GameObject GetDefaultLevelChunk()
        {
            return DefaultLevelChunk;
        }

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