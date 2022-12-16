using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField]
    AudioAndVolume clip;
    void PlaySound()
    {
        GameManager.Instance.PlaySoundEffect(clip);
    }

    public void PlayButton()
    {
        PlaySound();
        //Show UI for level select
        UIManager.Instance.ShowLevelSelect();
        //GameManager.Instance.PlayFromStart();
    }

    public void NextLevelButton()
    {
        PlaySound();
        LevelManager.Instance.GoToNextLevel();
        //LevelManager.Instance.NextLevel();
    }

    public void RetryButtonm()
    {
        PlaySound();
        LevelManager.Instance.ReloadCurrent();
    }

    public void MainMenuButton()
    {
        PlaySound();
        LevelManager.Instance.OpenMainMenu();
    }

    public void PauseGameplayButton()
    {
        PlaySound();
        UIManager.Instance.ShowPauseMenu();
    }

    public void ResumeButton()
    {
        PlaySound();
        UIManager.Instance.HidePauseMenuGameplay();
    }
}
