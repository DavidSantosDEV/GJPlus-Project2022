using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static PlayerController;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Grounded,
        Jumping,
        Falling,
    }


    [Header("Movement")]
    [SerializeField]
    private float SpeedMovement = 14f;
    [SerializeField]
    private float SpeedAir = 8f;
    [SerializeField]
    private float GravityFalling=4.5f;
    [SerializeField]
    private float GravityDefault = 3f;
    
    [Header("Jump")]
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    float fStrenghtOffsetInput = 0.1f;
    [SerializeField]
    private float jumpStartTime;
    private float jumpTime;
    [SerializeField]
    private float jumpInfluenceTime=3f;
    private float currentJumpInfluenceTime;
    [Header("Gameplay")]
    [SerializeField]
    private int MaxProjectiles = 3;
    [SerializeField]
    private Transform TongueStart;
    [SerializeField]
    private float tongueLenght;
    [SerializeField]
    float TongueSpeed = 0;

    float currentTongueLenght = 0;

    [Header("Grounding")]
    [SerializeField]
    private Transform[] feetPositions;
    [SerializeField]
    private float castSizeGrounded = 0.1f;
    [SerializeField]
    private LayerMask LayerGround;

    private PlayerActions InputSource;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private LineRenderer _lineRenderer;

    private List<GameObject> currentProjectiles; 

    private PlayerState myState = PlayerState.Grounded;
    private Vector2 movementInput;
    private bool bHasJumpSetDirection;
    private Vector2 lastNonZeroDirection;
    private Vector2 SetJumpDirection;

    private readonly int RunAnim = Animator.StringToHash("Running");
    private readonly int JumpAnim = Animator.StringToHash("Jumping");
    private readonly int idleAnim = Animator.StringToHash("Idle");
    private readonly int fallingAnim = Animator.StringToHash("Falling");

    private void Awake()
    {

        InputSource = new PlayerActions();
        InputSource?.Enable();


        _animator = GetComponent<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case PlayerState.Falling:
                currentJumpInfluenceTime -= Time.deltaTime;
                if (InputSource.Gameplay.Jump.triggered)
                {
                    //Shoot
                }
                break;
            case PlayerState.Jumping:
                currentJumpInfluenceTime -= Time.deltaTime;
                if (InputSource.Gameplay.Jump.IsPressed())
                {
                    if (jumpTime > 0f)
                    {
                        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);
                        jumpTime -= Time.deltaTime;
                    }
                    else
                    {
                        ChangePlayerState(PlayerState.Falling);
                    }
                }
                else
                {
                    ChangePlayerState(PlayerState.Falling);
                }
                break;
            case PlayerState.Grounded:

                if (!GetHasTongueOut())
                {
                    if (InputSource.Gameplay.Jump.triggered)
                    {
                        ChangePlayerState(PlayerState.Jumping);
                        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Vector2.up.y * jumpForce);
                        currentJumpInfluenceTime = jumpInfluenceTime;
                        Debug.Log(_rigidBody.velocity);
                        jumpTime = jumpStartTime;
                    }
                }


                if (InputSource.Gameplay.Tongue.IsPressed())
                {

                    if (_lineRenderer.positionCount>0)
                    {
                        if (currentTongueLenght<tongueLenght)
                        {
                            currentTongueLenght += Time.deltaTime* TongueSpeed;
                            currentTongueLenght = Mathf.Clamp(currentTongueLenght, 0, tongueLenght);
                        }
                        _lineRenderer.SetPosition(0, TongueStart.transform.position);
                        Vector3 pos = transform.TransformPoint(TongueStart.localPosition * Vector2.right * currentTongueLenght);

                        _lineRenderer.SetPosition(1, new Vector3(pos.x, TongueStart.transform.position.y, TongueStart.transform.position.z));
                    }
                    else
                    {
                        _lineRenderer.SetPosition(0, TongueStart.transform.position);
                        Vector3 pos = transform.TransformPoint(TongueStart.localPosition * Vector2.right * currentTongueLenght);
                        _lineRenderer.SetPosition(1, new Vector3(pos.x, TongueStart.transform.position.y, TongueStart.transform.position.z));
                    }
                    

                }
                else
                {
                    if (currentTongueLenght > 0)
                    {
                        currentTongueLenght -= Time.deltaTime * TongueSpeed;
                        Mathf.Clamp(currentTongueLenght, 0, currentTongueLenght);
                        _lineRenderer.SetPosition(0, TongueStart.transform.position);
                        Vector3 pos = transform.TransformPoint(TongueStart.localPosition * Vector2.right * currentTongueLenght);
                        _lineRenderer.SetPosition(1, new Vector3(pos.x, TongueStart.transform.position.y, TongueStart.transform.position.z));
                    }
                    else
                    {
                        Vector3[] vals = { Vector3.zero, Vector3.zero };
                        _lineRenderer.SetPositions(vals);
                    }
                    
                }
                break;
        }
        CheckFlipped();
        CheckGrounded();
        //movementInput = InputSource.Gameplay.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (GetHasTongueOut()) return;


        Vector2 vector = InputSource.Gameplay.Movement.ReadValue<Vector2>(); //Input Vector
        if (vector != Vector2.zero && vector != Vector2.down && vector != Vector2.up)
        {
            if (myState.Equals(PlayerState.Falling) || myState.Equals(PlayerState.Jumping))
            {
                bHasJumpSetDirection = true;
                lastNonZeroDirection = vector;
            }
        }

        

        switch (myState)
        {
            case PlayerState.Jumping:
            case PlayerState.Falling:
                
                if (bHasJumpSetDirection)
                {
                    vector = lastNonZeroDirection;
                }
                vector *= SpeedAir;
                _rigidBody.velocity = new Vector2(vector.x, _rigidBody.velocity.y);
                break;
            case PlayerState.Grounded:
                {
                    vector *= SpeedMovement;
                    _rigidBody.velocity = new Vector2(vector.x, _rigidBody.velocity.y);
                    int stateHashName = ((Mathf.Abs(_rigidBody.velocity.x) > 0f) ? RunAnim : idleAnim);
                    _animator?.CrossFade(stateHashName, 0f, 0);
                    break;
                }
        }
     
    }

    bool GetHasTongueOut() { return currentTongueLenght > 0; }

    public void ChangePlayerState(PlayerState newState)
    {
        if (newState != myState)
        {
            myState = newState;
            Debug.Log(newState.ToString());
            int stateHashName = idleAnim;
            switch (newState)
            {
                case PlayerState.Jumping:
                    stateHashName = JumpAnim;
                    break;
                case PlayerState.Falling:
                    _rigidBody.gravityScale = GravityFalling;
                    stateHashName = fallingAnim;
                    break;
                case PlayerState.Grounded:
                    _rigidBody.gravityScale = GravityDefault;
                    bHasJumpSetDirection = false;
                    break;
                default:
                    stateHashName = idleAnim;
                    break;
            }
            _animator.CrossFade(stateHashName, 0f, 0);
        }
    }


    void ShootProjectile(Vector2 Direction)
    {
        if (currentProjectiles.Count<=0) 
        {
            return;
        }
        //Shoot to x direction


    }

    void CheckGrounded()
    {
        bool flag = false;
        foreach (Transform foot in feetPositions)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(foot.transform.position, Vector2.down, castSizeGrounded, LayerGround);
            Debug.DrawRay(foot.transform.position, Vector2.down * castSizeGrounded, Color.blue);
            if ((bool)raycastHit2D)
            {
                if (_rigidBody.velocity.y <= 0f || myState == PlayerState.Falling)
                {
                    flag = true;
                    _rigidBody.gravityScale = GravityDefault;
                    ChangePlayerState(PlayerState.Grounded);
                    break;
                }
            }
            else
            {
                flag = false;
            }
        }
        if (!flag && !myState.Equals(PlayerState.Jumping))
        {
            ChangePlayerState(PlayerState.Falling);
        }

    }

    void CheckFlipped()
    {
        Vector2 InputVal = InputSource.Gameplay.Movement.ReadValue<Vector2>();
        if (Mathf.Abs(_rigidBody.velocity.x) > 0f && InputVal!=Vector2.zero)
        {
            gameObject.transform.localScale = InputVal.x < 0f ? Vector3.one : new Vector3(-1, 1, 1);
        }
    }

    bool AddProjectile(GameObject newProjectile)
    {
        if (currentProjectiles.Count>= MaxProjectiles)
        {
            return false;
        }

        currentProjectiles.Add(newProjectile);
        newProjectile.SetActive(false);

        return true;
    }
}