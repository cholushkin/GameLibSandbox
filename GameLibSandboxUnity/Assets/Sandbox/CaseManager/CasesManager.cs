using GameLib.Log;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class CasesManager : MonoBehaviour
{
    public int CurrentCaseIndex;
    public SceneAsset[] CaseScenes;
    private SceneAsset _currentScene;
    public LogChecker Log;

    void Start()
    {
        RunCase(CurrentCaseIndex);
    }

    public void RunCase(int caseIndex = -1)
    {
        // Update CurrentCaseIndex
        if (caseIndex == -1)
            caseIndex = CurrentCaseIndex;
        CurrentCaseIndex = caseIndex;

        // Destroy old scene
        var oldCurScene = _currentScene;
        _currentScene = CaseScenes[CurrentCaseIndex];

        //SceneLoader.Instance.Replace(_currentScene, oldCurScene, true);
    }

    [Button]
    void test()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("ProBuilderRndShapes"));
        print(SceneManager.GetActiveScene().name);
    }

    //public string GetNextCaseName()
    //{
    //    var nextIndex = (CurrentCaseIndex + 1) % CasesHolder.Order.Length;
    //    return CasesHolder.Order[nextIndex].Name;
    //}

    //public string GetPrevCaseName()
    //{
    //    var prevIndex = (CurrentCaseIndex - 1);
    //    if (prevIndex < 0)
    //        prevIndex = CasesHolder.Order.Length - 1;
    //    return CasesHolder.Order[prevIndex].Name;
    //}

    //public string GetCurrentCaseName()
    //{
    //    return CasesHolder.Order[CurrentCaseIndex].Name;
    //}

    public void IncIndex()
    {
        CurrentCaseIndex = (CurrentCaseIndex + 1) % GetCasesCount();
    }

    public void DecIndex()
    {
        CurrentCaseIndex = (CurrentCaseIndex - 1);
        if (CurrentCaseIndex < 0)
            CurrentCaseIndex = GetCasesCount() - 1;
    }

    public int GetCasesCount()
    {
        return CaseScenes.Length;
    }


}
