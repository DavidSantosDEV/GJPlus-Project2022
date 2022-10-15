using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [Header("Point Settings")]
    [SerializeField]
    bool bIsFirstSelectables;
    [SerializeField]
    List<Point> nextPoints = new List<Point>();
    
    bool bIsUsed = false;
    public virtual void OnLeave()
    {
        //Play Anim
        //Destroy
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerEntered();
        }
        //
    }

    public virtual void OnPlayerEntered()
    {

    }

    public void SetUsed()
    {
        bIsUsed = true;
    }

    public bool GetIsStartingPoint() { return bIsFirstSelectables; }

    public bool GetIsUsed() { return bIsUsed; }
    
    public List<Point> GetNextPoints() { return nextPoints; }


    private void OnMouseDown()
    {
        FrogController player = FindObjectOfType<FrogController>();

        player?.ChoseThis(this);
    }

    public void ToggleSelected(bool bNewValue)
    {

    }
}
