using System;
using System.Collections;
using System.Collections.Generic;
using GameLib;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Spawner InstanceSpawner;
    public Spawner PoolSpawner;

    public GameObject PrefabDestroy;
    public GameObject PrefabPool;

    public float SpawnOffset = 0.01f;
    public int SpawnAmount = 50;
    public float SpawnInterval = 0f;
    public float AutoDespawnInterval = 0f;
    private System.Diagnostics.Stopwatch _stopwatch;

    private void Update()
    {
        _stopwatch = new System.Diagnostics.Stopwatch();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PrefabPool.GetComponent<Despawn>().Timeout = AutoDespawnInterval;
            WarmPool(PrefabPool);
            Spawn(PoolSpawner, PrefabPool);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PrefabDestroy.GetComponent<Despawn>().Timeout = AutoDespawnInterval;
            Spawn(InstanceSpawner, PrefabDestroy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var objects = FindObjectsOfType<DespawnWithPool>();
            foreach (var o in objects)
            {
                o.DespawnObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var objects = FindObjectsOfType<DespawnWithDestroy>();
            foreach (var o in objects)
            {
                o.DespawnObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            var objects = FindObjectsOfType<Despawn>();
            foreach (var o in objects)
            {
                o.DespawnObject();
            }
        }
    }

    private void WarmPool(GameObject prefab)
    {
        try
        {
            _stopwatch.Start();
            float time = Time.time;
            PoolManager.WarmPool(prefab, SpawnAmount);
            _stopwatch.Stop();

            float elapsedTime = _stopwatch.ElapsedMilliseconds / 1000;
            Debug.LogFormat("Pool warmed {0} objects in {1} seconds", SpawnAmount, elapsedTime);
            _stopwatch.Reset();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Spawn(Spawner spawner, GameObject prefab)
    {
        _stopwatch.Start();
        float time = Time.time;
        spawner.SpawnAll(prefab, SpawnAmount, SpawnOffset, SpawnInterval);
        _stopwatch.Stop();

        float elapsedTime = _stopwatch.ElapsedMilliseconds / 1000;
        Debug.LogFormat("{0} spawned all {1} objects in {2} seconds", spawner.name, SpawnAmount, elapsedTime);
        _stopwatch.Reset();
    }
}
