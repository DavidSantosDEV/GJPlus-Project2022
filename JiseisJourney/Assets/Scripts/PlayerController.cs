using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlayerStates
{
    Grounded,
    Jumping,
    Falling,
    Floating,
}

public class PlayerController : MonoBehaviour
{
    private PlayerActions InputSource;

    [Header("Movement")]
    [SerializeField]
    private float MovementSpeedGround;
    [SerializeField]
    private float MovementSpeedAir;
    [SerializeField]
    private float JumpStrenght;
    [SerializeField] 
    private float JumpTime;
    [SerializeField] 
    private float CoyoteeTime;
    [SerializeField]
    private float GravityScaleGrounded;
    [SerializeField] 
    private float GravityScaleFalling;
    [SerializeField] 
    private float GravityScaleFloating;

    private float MovementInput;
    private float LastNonZeroInput;

    private float currentJumpTime=0f;

    [Header("Grounding")]
    [SerializeField]
    private Transform[] feetPos;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private PlayerStates playerState;

    private void Awake()
    {
        InputSource = new PlayerActions();
        InputSource.Enable();


        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerStates.Grounded;
    }

    // Update is called once per frame
    void Update()
    {

        MovementInput = InputSource.Gameplay.Movement.ReadValue<float>();

        switch (playerState)
        {
            case PlayerStates.Grounded:

                if (InputSource.Gameplay.Jump.IsPressed())
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpStrenght);
                    ChangeState(PlayerStates.Jumping);
                }
                
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerStates.Grounded:

                _rigidbody2D.velocity = new Vector2(MovementSpeedGround * MovementInput, _rigidbody2D.velocity.y);
                
                break;
            case PlayerStates.Jumping:

                if (InputSource.Gameplay.Jump.IsPressed())
                {
                    if(currentJumpTime>=0)
                    {
                        _rigidbody2D.velocity = new Vector2( _rigidbody2D.velocity.x, JumpStrenght);
                        currentJumpTime -= Time.deltaTime;
                    }
                }
                else
                {
                    ChangeState(PlayerStates.Falling);
                }
                
                break;
        }
    }

    void ChangeState(PlayerStates newState)
    {
        if (playerState == newState)
        {
            return;
        }

        PlayerStates previousState = playerState;
        playerState = newState;

        switch (playerState)
        {
            case PlayerStates.Jumping:
                currentJumpTime = JumpTime;
                break;
            default:
                break;
        }


    }

    void CheckFlipCharacter()
    {

    }

}
