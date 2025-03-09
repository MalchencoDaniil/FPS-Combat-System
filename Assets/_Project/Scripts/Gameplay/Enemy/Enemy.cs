using System.Collections.Generic;
using UnityEngine;

public class Enemy : Death
{
    internal bool _canLive = true;

    [Header("References")]
    [SerializeField]
    private HealthSystem _enemyHealthSystem;

    [SerializeField]
    private Collider[] _enemyColliders;

    [Header("Animation Settings")]
    [SerializeField]
    private Animator _enemyAnimator;

    [SerializeField]
    private int _hitAnimCount = 3;

    [SerializeField]
    private int _deathAnimCount = 2;

    [Header("Sound Settings")]
    [SerializeField]
    private AudioSource _enemyAudioSource;

    [SerializeField]
    private List<AudioClip> _hitClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> _dieClips = new List<AudioClip>();

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem _bloodImpact;

    [SerializeField]
    private Transform _effectSpawnPoint;

    public void TakeHit(float _damage)
    {
        if (!_canLive)
            return;

        _enemyAnimator.SetFloat("HitID", Random.Range(0, _hitAnimCount));
        _enemyAnimator.CrossFade("Hiting", 0.1f);

        _enemyHealthSystem.TakeDamage(_damage);
        _enemyAudioSource.PlayOneShot(_hitClips[Random.Range(0, _hitClips.Count)]);

        ParticleSystem _impact = Instantiate(_bloodImpact, _effectSpawnPoint.position, Quaternion.identity);
        Destroy(_impact.transform.gameObject, _impact.duration);
    }

    public override void Die()
    {
        _canLive = false;
        _enemyAudioSource.PlayOneShot(_dieClips[Random.Range(0, _dieClips.Count)]);

        _enemyAnimator.SetFloat("DeathID", Random.Range(0, _deathAnimCount));
        _enemyAnimator.CrossFade("Death", 0.1f);

        for (int i = 0; i < _enemyColliders.Length; i++)
        {
            _enemyColliders[i].enabled = false;
        }

        Destroy(gameObject, 5);
    }

    public override void Damage()
    {
        base.Damage();
    }
}