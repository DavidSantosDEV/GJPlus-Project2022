using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositioning : MonoBehaviour
{

    public string PlayerStartTag;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene currentScene, LoadSceneMode loadMode)
    {

    }
}
