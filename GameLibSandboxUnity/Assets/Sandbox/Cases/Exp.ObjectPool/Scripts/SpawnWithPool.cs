using System.Collections;
using System.Collections.Generic;
using GameLib;
using UnityEngine;

public class SpawnWithPool : Spawner
{
    protected override void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        PoolManager.SpawnObject(prefab, position, rotation);
    }
}
