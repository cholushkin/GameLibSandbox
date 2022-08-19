using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Despawn : MonoBehaviour
{
    public float Timeout = -1;
    private float _time = -1;

    private void OnEnable()
    {
        _time = Time.time;
    }

    void Update ()
    {
        if (Timeout <= 0)
            return;

        if (Time.time - _time >= Timeout)
            DespawnObject();

	}

    public abstract void DespawnObject();
}
