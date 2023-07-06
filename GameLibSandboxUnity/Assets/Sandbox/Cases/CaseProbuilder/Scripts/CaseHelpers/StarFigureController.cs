using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFigureController : MonoBehaviour
{
    public List<RandomRotation> Rotations;
    public float Step;
    
    void Awake()
    {
        for (int i = 0; i < Rotations.Count; ++i)
        {
            Rotations[i].Seed = Step * i;
        }
    }
}
