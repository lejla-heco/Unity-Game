using Spacecraft.Consts;
using Spacecraft.ScriptableObjects;
using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
	public class LevelGenerator : MonoBehaviour
	{
		[SerializeField] private ChunkGeneratorData GeneratorData;
		[SerializeField] private Transform ChunksParent;

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
			ChunksShuffleBag = new ShuffleBag<GameObject>();
			foreach (var ChunkData in GeneratorData.Chunks)
			{
				ChunksShuffleBag.Add(ChunkData.Chunk, ChunkData.NumberOfOccurances);
			}


			LevelPool = new ObjectPool<GameObject>();

			for (int i = 0; i < 50; i++)
			{
				var Object = Instantiate(ChunksShuffleBag.Next(), Vector3.zero, Quaternion.identity, ChunksParent);
				Object.SetActive(false);
				LevelPool.Add(Object);
			}

			for (int i = 0; i < GameConsts.InitialChunksNumber; i++)
			{
				PickAndSpawnChunk();
			}

		}
		private void PickAndSpawnChunk()
		{
			var PooledChunk = LevelPool.PickChunkFromPool();
			Debug.Log("About to spawn chunk at: " + (ActiveChunkCount * GameConsts.ChunkLength) + GameConsts.ChunkGenerationOffset);
			PooledChunk.transform.position = new Vector3(0, 0, (ActiveChunkCount * GameConsts.ChunkLength) + GameConsts.ChunkGenerationOffset);
			PooledChunk.SetActive(true);
			ActiveChunkCount++;
		}

		public void ResetChunkNumbers(int n = 0)
		{
			ActiveChunkCount = n;
		}
	}
}
