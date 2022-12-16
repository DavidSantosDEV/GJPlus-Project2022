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

    FrogController Player;

    [SerializeField]
    private bool DebugEditorMode=false;

    [SerializeField]
    AudioAndVolume SoundVictory;

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



    private void Start()
    {
        if (DebugEditorMode)
        {
            LevelManager.Instance.ResetLevelData();
            //string NameOfScene = SceneManager.GetActiveScene().name;
            /*for (int i = 0; i<allLevels.levelList.Length; i++)
            {
                if (allLevels.levelList[i].LevelName == NameOfScene)
                {
                    currentLevelIndex = i;
                    break;
                }
            }*/
            LevelManager.Instance.LoadLevelData();
            UIManager.Instance.HideMenu();
            UIManager.Instance.ShowGameplayScreen();
        }
        else
        {
            UIManager.Instance.ShowMainMenu();
        }
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

    /*
    public void LoadNewLevel(int index)
    {
        LevelData levelData = allLevels.levelList[index];
        LevelManager.Instance.ResetLevelData();
        StartCoroutine(LoadSceneAsync(levelData.LevelName));
    }

    public void OpenLevel(string level)
    {
        for (int i = 0; i< allLevels.levelList.Length; ++i)
        {
            if (level == allLevels.levelList[i].LevelName)
            {
                currentLevelIndex= i;
                break;
            }
        }



    }
    */


    void SaveGameplayData()
    {

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

    public void AddScoreForLevel()
    {

    }

    public FrogController GetPlayer() { return Player; }
    public void SetPlayer(FrogController P) { Player = P; }
}
