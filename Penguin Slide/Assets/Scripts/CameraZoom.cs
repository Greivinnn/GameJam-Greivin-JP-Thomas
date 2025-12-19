using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CameraZoom : MonoBehaviour
{
[SerializeField] private int targetZoom = 40;
[SerializeField] private CinemachineCamera cinemachineCamera = null;
private int defaultZoom = 0;
private PlayerInput input = null;
private InputAction zoomOutAction = null;
[SerializeField] private Transform zoomOutLocation = null;
private Transform player = null;
    void Awake()
    {
        input = new PlayerInput();
        zoomOutAction = input.UI.ZoomOut;
        zoomOutAction.started += StartZoomOutAction;
        zoomOutAction.canceled += EndZoomOutAction;
        defaultZoom = GetComponent<PixelPerfectCamera>().assetsPPU;
    }

    void OnEnable()
    {
        input.Enable();
        zoomOutAction.Enable();
    }

    void OnDisable()
    {
        input.Disable();
        zoomOutAction.Disable();
    }

    void StartZoomOutAction(InputAction.CallbackContext context)
    {
        player = cinemachineCamera.Follow;
        cinemachineCamera.Follow = zoomOutLocation;
        GetComponent<PixelPerfectCamera>().assetsPPU = targetZoom;
    }

    void EndZoomOutAction(InputAction.CallbackContext context)
    {
        GetComponent<PixelPerfectCamera>().assetsPPU = defaultZoom;
        cinemachineCamera.Follow = player;
    }
}
