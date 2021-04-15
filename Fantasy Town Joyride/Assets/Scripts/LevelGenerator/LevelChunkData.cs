using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.LevelGenerator
{
[CreateAssetMenu(menuName = "LevelChunkData")]
public class LevelChunkData : ScriptableObject
{
    public Vector2 chunkSize = new Vector2(15f, 100f);
    public GameObject LevelChunk;
}
}