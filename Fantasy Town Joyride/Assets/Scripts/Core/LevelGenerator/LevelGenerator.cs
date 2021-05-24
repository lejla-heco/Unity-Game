using System.Collections.Generic;
using Spacecraft.Consts;
using Spacecraft.ScriptableObjects;
using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
	public class LevelGenerator : MonoBehaviour
	{
		[SerializeField]
		private ChunkGeneratorData GeneratorData;
		[SerializeField]
		private Transform ChunksParent;
		[SerializeField]
		private ObstacleCollection Obstacles;
		[SerializeField]
		private GameObject DefaultLevelChunk;

		private ShuffleBag<GameObject> ChunksShuffleBag;
		private ObjectPool<GameObject> LevelPool { get; set; }

		private int ActiveChunkCount = 0;

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
				// take empty chunk and generate obstacles on it
				var Segment = Instantiate(DefaultLevelChunk);
				for (int j = 0; j < 10; j++)
				{
					var NextIndex = GameConsts.Rnd.Next(Obstacles.Items.Count);
					Debug.Log(NextIndex);
					Instantiate(Obstacles.Items[NextIndex].Data, new Vector3(0, 0, 15 * j), Quaternion.identity, Segment.transform);
				}
				Segment.SetActive(false);
				LevelPool.Add(Segment);
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
			ActiveChunkCount++;
		}

		public void ResetChunkNumbers(int n = 0)
		{
			ActiveChunkCount = n;
		}



		private GameObject GenerateObstaclesOnChunk(GameObject Chunk, int Difficulty = 1)
		{
			return Chunk;
		}
	}
}
