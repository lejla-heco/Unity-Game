using System;
using System.Collections;
using System.Collections.Generic;
using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;

namespace Spacecraft
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
        private int ChunkCount = 0;
        private void Start()
        {
            ChunksShuffleBag = new ShuffleBag<GameObject>();
            for (int i = 0; i < GeneratorData.Chunks.Count; i++)
            {
                ChunksShuffleBag.Add(GeneratorData.Chunks[i].Chunk, GeneratorData.Chunks[i].NumberOfOccurances);
            }
        }

        private void Update()
        {
            if (Time.time > TimeForNextChunk)
            {
                TimeForNextChunk = Time.time + ChunkGenerationSpeed;
                Instantiate(ChunksShuffleBag.Next(), new Vector3(0, 0, ChunkCount * 100), Quaternion.identity);
                ChunkCount++;
            }
        }
    }
}
