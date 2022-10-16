using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static PlayerController;
using System.Runtime.CompilerServices;

[System.Serializable]
public enum PlayerState
{
    Grounded,
    Jumping,
    Falling,
    TongueOut,
    TongueIn,
    ImportantAnim,
}

public class PlayerController : MonoBehaviour, MovingActors
{



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
    float TongueSpeed = 0f;
    [SerializeField]
    float TongueCheckSize = 5f;
    [SerializeField]
    LayerMask GrabbableLayers;

    private GameObject GrabbedObject;

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
    private readonly int tongueInAnim = Animator.StringToHash("LoopTongue");
    private readonly int tongueOutAnim = Animator.StringToHash("TongueOut");
    private readonly int eatingAnim = Animator.StringToHash("Eating");

    private float FatValue = 0f;

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
        Vector3[] vals = { Vector3.zero, Vector3.zero };
        _lineRenderer.SetPositions(vals);
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Blend", FatValue);
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

                if (InputSource.Gameplay.Tongue.triggered)
                {
                    ChangePlayerState(PlayerState.TongueOut);
                }

                break;
            case PlayerState.TongueOut:
                if (_lineRenderer.positionCount > 0)
                {
                    if (currentTongueLenght < tongueLenght)
                    {
                        currentTongueLenght += Time.deltaTime * TongueSpeed;
                        currentTongueLenght = Mathf.Clamp(currentTongueLenght, 0, tongueLenght);

                        _lineRenderer.SetPosition(0, TongueStart.transform.position);
                        Vector3 pos = transform.TransformPoint(TongueStart.localPosition * Vector2.right * currentTongueLenght);

                        Collider2D hit = Physics2D.OverlapCircle(pos, TongueCheckSize, GrabbableLayers);
                        
                        if (hit)
                        {
                            
                            IEatableInterface eatable = hit.gameObject.GetComponent<IEatableInterface>();
                            if (eatable!=null)
                            {
                                Debug.Log("Eatable found");
                                eatable.OnGrabbed();
                                GrabbedObject = hit.gameObject;
                                ChangePlayerState(PlayerState.TongueIn);
                            }
                        }

                        _lineRenderer.SetPosition(1, new Vector3(pos.x, TongueStart.transform.position.y, TongueStart.transform.position.z));
                    }
                    else
                    {
                        ChangePlayerState(PlayerState.TongueIn);
                    }

                }
                else
                {
                    _lineRenderer.SetPosition(0, TongueStart.transform.position);
                    Vector3 pos = transform.TransformPoint(TongueStart.localPosition * Vector2.right * currentTongueLenght);
                    _lineRenderer.SetPosition(1, new Vector3(pos.x, TongueStart.transform.position.y, TongueStart.transform.position.z));
                }
                break;
            case PlayerState.TongueIn:

                if (currentTongueLenght > 0)
                {
                    currentTongueLenght -= Time.deltaTime * TongueSpeed;
                    Mathf.Clamp(currentTongueLenght, 0, currentTongueLenght);
                    _lineRenderer.SetPosition(0, TongueStart.transform.position);
                    Vector3 pos = transform.TransformPoint(TongueStart.localPosition * Vector2.right * currentTongueLenght);
                    _lineRenderer.SetPosition(1, new Vector3(pos.x, TongueStart.transform.position.y, TongueStart.transform.position.z));
                    if (GrabbedObject)
                    {
                        GrabbedObject.transform.position = pos;
                    }
                }
                else
                {
                    Vector3[] vals = { Vector3.zero, Vector3.zero };
                    _lineRenderer.SetPositions(vals);
                    ChangePlayerState(PlayerState.Grounded);
                    if (GrabbedObject)
                    {
                        Destroy(GrabbedObject);

                        
                        ChangePlayerState(PlayerState.ImportantAnim);
                        _animator.CrossFade(eatingAnim, 0f, 0);

                    }
                }

                

                break;
        }
        CheckFlipped();
        CheckGrounded();
        //movementInput = InputSource.Gameplay.Movement.ReadValue<Vector2>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_lineRenderer != null)
        if (_lineRenderer.positionCount>0)
        {
            Gizmos.DrawSphere(this._lineRenderer.GetPosition(1), TongueCheckSize);
        }
        
    }

    private void FixedUpdate()
    {
        if (GetHasTongueOut())
        {
            _rigidBody.velocity = Vector2.zero;
            return;
        }

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
            default:
                _rigidBody.velocity = Vector2.zero;
                break;
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
                case PlayerState.TongueOut:
                    stateHashName = tongueOutAnim;
                    break;
                case PlayerState.TongueIn:
                    stateHashName = tongueInAnim;
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
                    if (!(PlayerState.TongueIn == myState) && !(PlayerState.TongueOut == myState) && !(PlayerState.ImportantAnim == myState))
                    {
                        ChangePlayerState(PlayerState.Grounded);
                    }
                    
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

    public bool GetIsMoving()
    {
        throw new System.NotImplementedException();
    }
}
