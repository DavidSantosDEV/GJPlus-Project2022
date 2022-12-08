using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { private set; get; }

    [SerializeField]
    private GameObject _VictoryScreen;

    [SerializeField]
    private GameObject GameOverScreen;

    [SerializeField]
    private GameObject gameplayScreen;

    [SerializeField]
    private GameObject mainMenuScreen;

    [SerializeField]
    private GameObject pauseMenuScreen;

    [SerializeField]
    private GameObject loadingScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _VictoryScreen.SetActive(false);
        //gameplayScreen.SetActive(false);
    }

    public void ShowGameWon(int starsWon, bool bHasNextLevel, int MosquitosEaten, int MosquitosNeeded, int MovesDone, int MovesNeeded)
    {
        gameplayScreen?.SetActive(false);
        Debug.Log("Showing Stars: " + starsWon);
        Debug.Log(bHasNextLevel ? "Has next Level" : "No Next Level");
        _VictoryScreen.SetActive(true);
        VictoryScreen vc = _VictoryScreen.GetComponent<VictoryScreen>();
        if (vc)
        {
            vc.SetStars(starsWon);
            vc.SetHasNextLevel(bHasNextLevel);
            vc.SetTexts(MosquitosEaten, MosquitosNeeded, MovesDone, MovesNeeded);
        }
    }
    public void ShowPauseMenu()
    {
        gameplayScreen.SetActive(false);
        pauseMenuScreen.SetActive(true);
    }

    public void HidePauseMenuGameplay()
    {
        pauseMenuScreen?.SetActive(false);
        gameplayScreen.SetActive(true);
    }

    public void HideAllGameplayStuff()
    {
        gameplayScreen.SetActive(false);
        _VictoryScreen.SetActive(false);
        pauseMenuScreen?.SetActive(false);
        GameOverScreen.SetActive(false);
    }
    public void HideMenu()
    {
        mainMenuScreen.SetActive(false);
    }

    public void ShowGameplayScreen()
    {
        GameOverScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        pauseMenuScreen.SetActive(false);
    }

    public void ShowGameOver()
    {
        HideAllGameplayStuff();
        GameOverScreen.SetActive(true);
    }

    public void ShowMainMenu()
    {
        HideAllGameplayStuff();
        mainMenuScreen.SetActive(true);
    }

    public void ToggleLoadingScreen(bool v)
    {
        loadingScreen.SetActive(v);
    }
}
