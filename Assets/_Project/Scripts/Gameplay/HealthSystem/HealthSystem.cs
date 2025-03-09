using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageble
{
    private float _maxHealth = 0;

    [Header("Main Settings")]
    [SerializeField] 
    private float _health = 100;

    [SerializeField]
    private Death _deathSystem;

    private void Start()
    {
        _maxHealth = _health;
    }

    public float CurrentHealth()
    {
        return _health;
    }

    public void TakeDamage(float _damage)
    {
        if (_damage < 0)
        {
            _damage = Mathf.Abs(_damage);
        }

        _health -= _damage;
        _deathSystem.Damage();

        if (_health <= 0)
            Death();
    }

    public void Heal(float _healAmount)
    {
        _health += _healAmount;

        if (_health >= _maxHealth) _health = _maxHealth;
    }

    public void AddHealth(float _amount)
    {
        if (_amount < 0) _amount = Mathf.Abs(_amount);

        _health += _amount;

        if (_health >= _maxHealth)
            _health = _maxHealth;
    }

    private void Death()
    {
        _health = 0;
        _deathSystem.Die();
    }
}