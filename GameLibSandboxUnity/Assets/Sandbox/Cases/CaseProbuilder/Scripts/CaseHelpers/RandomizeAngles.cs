using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAngles : MonoBehaviour
{
    public List<Transform> Targets;
    
    void Start()
    {
        foreach (var target in Targets)
        {
            target.rotation =
                Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        }
    }
}
