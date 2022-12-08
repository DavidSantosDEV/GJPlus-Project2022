using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    float speed=5;
    Vector2 curDir = Vector2.right;

    public void SetDir(Vector2 dir) { 
        if (dir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1,1);
        }
        curDir = dir; 
    }
    public void SetSpeed(float s) { speed = s; }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(curDir*speed*Time.deltaTime);
    }


    private void OnBecameInvisible()
    {
       Destroy(gameObject);
    }
}
