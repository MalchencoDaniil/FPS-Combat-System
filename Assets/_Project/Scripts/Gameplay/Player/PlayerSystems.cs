using UnityEngine.UI;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerSystems : Death
{
    private float _maxStamina;

    private HealthSystem _healthSystem;
    private CameraAIM _camera;

    [Header("Stamina")]
    [SerializeField]
    private float _currentStamina = 100;

    [SerializeField]
    private Slider _staminaSlider;

    [Header("Damage Effect")]
    [SerializeField]
    private Volume _hdrpGlobalVolume;

    [Space(15)]
    [SerializeField]
    private float _chromaticAberrationMaxHealth = 0.2f;

    [SerializeField]
    private float _chromaticAberrationMinHealth = 1f;

    [Space(15)]
    [SerializeField]
    private float _saturationMaxHealth = 0f;

    [SerializeField]
    private float _saturationMinHealth = -100f;

    private ChromaticAberration _chromaticAberration;
    private ColorAdjustments _colorAdjustments;

    [Header("Death")]
    [SerializeField]
    private Transform[] _disabledObjects;

    [SerializeField]
    private Transform _cutsceneObject;

    [SerializeField]
    private Transform _cutscenePlayer, _cutsceneCamera;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();

        _camera = FindObjectOfType<CameraAIM>();
        _maxStamina = _currentStamina;

        _hdrpGlobalVolume.profile.TryGet(out _chromaticAberration);
        _hdrpGlobalVolume.profile.TryGet(out _colorAdjustments);
    }

    private void Update()
    {
        _staminaSlider.maxValue = _maxStamina;
        _staminaSlider.value = _currentStamina;

        if (Input.GetKeyDown(KeyCode.Space))
            _healthSystem.TakeDamage(15);

        if (_healthSystem.CurrentHealth() < 100)
        {
            _healthSystem.Heal(0.1f);
            UpdateVisualEffects();
        }
    }

    public void TakeStamina(float _damage)
    {
        if (_damage <= 0)
            _damage = _damage * (-1);

        _currentStamina -= _damage;

        if (_currentStamina <= 0)
            _currentStamina = 0;
    }

    public void StaminaHeal(float _healAmount)
    {
        _currentStamina += _healAmount;

        if (_currentStamina >= _maxStamina) _currentStamina = _maxStamina;
    }

    public bool StaminaNotNull()
    {
        return _currentStamina >= 1;
    }

    public override void Damage()
    {
        UpdateVisualEffects();
    }

    public override void Die()
    {
        _cutsceneObject.gameObject.SetActive(true);

        _cutscenePlayer.position = transform.position;
        _cutsceneCamera = _camera.transform;

        for (int i = 0; i < _disabledObjects.Length; i++)
            _disabledObjects[i].gameObject.SetActive(false);
    }

    private void UpdateVisualEffects()
    {
        float healthPercentage = _healthSystem.CurrentHealth() / 100;

        if (_chromaticAberration != null)
        {
            float chromaticAberrationIntensity = Mathf.Lerp(_chromaticAberrationMinHealth, _chromaticAberrationMaxHealth, healthPercentage);
            _chromaticAberration.intensity.Override(chromaticAberrationIntensity);
        }

        if (_colorAdjustments != null)
        {
            float saturationValue = Mathf.Lerp(_saturationMinHealth, _saturationMaxHealth, healthPercentage);
            _colorAdjustments.saturation.Override(saturationValue);
        }
    }
}