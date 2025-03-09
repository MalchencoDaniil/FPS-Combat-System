using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;

    private float _constMaxY, _rotationX, _rotationY;

    [Header("References")]
    [SerializeField] private SettingsData _settings;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _cameraHandler;
    [SerializeField] private bool _canRotateCameraTarget = true;

    [Header("Camera Stats")]
    [SerializeField] public float _maxY = 62;
    [SerializeField] public float _minY = -80;
    [SerializeField] public float _maxX = 360;
    [SerializeField] public float _minX = -360;

    private void Start()
    {
        _constMaxY = _maxY;
    }

    private void Update()
    {
        MyInput();
        CameraMove();
    }

    private void MyInput()
    {
        Vector2 _mouseLook = InputManager._instance._inputActions.Camera.Look.ReadValue<Vector2>();

        _horizontalInput = _mouseLook.x * _settings._mouseSensetivity * Time.deltaTime;
        _verticalInput = _mouseLook.y * _settings._mouseSensetivity * Time.deltaTime;
    }

    private void CameraMove()
    {
        _maxY = Mathf.Lerp(_maxY, _constMaxY, 6 * Time.deltaTime);

        FPSCamera();
    }

    private void FPSCamera()
    {
        transform.position = _cameraHandler.position;

        _rotationX += _horizontalInput * 5.0f;
        _rotationY += _verticalInput * 5.0f;

        _rotationX = ClampAngleFPS(_rotationX, _minX, _maxX);
        _rotationY = ClampAngleFPS(_rotationY, _minY, _maxY);

        Quaternion _xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
        Quaternion _yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);
        Quaternion _finalRotation = Quaternion.identity * _xQuaternion * _yQuaternion;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, _finalRotation, Time.deltaTime * 15.0f);

        if (_canRotateCameraTarget)
            _target.Rotate(Vector3.up * _horizontalInput);
    }

    private float ClampAngleFPS(float _angle, float _min, float _max)
    {
        if (_angle < -360)
            _angle += 360;

        if (_angle > 360)
            _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max);
    }
}