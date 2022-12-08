using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHolder", menuName = "ScriptableObjects/Levels/LevelHolderObject", order = 2)]
public class LevelHolder : ScriptableObject
{
    public LevelData[] levelList;
}
