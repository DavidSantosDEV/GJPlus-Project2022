using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowGameOver()
    {

    }

    public void ShowGameWon(int starsWon)
    {

    }
    public void ShowNextLevel(string nextLevel)
    {
        if (nextLevel == "")
        {
            ShowToMainMenu();
        }
    }
    void ShowToMainMenu()
    {

    }
}
