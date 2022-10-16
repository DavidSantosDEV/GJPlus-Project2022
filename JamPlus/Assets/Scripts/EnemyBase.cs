using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEatableInterface, MovingActors
{
    [Header("Enemy Settings")]
    [SerializeField]
    private List<Transform> patrolPoints = new List<Transform>();

    private Rigidbody2D _rigidbody2D;

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
            //Player?.OnPlayerEatFLy.RemoveListener(StartMove);
            Player?.OnPlayerJumped.RemoveListener(StartMove);
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
        currentIndexOfPosition++;
        if (currentIndexOfPosition > patrolPoints.Count - 1)
        {
            currentIndexOfPosition = 0;
        }
        while (Value < 1)
        {
            Value += Time.deltaTime * GameManager.Instance.GetMovementSpeed();
            Value = Mathf.Clamp01(Value);
            transform.position = Vector3.Lerp(originalPosition, patrolPoints[currentIndexOfPosition].transform.position, Value);
            yield return null;
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
        Debug.Log(collision.name);
        if (collision.gameObject.tag == "Player")
        {
            
            Destroy(gameObject);
            FrogController player = GameManager.Instance.GetPlayer();
            player.ChangeAnimation(player.eatingAnim);

            GameManager.Instance.AddFly();
            //Play anim
        }
    }
    public void OnGrabbed()
    {
        CancelInvoke(nameof(EnableCanMove));
        //bCanMove=false;
        //bIsGrabbed = true;

    }

    public bool GetIsMoving()
    {
        return bIsMoving;
        throw new System.NotImplementedException();
    }
}
