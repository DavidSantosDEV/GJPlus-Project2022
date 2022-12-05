using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class WheelSlot
{
    [Header("Piece Data")]
    [SerializeField]
    private UnityEngine.Sprite Icon;
    [SerializeField]
    private string Label;
    [SerializeField]
    [Range(0f, 100f)]
    private float chance = 0;
    [SerializeField]
    private Color slotColor = Color.white;

    [Header("Reward")]
    [SerializeField]
    private WheelReward reward;

    public void ActivateReward()
    {
        reward?.OnActivateReward();
    }
    public float GetChance() { return chance; }
    public string GetLabel() { return Label; }
    public Sprite GetIcon() { return Icon; }
    public Color GetColor() { return slotColor; }
}
public class SlotPiece : MonoBehaviour
{
    public TextMeshProUGUI SlotName;
    public UnityEngine.UI.Image SlotImage;
    public TextMeshProUGUI SlotDescription;
    public Image backgroundImage;
}

