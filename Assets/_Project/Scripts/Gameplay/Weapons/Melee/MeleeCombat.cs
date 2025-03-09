using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponAnimator))]
public class MeleeCombat : MonoBehaviour
{
    private float _timeToAttack = 0f;

    private WeaponAnimator _weaponAnimator;

    [Header("Combat Settings")]
    [SerializeField, Range(0, 2)]
    private float _animWaitTime = 0.5f;

    [SerializeField]
    private float _attackDamage = 10;

    [SerializeField]
    private float _attackReloadTime = 2;

    [SerializeField]
    private float _attackRadius = 3;

    [SerializeField]
    private Transform _attackPoint;

    [SerializeField]
    private LayerMask _enemyLayer;

    [Header("Audio and Sound")]
    [SerializeField]
    private AudioSource _playerAudioSource;

    [SerializeField]
    private List<AudioClip> _swingClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> _smashClips = new List<AudioClip>();

    [Header("Editor")]
    [SerializeField]
    private Color _gizmosColor;

    private void Start()
    {
        _weaponAnimator = GetComponent<WeaponAnimator>();
    }

    private bool CanAttack()
    {
        return InputManager._instance._inputActions.Player.Attack.triggered && _timeToAttack <= 0f;
    }

    private void Update()
    {
        _timeToAttack -= Time.deltaTime;

        if (CanAttack())
        {
            Attack();
        }
    }

    private void Attack()
    {
        _weaponAnimator.PlayAttackAnim();
        _timeToAttack = _attackReloadTime;

        _playerAudioSource.PlayOneShot(_swingClips[Random.Range(0, _swingClips.Count)]);
    }

    public void CheckHit()
    {
        Collider[] _hitEnemys = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _enemyLayer);

        foreach (Collider _enemyCollider in _hitEnemys)
        {
            Enemy _enemy = _enemyCollider.gameObject.GetComponent<Enemy>();
            _enemy.TakeHit(_attackDamage);

            _weaponAnimator.CursorHit();

            _playerAudioSource.PlayOneShot(_smashClips[Random.Range(0, _smashClips.Count)]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawSphere(_attackPoint.position, _attackRadius);
    }
}