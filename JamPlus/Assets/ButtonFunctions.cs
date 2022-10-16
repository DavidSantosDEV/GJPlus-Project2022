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

    }

    public void MainMenuButton()
    {
        GameManager.Instance.OpenMainMenu();
    }

    public void PauseGameplayButton()
    {

    }

    public void ResumeButton()
    {

    }
}
