using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
[CreateAssetMenu(menuName = "LevelChunkData")]
public class LevelChunkData : ScriptableObject
{
    public Vector2 chunkSize = new Vector2(15f, 100f);
    public GameObject LevelChunk;
}
}