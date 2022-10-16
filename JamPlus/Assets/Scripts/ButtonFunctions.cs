using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void PlayButton()
    {
        GameManager.Instance.PlayFromStart();
    }

    public void NextLevelButton()
    {
        GameManager.Instance.NextLevel();
    }

    public void RetryButtonm()
    {
        GameManager.Instance.ReloadCurrent();
    }

    public void MainMenuButton()
    {
        GameManager.Instance.OpenMainMenu();
    }

    public void PauseGameplayButton()
    {
        UIManager.Instance.ShowPauseMenu();
    }

    public void ResumeButton()
    {
        UIManager.Instance.HidePauseMenuGameplay();
    }
}
