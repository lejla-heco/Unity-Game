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
        private float ChunkGenerationSpeed;
        [SerializeField]
        private Transform ChunksParent;
        
        private ShuffleBag<GameObject> ChunksShuffleBag;
        private float TimeForNextChunk;
        
        private ObjectPool<GameObject> LevelPool { get; set; }
        
        private GameObject PooledChunk { get; set; }
        
        private int ChunkCount = 0;
        public int ChunksToSpawn = 3;
        
        void OnEnable()
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
            for (int i = 0; i < GeneratorData.Chunks.Count; i++)
            {
                ChunksShuffleBag.Add(GeneratorData.Chunks[i].Chunk, GeneratorData.Chunks[i].NumberOfOccurances);
            }

            LevelPool = new ObjectPool<GameObject>();

            for (int i = 0; i < 100; i++)
            {
                LevelPool.Add(Instantiate(ChunksShuffleBag.Next(), Vector3.zero, Quaternion.identity, ChunksParent));
            }
            
            for (int i = 0; i < ChunksToSpawn; i++)
            {
                PickAndSpawnChunk();   
            }
            
        }
        
        void PickAndSpawnChunk()
        {
            PooledChunk = LevelPool.PickChunkFromPool();

            PooledChunk.transform.position = new Vector3(0, 0, ChunkCount * 100);
            PooledChunk.SetActive(true);
                
            ChunkCount++;
        }

        /*private void Update()
        {
            if (Time.time > TimeForNextChunk)
            {
                TimeForNextChunk = Time.time + ChunkGenerationSpeed;
                PooledChunk = LevelPool.PickChunkFromPool();

                PooledChunk.transform.position = new Vector3(0, 0, ChunkCount * 100);
                PooledChunk.SetActive(true);
                
                ChunkCount++;
            }
        }*/
    }
}
