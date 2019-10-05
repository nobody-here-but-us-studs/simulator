/**
 * Copyright (c) 2019 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public enum CameraStateType
{
    Free,
    Follow
};

public class SimulatorCameraController : MonoBehaviour
{
    private SimulatorControls controls;
    private Vector2 directionInput;
    private float elevationInput;
    private Vector2 mouseInput;
    private float mouseLeft;
    private float mouseRight;
    //private float isMouseMiddle;
    //private Vector2 mouseScroll;
    private float zoomInput;

    private Camera thisCamera;
    private Transform pivot;
    
    private float freeSpeed = 10f;
    private float followSpeed = 25f;
    private float boost = 0f;
    private float targetTiltFree = 0f;
    private float targetLookFree = 0f;
    private Quaternion mouseFollowRot = Quaternion.identity;
    private bool inverted = true;
    private bool defaultFollow = true;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 lastZoom = Vector3.zero;
    public Transform targetObject;

    public Vector3 Offset = new Vector3(0f, 1.15f, 0f);

    public CameraStateType CurrentCameraState = CameraStateType.Free;

    private void Awake()
    {
        thisCamera = GetComponentInChildren<Camera>();

        controls = SimulatorManager.Instance.controls;

        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux && Application.isEditor)
        {
            // empty
        }
        else
        {
            controls.Camera.Direction.started += ctx => directionInput = ctx.ReadValue<Vector2>();
            controls.Camera.Direction.performed += ctx => directionInput = ctx.ReadValue<Vector2>();
            controls.Camera.Direction.canceled += ctx => directionInput = Vector2.zero;
            controls.Camera.Elevation.started += ctx => elevationInput = ctx.ReadValue<float>();
            controls.Camera.Elevation.performed += ctx => elevationInput = ctx.ReadValue<float>();
            controls.Camera.Elevation.canceled += ctx => elevationInput = 0f;

            controls.Camera.Zoom.started += ctx => zoomInput = ctx.ReadValue<float>();
            controls.Camera.Zoom.performed += ctx => zoomInput = ctx.ReadValue<float>();
            controls.Camera.Zoom.canceled += ctx => zoomInput = 0f;

            controls.Camera.Boost.performed += ctx => boost = ctx.ReadValue<float>();
            controls.Camera.Boost.canceled += ctx => boost = ctx.ReadValue<float>();
        }

        controls.Camera.MouseDelta.started += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Camera.MouseDelta.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Camera.MouseDelta.canceled += ctx => mouseInput = Vector2.zero;

        controls.Camera.MouseLeft.performed += ctx => mouseLeft = ctx.ReadValue<float>();
        controls.Camera.MouseLeft.canceled += ctx => mouseLeft = ctx.ReadValue<float>();
        controls.Camera.MouseRight.performed += ctx => mouseRight = ctx.ReadValue<float>();
        controls.Camera.MouseRight.canceled += ctx => mouseRight = ctx.ReadValue<float>();

        //controls.Camera.MouseMiddle.performed += ctx => ResetFollowRotation();

        // TODO broken in package currently https://github.com/Unity-Technologies/InputSystem/issues/647
        //controls.Camera.MouseScroll.started += ctx => mouseScroll = ctx.ReadValue<Vector2>();
        //controls.Camera.MouseScroll.performed += ctx => mouseScroll = ctx.ReadValue<Vector2>();
        //controls.Camera.MouseScroll.canceled += ctx => mouseScroll = Vector2.zero;
    }

    private void Start()
    {
        targetTiltFree = transform.eulerAngles.x;
        targetLookFree = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux && Application.isEditor)
        {
            // this is a temporary workaround for Unity Editor on Linux
            // see https://issuetracker.unity3d.com/issues/linux-editor-keyboard-when-input-handling-is-set-to-both-keyboard-input-stops-working

            if (Input.GetKeyDown(KeyCode.A)) directionInput.x -= 1;
            else if (Input.GetKeyUp(KeyCode.A)) directionInput.x += 1;

            if (Input.GetKeyDown(KeyCode.D)) directionInput.x += 1;
            else if (Input.GetKeyUp(KeyCode.D)) directionInput.x -= 1;

            if (Input.GetKeyDown(KeyCode.W))
            {
                zoomInput += 1;
                directionInput.y += 1;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                zoomInput -= 1;
                directionInput.y -= 1;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                zoomInput -= 1;
                directionInput.y -= 1;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                zoomInput += 1;
                directionInput.y += 1;
            }

            if (Input.GetKeyDown(KeyCode.E)) elevationInput -= 1;
            else if (Input.GetKeyUp(KeyCode.E)) elevationInput += 1;

            if (Input.GetKeyDown(KeyCode.Q)) elevationInput += 1;
            else if (Input.GetKeyUp(KeyCode.Q)) elevationInput -= 1;

            if (Input.GetKeyDown(KeyCode.LeftShift)) boost += 1;
            else if (Input.GetKeyUp(KeyCode.LeftShift)) boost -= 1;
        }

        switch (CurrentCameraState)
        {
            case CameraStateType.Free:
                UpdateFreeCamera();
                break;
            case CameraStateType.Follow:
                UpdateFollowCamera();
                break;
        }
    }
    
    private void UpdateFreeCamera()
    {
        if (mouseRight == 1)
        {
            targetLookFree += mouseInput.x * 0.25f;
            targetTiltFree += mouseInput.y * 0.1f * (inverted ? -1 : 1);
            targetTiltFree = Mathf.Clamp(targetTiltFree, -90, 90);
            mouseFollowRot = Quaternion.Euler(targetTiltFree, targetLookFree, 0f);
            transform.rotation = mouseFollowRot;
        }

        transform.position = Vector3.MoveTowards(transform.position, (transform.rotation * new Vector3(directionInput.x, elevationInput, directionInput.y)) + transform.position, Time.unscaledDeltaTime * freeSpeed * (boost == 1 ? 10f : 1f));
    }
    
    private void UpdateFollowCamera()
    {
        Debug.Assert(targetObject != null);

        //var dist = Vector3.Distance(thisCamera.transform.position, targetObject.position);
        //if (dist < 3)
        var position = InputTracking.GetLocalPosition(XRNode.CenterEye);
        thisCamera.transform.localPosition = thisCamera.transform.InverseTransformPoint(targetObject.position);
        //else if (dist > 30)
        //    thisCamera.transform.localPosition = Vector3.MoveTowards(thisCamera.transform.localPosition, thisCamera.transform.InverseTransformPoint(targetObject.position), Time.unscaledDeltaTime);
        //else if (zoomInput != 0)
        //    thisCamera.transform.localPosition = Vector3.MoveTowards(thisCamera.transform.localPosition, thisCamera.transform.InverseTransformPoint(targetObject.position), Time.unscaledDeltaTime * zoomInput * 10f * (boost == 1 ? 10f : 1f));

        //if (mouseRight == 1)
        //{
        //    defaultFollow = false;
        //    targetLookFree += mouseInput.x * 0.25f;
        //    targetTiltFree += mouseInput.y * 0.1f * (inverted ? -1 : 1);
        //    targetTiltFree = Mathf.Clamp(targetTiltFree, -15, 65);
        //    mouseFollowRot = Quaternion.Euler(targetTiltFree, targetLookFree, 0f);
        //    transform.localRotation = mouseFollowRot;
        //}
        //else
        //{
            //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (mouseFollowRot * targetObject.forward), followSpeed * Time.unscaledDeltaTime, 1f)); // TODO new state for follow camera at mouse rotation else mouseFollowRot
        if (defaultFollow)
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetObject.forward, followSpeed * Time.unscaledDeltaTime, 1f));

        targetTiltFree = transform.eulerAngles.x;
        targetLookFree = transform.eulerAngles.y;

        if (targetTiltFree > 180)
            targetTiltFree -= 360;

        transform.position = targetObject.position - position + Offset;
    }

    public void SetFollowCameraState(GameObject target)
    {
        Debug.Assert(target != null);
        CurrentCameraState = CameraStateType.Follow;
        targetObject = target.transform;
        transform.position = targetObject.position;
        transform.rotation = targetObject.rotation;
        thisCamera.transform.localRotation = Quaternion.identity;
        thisCamera.transform.localPosition = Vector3.zero;
        thisCamera.transform.localPosition = thisCamera.transform.InverseTransformPoint(targetObject.position);
        defaultFollow = true;
        targetTiltFree = transform.eulerAngles.x;
        targetLookFree = transform.eulerAngles.y;
        SimulatorManager.Instance.UIManager?.SetCameraButtonState();
    }
}
