using System.Linq;
using GameLib.Log;
using UnityEngine;
using UnityEngine.Assertions;

public class CasesManager : MonoBehaviour
{
 //    public SceneDependenciesConfig SceneDependenciesConfig;
 //    public SceneLoader SceneLoader;
 //    public LogChecker Log;
 //    private string _currentSceneName;
 //    private string _previousSceneName;
 //
	// void Start()
 //    {
 //        RunCase(GetNextSceneName());
 //    }
 //
 //    public void RunCase(string sceneName)
 //    {
	//     _previousSceneName = _currentSceneName;
	// 	SceneLoader.LoadSequence(sceneName);
 //    }
 //
 //    public string GetNextSceneName()
 //    {
	// 	// If _currentSceneName is not initialized yet then return the first scene from SceneDependenciesConfig
	// 	if (string.IsNullOrEmpty(_currentSceneName))
	// 	    return SceneDependenciesConfig.AllSceneDependencies[0].DevSceneOrWildcard;
 //
	//     int index = SceneDependenciesConfig.AllSceneDependencies.Select((p, idx) => new { p, idx })
	// 	    .FirstOrDefault(x => x.p.DevSceneOrWildcard == _currentSceneName)?.idx ?? -1;
 //        Assert.IsTrue(index != -1);
 //
	// 	var nextIndex = (index + 1) % SceneDependenciesConfig.AllSceneDependencies.Length;
	// 	return SceneDependenciesConfig.AllSceneDependencies[nextIndex].DevSceneOrWildcard;
 //    }
}
