using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    internal void OnPauseGame()
    {
        Time.timeScale = 0;
    }

    internal void OffPauseGame()
    {
        Time.timeScale = 1;
    }
}