using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPoint : MonoBehaviour
{
    // Start is called before the first frame update

    Animator myAnim;
    int anim = Animator.StringToHash("Sinking");
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    public void PlayAnim()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
