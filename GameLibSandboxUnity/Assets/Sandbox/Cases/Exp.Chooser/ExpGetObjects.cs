using GameLib;
using UnityEngine;

public class ExpGetObjects : MonoBehaviour
{
    public CyclerType Cycler;
    public int CyclesCount;
    public int MaxValuesAmount;

    private Chooser<Progression> _chooser;

    void Awake()
    {
        _chooser = new Chooser<Progression>(
            GetComponentsInChildren<Progression>(),
            Cycler,
            CyclesCount,
            MaxValuesAmount
        );
    }

    void Update()
    {
        var comp = _chooser.GetCurrent();
        if(comp == null)
            return;
        _chooser.Step();
        Debug.LogFormat("Getting value from chooser: '{0}'", comp);
        comp.Progress();
    }
}
