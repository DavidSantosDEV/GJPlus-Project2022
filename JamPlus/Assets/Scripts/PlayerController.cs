using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

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
    private float GravityFalling=4.5f;
    [SerializeField]
    private float GravityDefault = 3f;


    [Header("Gameplay")]
    private int MaxProjectiles = 3;



    private PlayerActions InputSource;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private List<GameObject> currentProjectiles; 

    private PlayerState myState = PlayerState.Grounded;
    private Vector2 movementInput;

    private void Awake()
    {

        InputSource = new PlayerActions();
        InputSource?.Enable();


        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movementInput = InputSource.Gameplay.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case PlayerState.Grounded:
                _rigidBody.velocity = new Vector2(SpeedMovement * movementInput.x, _rigidBody.velocity.y);
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Falling:
                break;
        }
        
    }


    public void ChangePlayerState(PlayerState newState)
    {
        if (myState == newState)
        {
            return;
        }
        switch (newState)
        {
            default:

                break;
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
