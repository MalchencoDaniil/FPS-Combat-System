using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct GroundType
{
    [SerializeField] private string _groundName;
    [SerializeField] public List<AudioClip> _stepSounds;
}

public class FootstepController : MonoBehaviour
{
    private PlayerData _playerData;
    private TerrainDetector _terrainDetector;

    private enum FootstepType
    {
        Terrain,
        Single
    }

    [SerializeField] private FootstepType _footstepType;
    [SerializeField] private List<GroundType> _groundTypes = new List<GroundType>();

    private void Awake()
    {
        _playerData = GetComponent<PlayerData>();

        if (_footstepType == FootstepType.Terrain)
            _terrainDetector = new TerrainDetector();
    }

    private void Update()
    {
        if (_groundTypes != null)
        {
            Vector2 _input = InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();

            float _verticalInput = _input.y;
            float _horizontalInput = _input.x;

            if ((Mathf.Abs(_horizontalInput) > 0.35f || Mathf.Abs(_verticalInput) > 0.35f))
            {
                if (_playerData._footstepAudioSource.isPlaying)
                    return;

                _playerData._footstepAudioSource.PlayOneShot(GetRandomClip());
            }
        }
    }

    private AudioClip GetRandomClip()
    {
        if (_footstepType == FootstepType.Terrain)
        {
            int _textureID = _terrainDetector.GetActiveTerrainTextureIdx(transform.position);
            GroundType _currentGroundType = new GroundType();

            for (int i = 0; i < _groundTypes.Count; i++)
            {
                if (_textureID == i)
                {
                    _currentGroundType = _groundTypes[i];
                    break;
                }
            }

            AudioClip _clip = _currentGroundType._stepSounds[UnityEngine.Random.Range(0, _currentGroundType._stepSounds.Count)];
            return _clip;
        }

        if (_footstepType == FootstepType.Single)
        {
            return _groundTypes[0]._stepSounds[UnityEngine.Random.Range(0, _groundTypes[0]._stepSounds.Count)];
        }

        return null;
    }
}