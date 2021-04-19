using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft
{
    [CreateAssetMenu(fileName = "Chunk data", menuName = "Fantasy Town Joyride / Chunk data")]
    public class ChunkData : ScriptableObject
    {
        public GameObject Chunk;
        public int NumberOfOccurances;
    }
}
