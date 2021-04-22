using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Spacecraft.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChunkGeneratorData", menuName = "Fantasy Town Joyride/Chunk Generator data")]
    public class ChunkGeneratorData : ScriptableObject
    {
        [ReorderableList]
        public List<ChunkData> Chunks;
    }
}
