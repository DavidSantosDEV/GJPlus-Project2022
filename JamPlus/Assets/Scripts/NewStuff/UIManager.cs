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
    }

    public void ShowGameOver()
    {

    }

    public void ShowGameWon(int starsWon)
    {
        _VictoryScreen.SetActive(true);
        VictoryScreen vc = _VictoryScreen.GetComponent<VictoryScreen>();
        if (vc)
        {

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
}
