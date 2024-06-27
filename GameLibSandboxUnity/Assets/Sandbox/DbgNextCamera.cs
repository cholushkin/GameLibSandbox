using System.Collections.Generic;
using System.Linq;
using GameLib.Dbg;
using UnityEngine;

public class DbgNextCamera : Pane
{
    private Camera _currentCamera;
    private List<Camera> _camerasPool;
    private Camera _mainCamera;
    private static DbgNextCamera _staticInstance;

    public static DbgNextCamera GetStaticInstance()
    {
        return _staticInstance;
    }


    public override void InitializeState()
    {
        base.InitializeState();
        _staticInstance = this;
        _camerasPool = new List<Camera>();
    }

    public void RegisterCamera(Camera camera, bool main = false)
    {
        RemoveNulls();
        
        if (main)
            _mainCamera = camera;
        _camerasPool.Add(camera);
        _camerasPool = _camerasPool.OrderBy(x => x.gameObject.GetComponent<RegisterCameraToCameraSwitcher>().Priority).ToList();
        SetCamera(_camerasPool[0]);
        Debug.Log($"Cameras registered: {_camerasPool.Count}");
        RefreshText();
    }

    public override void Update()
    {
        base.Update();
        if (_currentCamera == null && _mainCamera != null)
        {
            SetCamera(_mainCamera);
        }
    }

    private void SetCamera(Camera camera)
    {
        _currentCamera = camera;
        foreach (var cam in _camerasPool)
        {
            cam.gameObject.SetActive(false);
        }
        _currentCamera.gameObject.SetActive(true);
    }

    public override void OnClick()
    {
        base.OnClick();
        RemoveNulls();
        var curCameraIndex = _camerasPool.IndexOf(_currentCamera);
        var nextCameraIndex = (curCameraIndex + 1) % _camerasPool.Count;
        SetCamera(_camerasPool[nextCameraIndex]);
        RefreshText();
    }

    void RefreshText()
    {
        var curCameraIndex = _camerasPool.IndexOf(_currentCamera);
        var nextCameraIndex = (curCameraIndex + 1) % _camerasPool.Count;
        SetText($"<b>[{_camerasPool[curCameraIndex].name}]</b>\nnext:{_camerasPool[nextCameraIndex].name}\n{curCameraIndex+1} of {_camerasPool.Count}");
    }

    void RemoveNulls()
    {
        _camerasPool.RemoveAll(item => item == null);
    }
}
