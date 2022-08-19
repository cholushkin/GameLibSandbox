using GameLib.Log;
using UnityEngine;

interface ITestScenario
{
    void Execute();
}


public abstract class TestScenario : MonoBehaviour, ITestScenario
{
    public LogChecker Log;
    public abstract void Execute();
    public override string ToString()
    {
        return GetType() + JsonUtility.ToJson(this, true);
    }
}