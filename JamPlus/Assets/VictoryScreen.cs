using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject StarHolder;


    private void OnEnable()
    {
        Debug.Log("I AM ACTIVE!");
    }

    public void SetStars(int count)
    {
        for (int i = 0; i< StarHolder.transform.childCount;i++) {
            StarHolder.transform.GetChild(i).gameObject.SetActive(i < count);
        }
    }
}
