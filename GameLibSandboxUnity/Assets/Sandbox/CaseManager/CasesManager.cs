using GameLib.Log;
using UnityEngine;

[RequireComponent(typeof(CasesHolder))]
public class CasesManager : MonoBehaviour
{
    public LogChecker Log;

    public int CurrentCaseIndex;
    public CasesHolder CasesHolder;
    private Case _currentCase;

    void Start()
    {
        RunCase(CurrentCaseIndex);
    }

    public void RunCase(int caseIndex = -1)
    {
        if (caseIndex == -1)
            caseIndex = CurrentCaseIndex;
        var casePrefab = CasesHolder.Order[caseIndex];
        if (_currentCase != null)
            Destroy(_currentCase.gameObject);
        Debug.Log($"Running case: {casePrefab.Name}. {casePrefab.Description}");
        _currentCase = Instantiate(casePrefab, transform);
    }

    public string GetNextCaseName()
    {
        var nextIndex = (CurrentCaseIndex + 1) % CasesHolder.Order.Length;
        return CasesHolder.Order[nextIndex].Name;
    }

    public string GetPrevCaseName()
    {
        var prevIndex = (CurrentCaseIndex - 1);
        if (prevIndex < 0)
            prevIndex = CasesHolder.Order.Length - 1;
        return CasesHolder.Order[prevIndex].Name;
    }

    public string GetCurrentCaseName()
    {
        return CasesHolder.Order[CurrentCaseIndex].Name;
    }

    public void IncIndex()
    {
        CurrentCaseIndex = (CurrentCaseIndex + 1) % CasesHolder.Order.Length;
    }

    public void DecIndex()
    {
        CurrentCaseIndex = (CurrentCaseIndex - 1);
        if (CurrentCaseIndex < 0)
            CurrentCaseIndex = CasesHolder.Order.Length - 1;
    }

    public int GetCasesCount()
    {
        return CasesHolder.Order.Length;
    }


}
