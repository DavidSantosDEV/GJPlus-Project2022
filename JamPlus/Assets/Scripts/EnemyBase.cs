using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    private float EnemySpeed=10;
    [SerializeField]
    private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField]
    private bool IsRandomPatrol=false;
    [SerializeField]
    private float WaitBetweenPoints=0f;
    [SerializeField]
    private float WaitOffset=1f;
    [SerializeField]
    private float ArrivalDistance = 1f;
    
    [SerializeField]
    float amp = 2, omega = 1;

    private Rigidbody2D _rigidbody2D;

    private bool bCanMove = true;
    private int currentIndexOfPosition=0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (bCanMove)
        {
            if (Vector2.Distance(transform.position, patrolPoints[currentIndexOfPosition].position) > ArrivalDistance)
            {
                Vector2 direction =  patrolPoints[currentIndexOfPosition].position- transform.position;
                _rigidbody2D.velocity = direction.normalized * EnemySpeed;
            }
            else
            {
                if (WaitBetweenPoints>0)
                {
                    bCanMove = false;
                    _rigidbody2D.velocity = Vector2.zero;
                    Invoke(nameof(EnableCanMove),WaitBetweenPoints);
                }
                currentIndexOfPosition++;
                if (currentIndexOfPosition>patrolPoints.Count-1)
                {
                    currentIndexOfPosition = 0;
                }
            }
        }
        else
        {
            
            float val = (amp * Mathf.Sin(Time.deltaTime * omega));
            Debug.Log(val);
            _rigidbody2D.velocity = new Vector2(0, val);
        }
        
    }

    void EnableCanMove()
    {
        bCanMove = true;
    }

    void StartPatrol()
    {

    }

    public void SetPatrolPoints(Transform[] points, bool bRandomPatrol)
    {

    }

}
