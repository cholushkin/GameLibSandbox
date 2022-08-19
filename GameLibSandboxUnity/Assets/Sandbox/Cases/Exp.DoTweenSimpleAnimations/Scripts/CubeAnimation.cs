using DG.Tweening;
using UnityEngine;

public class CubeAnimation : MonoBehaviour
{
    void Start()
    {
        transform.DOShakeScale(3f).SetLoops(-1,LoopType.Restart);
    }

    void Update()
    {
        if (Input.anyKeyDown)
            transform.DOMove(transform.position + Vector3.up * 5, 2f); 
    }
}
