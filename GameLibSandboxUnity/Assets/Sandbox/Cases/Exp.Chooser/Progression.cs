using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    public float _progression;

    public void Progress()
    {
        _progression += 0.1f;
    }
}
