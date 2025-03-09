using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    private int _atackID = 0;

    [SerializeField]
    private Animator _weaponAnimator;

    [SerializeField]
    private int _attackCount = 3;

    private PlayerData _playerData;

    [SerializeField]
    private Transform _cursorHit;

    private void Start()
    {
        _playerData = FindObjectOfType<PlayerData>();

        _cursorHit.gameObject.SetActive(false);
    }

    private void Update()
    {
        _weaponAnimator.SetFloat("Speed", _playerData._movementDirection.sqrMagnitude);
        _weaponAnimator.SetBool("CanSprint", _playerData.IsSprinting() && _playerData._playerSystems.StaminaNotNull() && !_playerData.CanAimig());
    }

    public void PlayAttackAnim()
    {
        if (_playerData._movementDirection != Vector3.zero)
            _atackID++;
        
        if (_atackID >= _attackCount || _playerData._movementDirection == Vector3.zero)
            _atackID = 0;

        _weaponAnimator.SetFloat("AttackID", _atackID);
        _weaponAnimator.CrossFade("Attacking", 0.1f);
    }

    public void CursorHit()
    {
        _cursorHit.gameObject.SetActive(true);

        Invoke("ResetCursorHit", 0.3f);
    }

    private void ResetCursorHit()
    {
        _cursorHit.gameObject.SetActive(false);
    }
}