using System.Collections;
using System.Collections.Generic;
using GameLib.Log;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitTestRunner : MonoBehaviour
{
    public LogChecker Log;
    public bool MarkStatus;
    public bool MoveFailsUpToHierarchy;
    private Test[] _tests;
    private List<Transform> _ok;
    private List<Transform> _fail;

    void Awake()
    {
        Assert.raiseExceptions = true;
        _tests = GetComponentsInChildren<Test>();
    }

    void Start()
    {
        StartCoroutine(ExecuteTests());
    }

    IEnumerator ExecuteTests()
    {
        _ok = new List<Transform>();
        _fail = new List<Transform>();

        Debug.LogFormat(">>>>> UnitTestRunner: '{0}'. Running '{1}' tests", name, _tests.Length);

        foreach (var test in _tests)
        {
            test.Run();
            Debug.LogFormat("Test '{0}': errors {1}", test.gameObject.name, test.ErrorsCounter);
            yield return null;
        }

        if(MarkStatus)
            MarkTransforms();
    }

    #region text marking
    private static readonly string StatusOK = "[OK] - ";
    private static readonly string StatusFail = "[Fail] - ";

    private void MarkTransforms()
    {
        foreach (var t in _ok)
            SetMark(t, StatusOK);
        foreach (var t in _fail)
            SetMark(t, StatusFail);
    }

    private void SetMark(Transform from, string status)
    {
        Transform curTransform = from;
        while (curTransform != null)
        {
            OverrideStatus(curTransform, status);
            if (curTransform == transform)
                break;
            if (status == StatusFail && MoveFailsUpToHierarchy)
                curTransform.SetAsFirstSibling();
            curTransform = curTransform.parent;
        }
    }

    public void MarkError(Transform errorTransform)
    {
        if (!MarkStatus)
            return;
        _fail.Add(errorTransform);
    }

    public void MarkOk(Transform okTransform)
    {
        if (!MarkStatus)
            return;
        _ok.Add(okTransform);
    }

    private static void OverrideStatus(Transform transform, string newStatus)
    {
        transform.gameObject.name = transform.gameObject.name.Replace(StatusFail, "");
        transform.gameObject.name = transform.gameObject.name.Replace(StatusOK, "");
        transform.gameObject.name = newStatus + transform.gameObject.name;
    }
    #endregion
}
