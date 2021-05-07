using System;
using System.Collections;
using System.Collections.Generic;
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

		private ShuffleBag<GameObject> ChunksShuffleBag;

		private ObjectPool<GameObject> LevelPool { get; set; }

		private int ChunkCount = 0;
		private int ChunksToSpawn = 2;
		private int ChunkLength = 50;
		// private int CurrentChunk = 0;

		private GameObject Player;

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

			if (Player == null)
			{
				Player = GameObject.FindGameObjectWithTag("Player");
			}

			ChunksShuffleBag = new ShuffleBag<GameObject>();
			for (int i = 0; i < GeneratorData.Chunks.Count; i++)
			{
				ChunksShuffleBag.Add(GeneratorData.Chunks[i].Chunk, GeneratorData.Chunks[i].NumberOfOccurances);
			}

			LevelPool = new ObjectPool<GameObject>();

			for (int i = 0; i < 50; i++)
			{
				var Object = Instantiate(ChunksShuffleBag.Next(), Vector3.zero, Quaternion.identity, ChunksParent);
				Object.SetActive(false);
				LevelPool.Add(Object);
			}

			for (int i = 0; i < ChunksToSpawn; i++)
			{
				PickAndSpawnChunk();
			}

		}
		private void PickAndSpawnChunk()
		{
			int nextZPosition = ChunkCount * ChunkLength;
			var PooledChunk = LevelPool.PickChunkFromPool();
			// while (PooledChunk.activeSelf)
			// {
			// 	PooledChunk = LevelPool.PickChunkFromPool();
			// }
			PooledChunk.transform.position = new Vector3(0, 0, nextZPosition);
			PooledChunk.SetActive(true);
			ChunkCount++;
		}
		private void Update()
		{
			// ∂CurrentChunk = (int)Math.Floor(Player.transform.position.z / 100);
			// Debug.Log("Current chunk is " + CurrentChunk + " - Z: " + Player.transform.position.z);
		}


		public void ResetChunkNumbers(int n = 0)
		{
			ChunkCount = n;
		}
	}
}
