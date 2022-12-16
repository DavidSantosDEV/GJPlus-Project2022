using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelObject : MonoBehaviour
{
    [SerializeField]
    private LevelData level;
    [SerializeField]
    private LevelObject[] NextLevels;

    [SerializeField]
    private bool bIsUnlocked = false;
    
    
    
    private bool isComplete;
    private int starCount;
    private int bugCount;

    private MovingUI blemish;

    public LevelObject[] GetNextLevels() { return NextLevels; }
    public LevelData GetLevelData() { return level; }


    private void Awake()
    {

        blemish = GetComponent<MovingUI>();
    }


    public void SetStatus()
    {

        //Set Previous Values




        UpdateStatus();
    }

    public void SetAsFinished()
    {
        level.SetIsFinished();
        isComplete= true;
        UpdateStatus();
    }

    void UpdateStatus()
    {
        if (isComplete)
        {
            blemish.bShouldMove = false;
        }else if (bIsUnlocked)
        {
            blemish.bShouldMove = true;
        }
    }


    public void Unlock()
    {
        bIsUnlocked = true;

        UpdateStatus();
    }

    public void OnClick() {
        if (!level) return;

        if (bIsUnlocked)
        {
            LevelManager.Instance.SelectLevel(this);
        }
        //GameManager.Instance.LoadNewLevel
        //
        //(level.LevelName
    }
}
