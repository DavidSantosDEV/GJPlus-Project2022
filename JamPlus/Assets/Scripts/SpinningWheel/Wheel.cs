using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class SlotsAndChance
{
    public SlotsAndChance(SlotPiece p,WheelSlot s, float c) {
        _pieceData = p; 
        _slot = s;  
        chance = c; 
        weight = 0; 
        angle = 0; 
        halfAngle = 0; 
    }
    public SlotsAndChance(SlotPiece p, WheelSlot s, float c, float Oangle, float hAngle)
    {
        _pieceData = p;
        _slot = s;
        chance = c;
        weight = 0;
        angle = Oangle;
        halfAngle = hAngle;
    }

    public WheelSlot _slot;

    public SlotPiece _pieceData;

    public float chance;

    public float weight;

    public float GetWeight() { return weight; }
    public void SetWeight(float val) { weight = val; }

    float angle;
    public float Angle { get { return angle; } }  

    float halfAngle;
    public float HalfAngle { get { return halfAngle; } }
}

public class Wheel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private WheelStyles currentWheelStyle;
    [SerializeField]
    private WheelSlotData WheelslotsData;
    [SerializeField]
    float spinDuration = 2f;

    private List<SlotsAndChance> GeneratedSlots = new List<SlotsAndChance>();

    private List<GameObject> GeneratedObjects = new List<GameObject>();
    private float slotAngle = 0f;
    private float halfSlotAngle = 0f;

    private float accumulatedWeight;
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
            GeneratedSlots.Add(new SlotsAndChance(obj, sorted[i],sorted[i].GetChance(),angle,ownHalf));
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
        CalculateWeightsAndIndices();

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
    private void CalculateWeightsAndIndices()
    {
        for (int i = 0; i < GeneratedSlots.Count; ++i)
        {
            accumulatedWeight += GeneratedSlots[i].chance;

            GeneratedSlots[i].SetWeight(accumulatedWeight);
            //GeneratedSlots[i].weight = accumulatedWeight;
        }
    }

    public void Spin()
    {
        if(bIsSpinning) { return; }
        bIsSpinning= true;

        //Generate random location

        int GetRandomPieceIndex()
        {
            System.Random rand = new System.Random();
            double r = rand.NextDouble() * accumulatedWeight;

            for (int i = 0; i < GeneratedSlots.Count; i++)
                if (GeneratedSlots[i].weight >= r)
                    return i;

            return 0;
        }

        int index = GetRandomPieceIndex();
        SlotsAndChance selectedSlot = GeneratedSlots[index];

        float angle = -(selectedSlot.Angle * index);

        float rightOffset = (angle - selectedSlot.HalfAngle) % 360;
        float leftOffset = (angle + selectedSlot.HalfAngle) % 360;
        
        float randomAngle = UnityEngine.Random.Range(rightOffset, leftOffset);

        Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * spinDuration);

        Debug.Log("Target Rot: " + targetRotation);
        
        var = StartCoroutine(SpinRoutine(targetRotation, selectedSlot));
    }

    Coroutine var;

    private IEnumerator SpinRoutine(Vector3 targetRotation,SlotsAndChance slot)
    {
        float val=0;
        Vector3 start = transform.localEulerAngles;
        while (val<=spinDuration)
        {
            transform.localEulerAngles = Vector3.Lerp(start, targetRotation, val / spinDuration);
            val += Time.deltaTime;
            yield return null;
        }
        slot._slot.ActivateReward();
        bIsSpinning = false;

        StopCoroutine(var);
        //OnComplete
    }
}







