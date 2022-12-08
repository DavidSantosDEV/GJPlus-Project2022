using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private float ProjectileSpeed = 20f;
    private Rigidbody2D _rigidbody2D;


    private Vector2 direction;
    public virtual void StartShoot(Vector2 newDirection)
    {
        
    }

    private void FixedUpdate()
    {
        if(_rigidbody2D)
        _rigidbody2D.velocity = direction * ProjectileSpeed;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
