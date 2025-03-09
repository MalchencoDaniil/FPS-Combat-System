using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class DamageEffect : MonoBehaviour
{
    public Volume postProcessVolume; // Ссылка на Volume
    public float damageEffectDuration = 0.5f; // Длительность эффекта
    public AnimationCurve intensityCurve; // Кривая интенсивности эффекта

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    private float currentIntensity = 0f;

    void Start()
    {
        if (postProcessVolume == null)
        {
            Debug.LogError("Volume is not assigned!");
            return;
        }

        // Получаем ссылки на эффекты. Важно проверять null, так как эффекты могут быть удалены.
        if (!postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color Adjustments effect not found in Volume Profile!");
            return;
        }
        if (!postProcessVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette effect not found in Volume Profile!");
            return;
        }


        // Инициализируем значения по умолчанию. Важно устанавливать overrideState = true.
        SetEffectIntensity(0f);
    }

    public void DamageTaken()
    {
        StartCoroutine(DamageEffectCoroutine());
    }

    IEnumerator DamageEffectCoroutine()
    {
        float time = 0f;
        while (time < damageEffectDuration)
        {
            time += Time.deltaTime;
            currentIntensity = intensityCurve.Evaluate(time / damageEffectDuration);
            SetEffectIntensity(currentIntensity);
            yield return null;
        }
        SetEffectIntensity(0f); // Убеждаемся, что эффект полностью отключен после окончания
    }


    void SetEffectIntensity(float intensity)
    {
        if (colorAdjustments != null)
        {
            // Important:  Set overrideState to true.  Otherwise, changes will not be applied.
            colorAdjustments.colorFilter.overrideState = true;
            colorAdjustments.colorFilter.value = Color.Lerp(Color.white, Color.red, intensity);

            colorAdjustments.saturation.overrideState = true;
            colorAdjustments.saturation.value = Mathf.Lerp(0f, -80f, intensity);  // More extreme saturation change
        }

        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = Mathf.Lerp(0f, 0.5f, intensity);

            vignette.color.overrideState = true;
            vignette.color.value = Color.Lerp(Color.black, new Color(0.2f, 0f, 0f), intensity); // Dark red
        }
    }
}