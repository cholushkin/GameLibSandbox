using System.Collections;
using System.Collections.Generic;
using GameLib;
using UnityEngine;

public class DespawnWithPool : Despawn
{
    public override void DespawnObject()
    {
        if(gameObject.activeInHierarchy)
            PoolManager.ReleaseObject(gameObject);
    }
}
