using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
public class LevelLayoutGenerator : MonoBehaviour
{
    //public LevelChunkData[] levelChunkData;

    //private LevelChunkData previousChunk;

    public Vector3 spawnOrigin;

    private Vector3 spawnPosition;
    public int chunksToSpawn = 10;

    void OnEnable()
    {
        //TriggerExit.OnChunkExited += PickAndSpawnChunk;
    }

    private void OnDisable()
    {
        //TriggerExit.OnChunkExited -= PickAndSpawnChunk;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //PickAndSpawnChunk();
        }
    }

    void Start()
    {

        for (int i = 0; i < chunksToSpawn; i++)
        {
           // PickAndSpawnChunk();
        }
    }
    
  /*  void PickAndSpawnChunk()
    {
        LevelChunkData chunkToSpawn = levelChunkData[Random.Range(0, levelChunkData.Length)];
        previousChunk = chunkToSpawn;
        Instantiate(chunkToSpawn.LevelChunk, spawnPosition + spawnOrigin, Quaternion.identity, this.transform);
        spawnPosition = spawnPosition + new Vector3(0f, 0f, previousChunk.chunkSize.y);
    }*/

    public void UpdateSpawnOrigin(Vector3 originDelta)
    {
        spawnOrigin = spawnOrigin + originDelta;
    }
}
}