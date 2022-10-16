using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointAndBlockingWho
{
    public Transform location;
    public Point BlockMe;
    public List<Point> Paths= new List<Point>();

    public void BlockPaths()
    {
        BlockMe.SetBlocked(Paths);
    }

    public void UnBlockPaths()
    {
        BlockMe.SetBlocked(new List<Point>());
    }
}

public class LogMoves : MonoBehaviour, MovingActors
{
    [SerializeField]
    private List<PointAndBlockingWho> LogPoints = new List<PointAndBlockingWho>();

    int currentIndex = 0;

    bool bIsMoving=false;
    private void Start()
    {
        if (LogPoints.Count>=2)
        {
            transform.position = LogPoints[0].location.position;
            LogPoints[0].BlockPaths();
            FrogController Player = GameManager.Instance.GetPlayer();
            
            if(!Player)Player = FindObjectOfType<FrogController>();

            Player?.OnPlayerJumped.RemoveListener(StartMove);
            Player?.OnPlayerJumped.AddListener(StartMove);
        }

    }

    void StartMove()
    {
        if(LogPoints.Count >= 2)
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        bIsMoving = true;

        Vector2 originalposition = transform.position;
        float t=0;

        LogPoints[currentIndex].UnBlockPaths();
        

        currentIndex++;

        
        if (currentIndex > LogPoints.Count - 1)
        {
            currentIndex = 0;
        }

        LogPoints[currentIndex].BlockPaths();

        while (t<1)
        {
            t += Time.deltaTime*GameManager.Instance.GetMovementSpeed();
            t = Mathf.Clamp01(t);
            transform.position = Vector2.Lerp(originalposition, LogPoints[currentIndex].location.position,t);
            yield return null;
        }

        

        bIsMoving = false;
    }

    public bool GetIsMoving()
    {
        return bIsMoving;
        throw new System.NotImplementedException();
    }
}
