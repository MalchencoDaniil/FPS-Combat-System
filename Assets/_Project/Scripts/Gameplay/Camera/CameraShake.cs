using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _startPosition;

    private float _inputMagnitude;

    [SerializeField, Range(0, 12)] private float _frequency = 10.0f;
    [SerializeField, Range(0, 5)] public float _amount = 0.002f;
    [SerializeField, Range(0, 12)] private float _smooth = 10.0f;

    [SerializeField] private PlayerData _playerData;

    [Header("Amount")]
    [SerializeField, Range(0.001f, 0.1f)] private float _walkCameraShake = 0.02f;
    [SerializeField, Range(0.001f, 0.1f)] private float _runCameraShake = 0.03f;

    private void Start()
    {
        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        CheckForHeadBobTrigger();

        StopHeadBob();

        _amount = _playerData.IsSprinting() && !_playerData.CanAimig() ? _runCameraShake : _walkCameraShake;
    }

    private void CheckForHeadBobTrigger()
    {
        Vector2 _input = InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();

        _inputMagnitude = new Vector3(_input.x, 0, _input.y).magnitude;

        if (_inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 _position = Vector3.zero;

        _position.y += Mathf.Lerp(_position.y, Mathf.Sin(Time.time * _frequency) * _amount * 1.4f, _smooth * Time.deltaTime);
        _position.x += Mathf.Lerp(_position.x, Mathf.Cos(Time.time * _frequency / 2f) * _amount * 1.6f, _smooth * Time.deltaTime);
        transform.localPosition += _position;

        return _position;
    }

    public void StopHeadBob()
    {
        if (transform.localPosition == _startPosition)
            return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, _startPosition, 1 * Time.deltaTime);
    }
}