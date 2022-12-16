using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureHoldingData", menuName = "ScriptableObjects/TextureHolder", order = 1)]
public class TextureHolder : ScriptableObject
{
    [SerializeField]
    private Sprite[] textureList;


    public Sprite GetRandomTexture()
    {
        int rnd = Random.Range(0, textureList.Length);
        return textureList[rnd];
    }

}
