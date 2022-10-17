using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public float speed=2;

    bool bCanDestroy = false;

    private void Update()
    {
        transform.position += (Vector3)(Vector2.right*speed*Time.deltaTime);
    }

    private void OnBecameVisible()
    {
        bCanDestroy = true;
    }
    private void OnBecameInvisible()
    {
        if (bCanDestroy)
        {
            Destroy(gameObject);
        }
    }
}
