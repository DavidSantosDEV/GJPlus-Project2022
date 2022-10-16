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

    public void HideAllGameplayStuff()
    {
        gameplayScreen.SetActive(false);
        _VictoryScreen.SetActive(false);
        if(GameOverScreen)GameOverScreen.SetActive(false);
    }

    public void ShowMainMenu()
    {

    }
}
