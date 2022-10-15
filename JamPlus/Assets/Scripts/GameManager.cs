using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

[System.Serializable]
public class LayerDouble
{
    [SerializeField]
    public LayerMask Firstlayer;
    [SerializeField]
    public LayerMask Secondlayer;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<LevelData> AllLevels = new List<LevelData>();

    private int currentLevelIndex = 0;


    FrogController Player;

    private List<Point> LevelPoints = new List<Point>();

    public static GameManager Instance { private set; get; }




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

        Player = FindObjectOfType<FrogController>();
    }
    private void Start()
    {
        LoadGameplayData();
    }
    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex<=AllLevels.Count-1)
        {
            LoadNewLevel(currentLevelIndex);
        }
    }

    public void LoadNewLevel(int index)
    {
        LevelData levelData = AllLevels[index];
        StartCoroutine(LoadSceneAsync(levelData.LevelName));
    }

    private IEnumerator LoadSceneAsync(string Levelname)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Levelname);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //Show UI

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Inform UI that you want to remove the thing

        yield return null; //Wait a new frame
    }
    void OnSceneLoaded(Scene currentScene, LoadSceneMode loadMode)
    {
        if (IsGameplayLevel(currentScene))
        {
            LoadGameplayData();
        }
    }

    void SaveGameplayData()
    {

    }

    void LoadGameplayData()
    {
        LevelPoints.Clear();
        LevelPoints.AddRange(FindObjectsOfType<Point>());
        if (!Player)
        {
            Player = FindObjectOfType<FrogController>();
        }
        if (Player)
        {
            Point startPoint = GetStartingPoint();
            Player?.SetCurrentPoint(startPoint);
            Player.transform.position = startPoint.transform.position;
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


    bool IsGameplayLevel(Scene next)
    {
        foreach (LevelData levelData in AllLevels)
        {
            if (levelData.LevelName == next.name)
            {
                //Its a gameplay level
                return true;
            }
        }
        return false;
    }

    public void AddScoreForLevel()
    {

    }

    public FrogController GetPlayer() { return Player; }
}
