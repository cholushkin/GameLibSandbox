using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    // TEST What happen if you let the engine breathe a bit between spawns
    public void SpawnAll(GameObject prefab, int spawnAmount, float spawnOffset, float spawnInterval)
    {
        if (spawnInterval > 0)
            StartCoroutine(SpawnAllWithIntervalCoroutine(prefab, spawnAmount, spawnOffset, spawnInterval));
        else
            SpawnAll(prefab, spawnAmount, spawnOffset);
    }

    private void SpawnAll(GameObject prefab, int spawnAmount, float spawnOffset)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnObject(prefab, new Vector3(i * spawnOffset, i * spawnOffset, i * spawnOffset), Quaternion.identity);
        }
    }

    private IEnumerator SpawnAllWithIntervalCoroutine(GameObject prefab, int spawnAmount, float spawnOffset, float spawnInterval)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnObject(prefab, new Vector3(i * spawnOffset, i * spawnOffset, i * spawnOffset), Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    protected abstract void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation);
}
