using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWithInstance : Spawner
{
    protected override void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(prefab, position, rotation);
    }
}
