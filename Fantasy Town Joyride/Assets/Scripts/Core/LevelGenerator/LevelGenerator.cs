using System.Collections.Generic;
using JetBrains.Annotations;
using Spacecraft.Consts;
using Spacecraft.Core.LevelGenerator;
using Spacecraft.ScriptableObjects;
using Spacecraft.ScriptableObjects.Obstacles;
using UnityEngine;

namespace Spacecraft.Core.LevelGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Transform ChunksParent;
        [SerializeField] private GameAssetsCollection AssetsCollection;
        [SerializeField] private GameObject LevelWrapper;

        private int[] GeneratePowerUp = new int[]
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0};

        private int[] GenerateNewLife = new int[]
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0};

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

            for (int i = 0; i < 4; i++) // how many level segments we want
            {
                // take an empty chunk and generate obstacles on it
                LevelPool.Add(GenerateChunkWithObjects(
                    Instantiate(AssetsCollection.DefaultLevelChunk, Vector3.zero, Quaternion.identity, ChunksParent)));
            }

            var Chunk = GenerateChunkWithObjects(
                Instantiate(AssetsCollection.DefaultLevelChunk, Vector3.zero, Quaternion.identity, ChunksParent),0);
            
            // spawn first chunk with obstacle and other offset
            SpawnChunk(Chunk);

            for (int i = 1; i < GameConsts.InitialChunksNumber; i++)
            {
                PickAndSpawnChunk();
            }

            // generate player
            // load prefab
            var Id = PlayerPrefs.GetInt("ActiveSpeeder", 1);
            var LoadedSpeeder = AssetsCollection.GetSpeederById(Id);

            Instantiate(
                LoadedSpeeder.ShipModel,
                GameObject.FindWithTag("SpacecraftObject").transform
            );
        }

        private void PickAndSpawnChunk()
        {
            var PooledChunk = LevelPool.PickChunkFromPool();
            SpawnChunk(PooledChunk);
        }

        private void SpawnChunk(GameObject chunk)
        {
            chunk.transform.position = new Vector3(0, 0,
                (ActiveChunkCount * GameConsts.ChunkLength) + GameConsts.ChunkGenerationOffset);
            chunk.SetActive(true);
            ActiveChunkCount++;
        }

        public void ResetChunkNumbers(int n = 0)
        {
            ActiveChunkCount = n;
        }


        private GameObject GenerateChunkWithObjects(GameObject chunk, int chunkIndex = 1, int level = 1)
        {
            int ObstaclesPerLevelBase = 4;
            int CoinsPerLevelBase = 2;

            // if this is the very first chunk, set offset for generated stuff
            int FirstValue = chunkIndex == 0
                ? 1
                : 0;
            
            // chunk 2d array
            int[,] ChunkDataArray =
            {
                {
                    FirstValue, FirstValue, FirstValue
                },
                {
                    FirstValue, FirstValue, FirstValue
                },
                {
                    FirstValue, FirstValue, FirstValue
                },
                {
                    FirstValue, FirstValue, FirstValue
                },
                {
                    FirstValue, FirstValue, FirstValue
                },
                {
                    0, 0, 0
                },
                {
                    0, 0, 0
                },
                {
                    0, 0, 0
                },
                {
                    0, 0, 0
                },
                {
                    0, 0, 0
                }
            };

            // generate coins
            var GeneratedCoins = 0;
            while (GeneratedCoins < CoinsPerLevelBase * level)
            {
                var NextLane = GameConsts.Rnd.Next(Lanes.Length);
                var NextRow = GameConsts.Rnd.Next(10);

                // check if this position is free
                if (ChunkDataArray[NextRow, NextLane] != 0)
                {
                    continue;
                }

                ChunkDataArray[NextRow, NextLane] = 2;
                GeneratedCoins++;
                Instantiate(
                    AssetsCollection.Gems[0].GetObject(),
                    new Vector3(Lanes[NextLane], 1.5f, NextRow * 10 - 50),
                    Quaternion.identity,
                    chunk.transform
                );
            }

            // generate obstacles
            var GeneratedObstacles = 0;
            while (GeneratedObstacles < ObstaclesPerLevelBase * level)
            {
                var RandomObstacle = AssetsCollection.GetRandomObstacle(level);

                var NextLane = GameConsts.Rnd.Next(Lanes.Length);
                var NextRow = GameConsts.Rnd.Next(10);

                // check if this position is free
                if (ChunkDataArray[NextRow, NextLane] != 0)
                {
                    continue;
                }

                var Prev = (NextLane - 1) < 0 ? 2 : NextLane - 1;
                var Next = (NextLane + 1) > 2 ? 0 : NextLane + 1;
                // check if this position will block all paths
                if (ChunkDataArray[NextRow, Prev] != 0 && ChunkDataArray[NextRow, Next] != 0)
                {
                    continue;
                }

                ChunkDataArray[NextRow, NextLane] = 1;
                GeneratedObstacles++;

                Instantiate(
                    RandomObstacle.GetObject(),
                    new Vector3(Lanes[NextLane], 0, NextRow * 10 - 50),
                    RandomObstacle.GetRotation(),
                    chunk.transform
                );
            }

            //generate power up
            if (GeneratePowerUp[GameConsts.Rnd.Next(GeneratePowerUp.Length)] == 1)
            {
                var NextLane = GameConsts.Rnd.Next(Lanes.Length);
                var NextRow = GameConsts.Rnd.Next(10);

                if (ChunkDataArray[NextRow, NextLane] == 0)
                {
                    ChunkDataArray[NextRow, NextLane] = 3;
                    Instantiate(
                        AssetsCollection.PickUp,
                        new Vector3(Lanes[NextLane], 0.5f, NextRow * 10 - 50),
                        Quaternion.identity,
                        chunk.transform
                    );
                }
            }
            
            //generate new life
            
            if (GenerateNewLife[GameConsts.Rnd.Next(GenerateNewLife.Length)] == 1)
            {
                var NextLane = GameConsts.Rnd.Next(Lanes.Length);
                var NextRow = GameConsts.Rnd.Next(10);

                if (ChunkDataArray[NextRow, NextLane] == 0)
                {
                    ChunkDataArray[NextRow, NextLane] = 4;
                    Instantiate(
                        AssetsCollection.NewLife,
                        new Vector3(Lanes[NextLane], 1.3f, NextRow * 10 - 50),
                        Quaternion.Euler(-90,0,0),
                        chunk.transform
                    );
                }
            }

            chunk.SetActive(false);
            chunk.transform.SetParent(LevelWrapper.transform);
            return chunk;
        }
    }
}