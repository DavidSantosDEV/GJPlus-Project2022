using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WheelStyleData", menuName = "ScriptableObjects/FortuneWheel/WheelStyles", order = 1)]
public class WheelStyles : ScriptableObject
{
    [Header("Wheel")]
    public SlotPiece prefabSlot;
    public GameObject prefabSeperator;
    public Sprite wheelSprite;
    public Color wheelColor;
    public AudioClip wheelTickSound;
    public AudioClip wheelFinishSound;
}
