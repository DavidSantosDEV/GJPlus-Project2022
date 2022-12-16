using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AnimatedText : MonoBehaviour
{
    [Header("Animated Text")]
    public float time = .05f;

    public string baseText = "Loading";
    public string Addition = "...";

    private char[] Chararray;
    int index = 0;

    private string textFull;
    TextMeshProUGUI textComp;
    private void Awake()
    {
        textFull = baseText + Addition;
        Chararray = Addition.ToCharArray();
        textComp= GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine(TextLoop());
    }

    private void OnDisable()
    {
        StopCoroutine(TextLoop());
    }

    private IEnumerator TextLoop()
    {
        textComp.text = baseText;
        while(isActiveAndEnabled)
        {
            if (textComp.text.ToLower() == textFull.ToLower())
            {
                index = 0;
                textComp.text = baseText;
            }
            else
            {
                if(index < Chararray.Length)
                {
                    textComp.text += Chararray[index];
                    index++;
                }
                else
                {
                    index= 0;
                    textComp.text = baseText;
                }
            }
            yield return new WaitForSeconds(time);
        }
    }

}
