using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransistion : MonoBehaviour
{
    public static SceneTransistion _instance;

    [SerializeField] private Animator _blackScreenAnimator;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (InputManager._instance._inputActions.Game.Restart.triggered)
        {
            Restart();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SceneLoad(int _sceneID)
    {
        SceneManager.LoadScene(_sceneID);
        PauseManager._instance.OffPauseGame();
        CursorManager._instance._cursorState = CursorManager.CursorState.UnLocked;
    }

    public void SceneLoad_ScreenFade(int _level)
    {
        _blackScreenAnimator.SetTrigger("Open");
        StartCoroutine(AsyncLoadScene(_level));
    }

    private IEnumerator AsyncLoadScene(int _level)
    {
        yield return new WaitForSeconds(2);

        SceneLoad(_level);
    }

    public void QuitGame_ScreenFade()
    {
        _blackScreenAnimator.SetTrigger("Open");
        StartCoroutine(QuitWaitTime());
    }

    private IEnumerator QuitWaitTime()
    {
        yield return new WaitForSeconds(2);

        Application.Quit();
    }
}