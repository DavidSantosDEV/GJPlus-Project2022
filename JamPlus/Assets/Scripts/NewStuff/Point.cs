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
    private float waitTime = 3f;
    [SerializeField]
    private SpriteRenderer RippleSprite;
    [SerializeField]
    private WaterPoint sinking;
    bool bIsUsed = false;
    [SerializeField]
    bool OnLeaveDeactivate = false;

    [SerializeField]
    private AudioAndVolume audioDestroy;
    [SerializeField]
    private AudioAndVolume audioEnter;

    public bool GameOverIfVanished=false;
    public List<Point> savingPoints = new List<Point>();

    List<Point> BlockedPaths= new List<Point>();

    private Collider2D PointCollider;

    private void Awake()
    {
        PointCollider = GetComponent<Collider2D>();
    }


    public void SetBlocked(List<Point> paths)
    {
        BlockedPaths = paths;
    }

    public bool GetBlocked(Point here) {
        return BlockedPaths.Contains(here); 
    }

    public bool GetIsFinal() { return bIsEndPoint; }

    public virtual void OnLeave()
    {
        if (OnLeaveDeactivate)
        {
            if(PointCollider)PointCollider.enabled = false;
            sinking?.gameObject.SetActive(true);
            sinking?.PlayAnim();
            //LevelManager.Instance.
            //GameManager.Instance.RemoveFinalPoint(this);
            GameManager.Instance.PlaySoundEffect(audioDestroy);
        }
        //Play Anim
        //Destroy
    }

    [ExecuteInEditMode]
    public void GenerateRandomSprite()
    {

    }

    public virtual void OnPlayerEntered()
    {
        ToggleSelected(false);
        if (bIsEndPoint)
        {
            GameManager.Instance.PlaySoundEffect(audioEnter);
            Invoke(nameof(FinishGame), waitTime);
            
            //GANHOUUUU
        }
    }

    void FinishGame()
    {
        LevelManager.Instance?.LevelComplete();
        //GameManager.Instance?.LevelComplete();
    }

    public void SetUsed()
    {
        bIsUsed = true;
    }

    public bool GetIsStartingPoint() { return bIsFirstSelectables; }

    public bool GetIsUsed() { return bIsUsed; }
    
    public List<Point> GetNextPoints() {
        List<Point> points = new List<Point>();
        foreach (Point p in nextPoints)
        {
            if (p.gameObject.activeSelf && !GetBlocked(p))
            {
                points.Add(p);
            }
        }
        return points; 
    }
    public List<Point> GetNextPointNoModifiers() { return nextPoints; }


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
