using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class Movement : MonoBehaviour
{
    private PlayerData _playerData;

    private const float GRAVITY_FORCE = -16;

    private void Start()
    {
        _playerData = GetComponent<PlayerData>();
    }

    private Vector2 MovementInput()
    {
        return InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();
    }

    private void Update()
    {
        float _targetSpeed = _playerData.IsSprinting() && _playerData._playerSystems.StaminaNotNull() && !_playerData.CanAimig() ? _playerData._sprintSpeed : _playerData._walkSpeed;
        _playerData._footstepAudioSource.pitch = _playerData.IsSprinting() && _playerData._playerSystems.StaminaNotNull() && !_playerData.CanAimig() ? 1f : 0.8f;

        _playerData._movementDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(MovementInput().x, 0, MovementInput().y).normalized;

        ApplyGravity();
        ApplyStamina();

        _playerData._characterController.Move(_playerData._movementDirection * _targetSpeed * Time.deltaTime);
    }

    private void ApplyStamina()
    {
        if (_playerData.IsSprinting())
            _playerData._playerSystems.TakeStamina(0.3f);
        else
            _playerData._playerSystems.StaminaHeal(0.4f);
    }

    private void ApplyGravity()
    {
        if (_playerData.IsGrounded() && _playerData._playerVelocity.y < 0)
        {
            _playerData._playerVelocity.y = -1;
        }
        else
        {
            _playerData._playerVelocity.y += GRAVITY_FORCE * Time.deltaTime;
        }

        _playerData._characterController.Move(_playerData._playerVelocity * Time.deltaTime);
    }
}