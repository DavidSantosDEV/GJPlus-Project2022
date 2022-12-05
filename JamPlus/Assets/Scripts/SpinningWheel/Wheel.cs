using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct SlotsAndChance
{
    public SlotsAndChance(SlotPiece s, float c) { slot = s; chance = c; }

    public SlotPiece slot;
    public float chance;
}

public class Wheel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private WheelStyles currentWheelStyle;
    [SerializeField]
    private WheelSlotData WheelslotsData;


    private List<SlotsAndChance> GeneratedSlots = new List<SlotsAndChance>();
    private List<GameObject> GeneratedObjects = new List<GameObject>();
    private float slotAngle = 0f;
    private float halfSlotAngle = 0f;

    private UnityEngine.UI.Image wheelImg;

    private void Awake()
    {
        wheelImg= GetComponent<UnityEngine.UI.Image>();
    }

    private void Start()
    {
        GenerateWheel();
    }

    private void GenerateWheel()
    {
        if (!WheelslotsData) return;
        if (!currentWheelStyle) return;

        List<WheelSlot> wheelSlots = WheelslotsData.GetSlots();

        //Sort slots by size
        List<WheelSlot> sorted = GetSortedList(wheelSlots,0,wheelSlots.Count-1);

        for (int i = 0; i<GeneratedObjects.Count; ++i)
        {
            Destroy(GeneratedObjects[i]);
        }
        GeneratedObjects.Clear();
        GeneratedSlots.Clear();
        slotAngle = 360/ wheelSlots.Count; //Angle between each piece;

        halfSlotAngle = slotAngle / 2;
        if(wheelImg)
        wheelImg.color = currentWheelStyle.wheelColor;

        //Generate 
        List<float> angles = new List<float>();
        float prevAngle = 0f;
        for (int i = 0; i < sorted.Count; ++i)
        {
            SlotPiece obj = Instantiate(currentWheelStyle.prefabSlot, transform);
            obj.SlotImage.sprite = sorted[i].GetIcon();
            obj.SlotName.text = sorted[i].GetLabel();
            //1f /sorted.Count;

            float fill = (sorted[i].GetChance()/100);//sorted.Count;
            obj.backgroundImage.fillAmount = fill;

            float ownHalf = (360 * fill) / 2;
            float angle = ownHalf*2+prevAngle;
            obj.backgroundImage.transform.localEulerAngles = new Vector3(0, 0, angle);

            prevAngle = angle;// * 2;// + ownHalf;
            angles.Add(angle-ownHalf*2);

            //angle = 360* (sorted[i].GetChance()/100);

            obj.backgroundImage.color = sorted[i].GetColor();
            obj.SlotImage.transform.RotateAround(wheelImg.transform.position, Vector3.back, - angle + ownHalf);// = new Vector3(0, 0, angle);//, angle/*(slotAngle * i)*/);

            GeneratedObjects.Add(obj.gameObject);
            GeneratedSlots.Add(new SlotsAndChance(obj, sorted[i].GetChance()));
        }
        //Lines
        for (int i = 0; i < angles.Count; ++i)
        {
            if (currentWheelStyle.prefabSeperator)
            {
                Transform lineTrns = Instantiate(currentWheelStyle.prefabSeperator, transform.position, Quaternion.identity, transform).transform;
                lineTrns.Rotate(Vector3.back,360- angles[i]);//(slotAngle * i) + halfSlotAngle);
                GeneratedObjects.Add(lineTrns.gameObject);
            }
        }

    }

    private List<WheelSlot> GetSortedList(List<WheelSlot> array, int leftIndex, int rightIndex)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = array[leftIndex].GetChance();
        while (i <= j)
        {
            while (array[i].GetChance() < pivot)
            {
                i++;
            }

            while (array[j].GetChance() > pivot)
            {
                j--;
            }
            if (i <= j)
            {
                WheelSlot temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        if (leftIndex < j)
            GetSortedList(array, leftIndex, j);
        if (i < rightIndex)
            GetSortedList(array, i, rightIndex);
        return array;
    }
    public void SetSlots(WheelSlotData slots, bool createWheel)
    {
        if (!slots) return;

        WheelslotsData = slots;
        if (createWheel)
        {
            GenerateWheel();
        }
    }

    bool bIsSpinning = false;
    private void Update()
    {
        if(bIsSpinning)
        {

        }   
    }

    public void Spin()
    {
        if(bIsSpinning) { return; }
        bIsSpinning= true;

        //Generate random location




        Vector3 targetRotation = Vector3.back * (90 + 2 * 360 * 5f);
        transform
        .DORotate(targetRotation, 5f, RotateMode.Fast)
        .SetEase(Ease.InOutQuart);
    }

    private void OnMouseDown()
    {
        Spin();
    }

}







