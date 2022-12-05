using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelSlotData", menuName = "ScriptableObjects/FortuneWheel/WheelSlots", order = 1)]
public class WheelSlotData : ScriptableObject
{
    [SerializeField]
    List<WheelSlot> Slots = new List<WheelSlot>();

    public List<WheelSlot> GetSlots() { return Slots; }
}
