using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public int level;
    public string LevelName;
    
    public bool bIsFinished;
    
    public int Score;
    
    public int MaxMoves;
}
