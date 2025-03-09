using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
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
        _weaponAnimator.SetFloat("AttackID", Random.Range(0, _attackCount));
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