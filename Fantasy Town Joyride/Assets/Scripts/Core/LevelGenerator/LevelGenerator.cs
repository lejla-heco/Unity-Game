using System.Collections.Generic;
using Spacecraft.Consts;
using Spacecraft.ScriptableObjects;
using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
	public class LevelGenerator : MonoBehaviour
	{
		[SerializeField]
		private Transform ChunksParent;
		[SerializeField]
		private ObstacleCollection Obstacles;
		[SerializeField]
		private GameObject DefaultLevelChunk;

		private ObjectPool<GameObject> LevelPool { get; set; }

		private int ActiveChunkCount = 0;
		private float[] Lanes =
		{
			3.4f, 0, -3
		};

		private void OnEnable()
		{
			TriggerExit.OnChunkExited += PickAndSpawnChunk;
		}
		private void OnDisable()
		{
			TriggerExit.OnChunkExited -= PickAndSpawnChunk;
		}
		private void Start()
		{

			LevelPool = new ObjectPool<GameObject>();

			for (int i = 0; i < 8; i++) // how many level segments we want
			{
				// take an empty chunk and generate obstacles on it
				LevelPool.Add(GenerateObstaclesOnChunk(
					Instantiate(DefaultLevelChunk, Vector3.zero, Quaternion.identity, ChunksParent
					)));
			}


			for (int i = 0; i < GameConsts.InitialChunksNumber; i++)
			{
				PickAndSpawnChunk();
			}

		}
		private void PickAndSpawnChunk()
		{
			var PooledChunk = LevelPool.PickChunkFromPool();
			PooledChunk.transform.position = new Vector3(0, 0, (ActiveChunkCount * GameConsts.ChunkLength) + GameConsts.ChunkGenerationOffset);
			PooledChunk.SetActive(true);
			Debug.Log("ActiveChunkCount: " + ActiveChunkCount);
			ActiveChunkCount++;
		}

		public void ResetChunkNumbers(int n = 0)
		{
			ActiveChunkCount = n;
		}


		private GameObject GenerateObstaclesOnChunk(GameObject chunk)
		{
			for (int j = 0; j < 6; j++) // sada je 6 zbog toga sto je razmak 15 izmedju svake -> 6 * 15 = 90 a duzina chunk-a je 100
			{
				var NextIndex = GameConsts.Rnd.Next(Obstacles.Items.Count);
				var NextLane = GameConsts.Rnd.Next(Lanes.Length);
				var Obstacle = Obstacles.Items[NextIndex];

				Instantiate(
					Obstacle.GetObject(),
					new Vector3(Lanes[NextLane], 0, 15 * j - 50),
					Obstacle.GetRotation(),
					chunk.transform);
			}

			chunk.SetActive(false);
			return chunk;
		}
	}
}
