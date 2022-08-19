using GameLib;
using Sandbox;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeController : MonoBehaviour
{
    public CaseGamepadStretchControls Controls;
    public Transform CubeAiming;
    public Transform CubeMoving;

    private Vector3 _cubeMovingInitialPosition;
    private Vector3 _cubeAimingInitialPosition;
    private Vector3 _cubeAimingInitialScale;

    void Awake()
    {
        Controls = new CaseGamepadStretchControls();
        Controls.Enable();
        InitializeCubes();

        Controls.PlayerBaseControls.TimeStopHold.started += TimeStopHoldStarted;
        Controls.PlayerBaseControls.TimeStopHold.performed += TimeStopHoldPerformed;
        Controls.PlayerBaseControls.TimeStopHold.canceled += TimeStopHoldCanceled;
    }

    private void Update()
    {
        const float amplitude = 2f;
        // print($@"MOVE[{Controls.PlayerBaseControls.Move.phase}] {Controls.PlayerBaseControls.Move.ReadValue<Vector2>()}  STRETCH[{Controls.PlayerBaseControls.Stretch.phase}]{Controls.PlayerBaseControls.Stretch.ReadValue<Vector2>()}");

        CubeAiming.transform.position = _cubeAimingInitialPosition +
                                        Controls.PlayerBaseControls.Stretch.ReadValue<Vector2>().ToVector3(0f) *
                                        amplitude;
        CubeMoving.transform.position = _cubeMovingInitialPosition +
                                        Controls.PlayerBaseControls.Move.ReadValue<Vector2>().ToVector3(0f) *
                                        amplitude;
    }

    private void TimeStopHoldCanceled(InputAction.CallbackContext context)
    {
        Controls.PlayerBaseControls.Move.Enable();
        Controls.PlayerBaseControls.Stretch.Disable();
        StartMovingAimingCube();
    }

    private void TimeStopHoldStarted(InputAction.CallbackContext context)
    {
        Controls.PlayerBaseControls.Stretch.Enable();
        Controls.PlayerBaseControls.Move.Disable();
    }

    private void TimeStopHoldPerformed(InputAction.CallbackContext context)
    {
        StopMovingAiminCube();
    }

    private void InitializeCubes()
    {
        Controls.PlayerBaseControls.Stretch.Disable();
        Controls.PlayerBaseControls.Move.Enable();
        _cubeAimingInitialPosition = CubeAiming.transform.position;
        _cubeAimingInitialScale = CubeAiming.transform.localScale;
        _cubeMovingInitialPosition = CubeMoving.transform.position;
    }

    private void StartMovingAimingCube()
    {
        CubeAiming.transform.localScale = Vector3.one * 0.75f;
    }

    private void StopMovingAiminCube()
    {
        CubeAiming.transform.localScale = _cubeAimingInitialScale;
    }
}
