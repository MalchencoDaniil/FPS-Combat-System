using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Datas/SettingsData")]
public class SettingsData : ScriptableObject
{
    public float _cameraFOV = 65;
    public float _mouseSensetivity = 10;
    [Range(1, 5)] public float _interactedDistance = 2;

    private void OnDisable()
    {
        _cameraFOV = 65;
    }
}