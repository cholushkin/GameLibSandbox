using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWithDestroy : Despawn
{
    public override void DespawnObject()
    {
        Destroy(gameObject);
    }
}
