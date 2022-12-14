using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelRating
{

}

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Levels/LevelDataObject", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private string levelName;
    
    private bool IsFinished=false;

    private int Stars =0;

    public int MovesForStar = 1;
    
    public int FliesForStar = 1;


    public string LevelName { get => levelName; }


    public void SetIsFinished() {
        IsFinished = true;
    }

    public int CalculateStars(int FliesEaten, int MovesDone)
    {
        int StarNum=0;
        if (IsFinished)
        {
            StarNum++;         
        }
        if (FliesEaten>=FliesForStar)
        {
            StarNum++;
        }
        if (MovesDone <= MovesForStar)
        {
            StarNum++;
        }
        Stars = StarNum;
        Debug.Log("This Level he gained " + StarNum + " Stars!");
        return StarNum;
    }
}
