using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
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
    [Header("Settings")]
    [SerializeField]
    private float MovementSpeed = 4f;
    [SerializeField]
    private List<LevelData> AllLevels = new List<LevelData>();

    private int currentLevelIndex = 0;


    FrogController Player;

    private List<Point> LevelPoints = new List<Point>();

    [SerializeField]
    private bool DebugEditorMode=false;

    [SerializeField]
    AudioAndVolume SoundVictory;

    private int CurrentLevelFliesEaten;
    private int CurrentLevelMovesDone = 0;

    void ResetLevelData()
    {
        CurrentLevelFliesEaten=0;
        CurrentLevelMovesDone = 0;
}

    public static GameManager Instance { private set; get; }

    public float GetMovementSpeed() { return MovementSpeed; }

    private AudioSource SourceSFX;

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


        SourceSFX = gameObject.AddComponent<AudioSource>();
        SourceSFX.spatialBlend = 0;
    }


    public void PlaySoundEffect(AudioAndVolume clip)
    {
        SourceSFX.PlayOneShot(clip.audio,clip.volume);
    }

    void AddJump()
    {
        CurrentLevelMovesDone++;
    }
    public void AddFly()
    {
        CurrentLevelFliesEaten++;
    }

    private void Start()
    {
        if (DebugEditorMode)
        {
            ResetLevelData();
            string NameOfScene = SceneManager.GetActiveScene().name;
            for (int i = 0; i<AllLevels.Count; i++)
            {
                if (AllLevels[i].LevelName == NameOfScene)
                {
                    currentLevelIndex = i;
                    break;
                }
            }
            LoadGameplayData();
            UIManager.Instance.HideMenu();
            UIManager.Instance.ShowGameplayScreen();
        }
        else
        {
            UIManager.Instance.ShowMainMenu();
        }
    }

    public void OpenMainMenu()
    {
        Debug.Log("Opening Main Menu");

        StartCoroutine(LoadSceneAsync("MainMenu"));
        //SceneManager.LoadScene("MainMenu");
        UIManager.Instance.HideAllGameplayStuff();
        UIManager.Instance.ShowMainMenu();
        
    }

    public bool CanMove()
    {
        var actors = FindObjectsOfType<MonoBehaviour>().OfType<MovingActors>();
        foreach (MovingActors act in actors)
        {
            if (act!=null)
            {
                if (act.GetIsMoving())
                {
                    return false;
                }
            }

        }
        return true;
    }

    public void LoadNewLevel(int index)
    {
        LevelData levelData = AllLevels[index];
        ResetLevelData();
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
            UIManager.Instance.HideMenu();
            ResetLevelData();
            LoadGameplayData();
            UIManager.Instance.HideAllGameplayStuff();
            UIManager.Instance.ShowGameplayScreen();
        }
        else
        {
            UIManager.Instance.HideAllGameplayStuff();
            UIManager.Instance.ShowMainMenu();
        }
    }

    void SaveGameplayData()
    {

    }
    List<Point> IfZeroGameOver = new List<Point>();
    List<Point> savingPoints = new List<Point>();
    void LoadGameplayData()
    {
        LevelPoints.Clear();
        LevelPoints.AddRange(FindObjectsOfType<Point>());
        IfZeroGameOver.Clear();
        savingPoints.Clear();
        foreach (Point p in LevelPoints)
        {
            if (p.GameOverIfVanished)
            {
                savingPoints.AddRange(p.savingPoints);
                IfZeroGameOver.Add(p);
            }
        }
        if (!Player)
        {
            Player = FindObjectOfType<FrogController>();
        }
        if (Player)
        {
            Point startPoint = GetStartingPoint();
            Player?.SetCurrentPoint(startPoint);
            Player.transform.position = startPoint.transform.position;

            Player.OnPlayerJumped.RemoveListener(AddFly);
            Player.OnPlayerJumped.RemoveListener(AddJump);

            Player.OnPlayerJumped.AddListener(AddJump);
            Player.OnPlayerEatFLy.AddListener(AddFly);
        }
    }

    
    public void RemoveFinalPoint(Point po)
    {
        if (IfZeroGameOver.Contains(po))
        {
            IfZeroGameOver.Remove(po);
            
        }
    }

    public void CheckPoints()
    {
        /*
        if (IfZeroGameOver.Count <= 0)
        {
            if (!savingPoints.Contains(Player.GetCurrent()))
            {

                Invoke(nameof(ShowGameOver), 0.5f);
            }
            else
            {
                if (Player.GetCurrent().GetNextPoints().Count<=0)
                {
                    Invoke(nameof(ShowGameOver), 0.5f);
                }
            }
        }
        */
    }

    void ShowGameOver()
    {
        UIManager.Instance.ShowGameOver();
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


    public void PlayFromStart()
    {
        ResetLevelData();
        currentLevelIndex = 0;
        if (AllLevels.Count>0)
        {
            LoadNewLevel(currentLevelIndex);
        }
        
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex <= AllLevels.Count - 1)
        {
            LoadNewLevel(currentLevelIndex);
        }
    }
    public void ReloadCurrent()
    {
        if (AllLevels.Count <= 0) return;
        if (AllLevels[currentLevelIndex])
        {
            LoadNewLevel(currentLevelIndex);
        }
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

    public void LevelComplete()
    {
        if (AllLevels[currentLevelIndex])
        {
            
            PlaySoundEffect(SoundVictory);
            AllLevels[currentLevelIndex].SetIsFinished();
            int stars = AllLevels[currentLevelIndex].CalculateStars(CurrentLevelFliesEaten, CurrentLevelMovesDone);

            UIManager.Instance?.ShowGameWon(stars, currentLevelIndex < (AllLevels.Count-1),CurrentLevelFliesEaten, AllLevels[currentLevelIndex].FliesForStar, CurrentLevelMovesDone, AllLevels[currentLevelIndex].MovesForStar);
        }

        

        Debug.Log("Level completed!");
    }

    public FrogController GetPlayer() { return Player; }
}
