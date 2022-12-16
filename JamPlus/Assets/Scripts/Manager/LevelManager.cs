using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Manager")]
    [SerializeField]
    private LevelHolder levelList;
    [SerializeField]
    private string mainMenuName;

    List<LevelObject> fullLevelObjects;

    private LevelObject currentSelectedLevel;

    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        
        //DontDestroyOnLoad(this);
    }

    private int CurrentLevelFliesEaten;
    private int CurrentLevelMovesDone = 0;


    void OnSceneLoaded(Scene currentScene, LoadSceneMode loadMode)
    {
        if (IsGameplayLevel(currentScene))
        {
            UIManager.Instance.HideMenu();
            LevelManager.Instance.ResetLevelData();
            LoadLevelData();
            UIManager.Instance.HideAllGameplayStuff();
            UIManager.Instance.ShowGameplayScreen();
        }
        else
        {
            UIManager.Instance.HideAllGameplayStuff();
            UIManager.Instance.ShowMainMenu();
        }
    }

    bool IsGameplayLevel(Scene next)
    {
        return next.name != mainMenuName;
        /*
        foreach (LevelData levelData in allLevels.levelList)
        {
            if (levelData.LevelName == next.name)
            {
                //Its a gameplay level
                return true;
            }
        }
        return false;*/
    }

    private List<Point> LevelPoints = new List<Point>();
    public void LoadLevelData()
    {
        LevelPoints.Clear();
        LevelPoints.AddRange(FindObjectsOfType<Point>());

        FrogController Player = FindObjectOfType<FrogController>();
        if (Player)
        {
            GameManager.Instance.SetPlayer(Player);
            Point startPoint = GetStartingPoint();
            Player?.SetCurrentPoint(startPoint);
            Player.transform.position = startPoint.transform.position;

            Player.OnPlayerJumped.RemoveListener(LevelManager.Instance.AddFly);
            Player.OnPlayerJumped.RemoveListener(LevelManager.Instance.AddJump);

            Player.OnPlayerJumped.AddListener(LevelManager.Instance.AddJump);
            Player.OnPlayerEatFLy.AddListener(LevelManager.Instance.AddFly);
        }
    }

    public int GetPointCount() { return LevelPoints.Count; }

    Point GetStartingPoint()
    {
        foreach (Point point in LevelPoints)
        {
            if (point.GetIsStartingPoint())
            {
                return point;
            }
        }
        return null;
    }

    private IEnumerator LoadSceneAsync(string Levelname)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Levelname);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //Show UI
        UIManager.Instance.ToggleLoadingScreen(true);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Inform UI that you want to remove the thing



        yield return null; //Wait a new frame
        UIManager.Instance.ToggleLoadingScreen(false);

    }



    public void ResetLevelData()
    {
        CurrentLevelFliesEaten = 0;
        CurrentLevelMovesDone = 0;
    }

    public void AddJump()
    {
        CurrentLevelMovesDone++;
    }
    public void AddFly()
    {
        CurrentLevelFliesEaten++;
    }

    public void GoToNextLevel()
    {
        ResetLevelData();
        if (currentSelectedLevel)
        {
            LevelObject[] levels = currentSelectedLevel.GetNextLevels();
            if (levels.Length==1)
            {
                currentSelectedLevel = levels[0];
                StartCoroutine(LoadSceneAsync(levels[0].GetLevelData().LevelName));
            }
            else
            {
                //Show level UI
                OpenMainMenu();
            }
        }
    }

    public void ReloadCurrent()
    {
        if (currentSelectedLevel)
        {
            StartCoroutine(LoadSceneAsync(currentSelectedLevel.GetLevelData().LevelName));
        }
    }

    public void LevelComplete()
    {
        if (currentSelectedLevel)
        {

            //GameManager.PlaySoundEffect(SoundVictory);

            //Set Level as Finished
            currentSelectedLevel.SetAsFinished();

            LevelObject[] list = currentSelectedLevel.GetNextLevels();
            for (int i = 0; i<list.Length;i++)
            {
                list[i].Unlock();
            }
            LevelData led = currentSelectedLevel.GetLevelData();
            int stars = led.CalculateStars(CurrentLevelFliesEaten, CurrentLevelMovesDone);
            UIManager.Instance?.ShowGameWon(stars, true, CurrentLevelFliesEaten,led.FliesForStar, CurrentLevelMovesDone, led.MovesForStar);
        }

        Debug.Log("Level completed!");
    }


    public void SelectLevel(LevelObject level)
    {
        if (!level) return;
        currentSelectedLevel = level;
        LevelData dat = currentSelectedLevel.GetLevelData();
        StartCoroutine(LoadSceneAsync(dat.LevelName));
    }

    void OnCompleteLevel(LevelData levelComplete, int numStars)
    {

    }

    public void OpenMainMenu()
    {
        Debug.Log("Opening Main Menu");
        currentSelectedLevel = null;
        StartCoroutine(LoadSceneAsync(mainMenuName));
        UIManager.Instance.HideAllGameplayStuff();
        UIManager.Instance.ShowMainMenu();
    }
}
