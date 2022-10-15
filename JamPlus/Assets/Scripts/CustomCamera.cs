using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{

    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        if (!Target)
        {
            Target = FindObjectOfType<PlayerController>().gameObject;
        }
        
    }

    private void LateUpdate()
    {
        
    }
}
