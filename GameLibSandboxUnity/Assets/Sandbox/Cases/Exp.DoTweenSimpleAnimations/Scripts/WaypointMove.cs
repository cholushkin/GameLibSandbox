using System.Linq;
using DG.Tweening;
using UnityEngine;

public class WaypointMove : MonoBehaviour
{
    public Transform[] Path;
    public float Duration;
    public PathMode Mode;
    public Transform Target;

    void Start()
    {
        var v3path = Path.Select(x => x.position).ToArray();
        Target.DOPath(v3path, Duration, PathType.CatmullRom, Mode).SetLoops(-1, LoopType.Yoyo);
    }
}
