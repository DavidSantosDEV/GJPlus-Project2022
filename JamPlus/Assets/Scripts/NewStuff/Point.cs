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
    [SerializeField]
    private bool bIsEndPoint = false;
    [SerializeField]
    private SpriteRenderer RippleSprite;

    bool bIsUsed = false;
    public virtual void OnLeave()
    {
        //Play Anim
        //Destroy
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //{
        //    OnPlayerEntered();
        //}
        //
    }

    public virtual void OnPlayerEntered()
    {
        ToggleSelected(false);
        if (bIsEndPoint)
        {
            GameManager.Instance?.LevelComplete();
            //GANHOUUUU
        }
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
        RippleSprite?.gameObject.SetActive(bNewValue);
    }
}
