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
        GameManager.Instance.PlayFromStart();
    }

    public void NextLevelButton()
    {
        PlaySound();
        GameManager.Instance.NextLevel();
    }

    public void RetryButtonm()
    {
        PlaySound();
        GameManager.Instance.ReloadCurrent();
    }

    public void MainMenuButton()
    {
        PlaySound();
        GameManager.Instance.OpenMainMenu();
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
