using System;
using UnityEngine;
using Assets.Plugins.Alg;

public class Test : MonoBehaviour
{
    public int ErrorsCounter { get; set; }

    [TextArea]
    public string Description;

    public void Run()
    {
        var scenarios = GetComponentsInChildren<TestScenario>();
        Debug.LogFormat(">>> Test: {0}. Test scenarios: {1}", name, scenarios.Length);

        foreach (var scenario in scenarios)
        {
            try
            {
                Debug.Log($"> Scenario: {scenario.name}");
                scenario.Execute();
                GetMyTestRunner().MarkOk(scenario.transform);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("Scenario: '{0}'; Error: '{1}'",
                    scenario.transform.GetDebugName(),
                    ex.Message
                    );

                ++ErrorsCounter;
                GetMyTestRunner().MarkError(scenario.transform);
            }
        }
    }

    private UnitTestRunner GetMyTestRunner()
    {
        return GetComponentInParent<UnitTestRunner>();
    }
}
