using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.LevelGenerator
{
public class ObjectPool : MonoBehaviour
{
    
    [SerializeField]
    private Transform parentTransform;

    [SerializeField] private LevelChunkData[] Chunks;

    private List<LevelChunkData> Prefabs { get; set; }

    private int Index { get; set; } = 0;

    private void Start()
    {
        Prefabs = new List<LevelChunkData>();
        for (int i = 0; i < Chunks.Length; i++)
        {
            Prefabs.Add(Instantiate(Chunks[i], parentTransform));
        }
    }

    public LevelChunkData PickChunkFromPool()
    {
        var PooledObject = Prefabs[Index++ % Prefabs.Count];
        return PooledObject;
    }
}
}
