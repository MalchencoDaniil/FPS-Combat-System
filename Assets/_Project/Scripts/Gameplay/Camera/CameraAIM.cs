using UnityEngine;
using Cinemachine;

public class CameraAIM : MonoBehaviour
{
    internal bool _canAIM = false;
    private float _fovOnStart;
    private CinemachineVirtualCamera _mainCamera;


    [SerializeField] private SettingsData _cameraSettings;
    [SerializeField] private float _aimingSpeed = 8;

    private void Start()
    {
        _fovOnStart = _cameraSettings._cameraFOV;

        _mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        _canAIM = Aiming();

        _cameraSettings._cameraFOV = _canAIM ? Mathf.Lerp(_cameraSettings._cameraFOV, _fovOnStart - _cameraSettings._cameraFOV / 2.5f, _aimingSpeed * Time.deltaTime) : 
            Mathf.Lerp(_cameraSettings._cameraFOV, _fovOnStart, _aimingSpeed * Time.deltaTime);
        _mainCamera.m_Lens.FieldOfView = _cameraSettings._cameraFOV;
    }

    private bool Aiming() { return InputManager._instance._inputActions.Camera.Aim.IsPressed(); }
}