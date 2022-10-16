using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterX : MonoBehaviour
{

    public float time=1f;
    private void OnEnable()
    {
        Invoke(nameof(KillMySelf), time);
    }
    void KillMySelf()
    {
        Destroy(gameObject);
    }
}
