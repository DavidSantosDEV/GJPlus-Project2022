using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaterPoint : MonoBehaviour
{
    // Start is called before the first frame update

    Animator myAnim;
    int anim = Animator.StringToHash("Sinking");
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        myAnim = GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        myAnim.CrossFade(anim, 0, 0);
    }
}
