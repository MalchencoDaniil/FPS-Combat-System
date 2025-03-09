using UnityEngine;

[RequireComponent((typeof(CharacterController)))]
public class PlayerData : MonoBehaviour
{
    [Header("References")]
    public CharacterController _characterController;
    public PlayerSystems _playerSystems;

    [Header("Movement Settings")]
    [SerializeField] internal float _walkSpeed = 4;
    [SerializeField] internal float _sprintSpeed = 6;
    internal Vector3 _movementDirection, _playerVelocity;

    [Header("Ground Detection")]
    [SerializeField] private float _groundDistance = 0.1f;
    [SerializeField] private LayerMask _whatIsGround;

    [Header("Audio")]
    [SerializeField] internal AudioSource _footstepAudioSource;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    internal bool IsGrounded()
    {
        return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + (_characterController.height / 2) * -1, transform.position.z), _groundDistance, _whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + (_characterController.height / 2) * -1, transform.position.z), _groundDistance);
    }

    internal bool CanAimig()
    {
        return InputManager._instance._inputActions.Camera.Aim.IsPressed();
    }

    internal bool IsSprinting()
    {
        return InputManager._instance._inputActions.Player.Sprint.IsPressed() && _movementDirection != Vector3.zero;
    }
}