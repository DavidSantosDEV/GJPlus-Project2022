using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this); return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }




}
