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

    public void ShowGameOver()
    {

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
        if(GameOverScreen)GameOverScreen.SetActive(false);
    }
    public void HideMenu()
    {
        mainMenuScreen.SetActive(false);
    }

    public void ShowGameplayScreen()
    {

        gameplayScreen.SetActive(true);
        pauseMenuScreen.SetActive(false);
    }

    public void ShowMainMenu()
    {
        UIManager.Instance.HideAllGameplayStuff();
        mainMenuScreen.SetActive(true);
    }
}
