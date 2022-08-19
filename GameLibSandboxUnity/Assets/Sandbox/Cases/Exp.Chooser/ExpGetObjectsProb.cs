using GameLib;
using UnityEngine;

public class ExpGetObjectsProb : MonoBehaviour
{
    public CyclerProbType Cycler;
    public int CyclesCount;
    public int MaxValuesAmount;
    public float[] Probs;

    private ChooserProb<Progression> _chooser;

    void Awake()
    {
        _chooser = new ChooserProb<Progression>(
            GetComponentsInChildren<Progression>(),
            Probs,
            Cycler,
            CyclesCount,
            MaxValuesAmount
        );
    }

    void Update()
    {
        var comp = _chooser.GetCurrent();
        if (comp == null)
            return;
        Debug.LogFormat("Getting value from prob chooser: '{0}'", comp);
        _chooser.Step();
        comp.Progress();
    }
}
