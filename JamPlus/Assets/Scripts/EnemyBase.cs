using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEatableInterface
{
    [Header("Enemy Settings")]
    [SerializeField]
    private float EnemySpeed=10;
    [SerializeField]
    private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField]
    private float ArrivalDistance = 1f;
    [SerializeField]
    float oscilationsPerSecond = 4;
    [SerializeField]
    float speedFloating=1f;

    private Rigidbody2D _rigidbody2D;

    private bool bIsGrabbed = false;
    private bool bIsMoving = false;
    private int currentIndexOfPosition=0;

    float spawnTime;
    private void Awake()
    {
        spawnTime = Time.time;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        if (patrolPoints.Count>=2)
        {
            Debug.Log("Enemy delegate");
            transform.position = patrolPoints[0].position;
            FrogController Player = GameManager.Instance.GetPlayer();
            Player?.OnPlayerJumped.AddListener(StartMove);
        }
        _rigidbody2D.velocity = Vector2.zero;
        
    }

    void StartMove()
    {
        if(patrolPoints.Count>=2)
        StartCoroutine(MoveToNext());
    }

    private IEnumerator MoveToNext()
    {
        bIsMoving = true;
        float Value = 0f;
        Vector2 originalPosition = transform.position;
        while (Value < 1)
        {
            Value += Time.deltaTime * EnemySpeed;
            Value = Mathf.Clamp01(Value);
            transform.position = Vector3.Lerp(originalPosition, patrolPoints[currentIndexOfPosition].transform.position, Value);
            yield return null;
        }
        currentIndexOfPosition++;
        if (currentIndexOfPosition > patrolPoints.Count - 1)
        {
            currentIndexOfPosition = 0;
        }
        bIsMoving = false;
    }

    void FixedUpdate()
    {
        if (bIsMoving) return;
        /*
        float lifetime = Time.time - spawnTime;
        float phase = lifetime * oscilationsPerSecond * 2f * Mathf.PI;

        Vector2 velocity = _rigidbody2D.velocity;
        velocity.y = speedFloating * Mathf.Cos(phase);

        _rigidbody2D.velocity = velocity;*/
    }

    void EnableCanMove()
    {
        //bCanMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this);
            FrogController player = GameManager.Instance.GetPlayer();

            //Play anim

            //Add Score
            GameManager.Instance.AddScoreForLevel();
        }
    }
    public void OnGrabbed()
    {
        CancelInvoke(nameof(EnableCanMove));
        //bCanMove=false;
        bIsGrabbed = true;

    }
}
