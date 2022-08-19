using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LocalMoveUp : MonoBehaviour
{
    public float amplitude;
    public Vector3 MovingDirection = Vector3.up; // global

    void Awake()
    {
        print(name);
        

        // move in global space
        //var dest = transform.position + Vector3.up * amplitude;
        //transform.DOMove(dest, 2f);
        //print("destination: " + dest);
        //print("local destination: " + transform.InverseTransformPoint(dest));

        //// move in local space
        //var dest = transform.position + Vector3.up * amplitude;
        //var destlocal = transform.InverseTransformPoint(dest);
        //////print(destlocal);

        //
        // var lV3 = transform.localRotation * Vector3.up * amplitude;

        var lV3 = transform.localRotation * MovingDirection * amplitude;
        lV3 = Vector3.Scale(lV3, transform.localScale);
        transform.DOLocalMove(transform.localPosition + lV3, 2f);
    }
}
