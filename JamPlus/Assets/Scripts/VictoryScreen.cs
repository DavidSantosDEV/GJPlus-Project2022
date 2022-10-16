using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject StarHolder;

    [SerializeField]
    TextMeshProUGUI TextMosquito;
    [SerializeField]
    TextMeshProUGUI TextMoves;

    [SerializeField]
    GameObject buttonHasNextLevel;
    [SerializeField]
    GameObject buttonHasNoNextLevel;

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

    public void SetHasNextLevel(bool bHasNext)
    {
        buttonHasNextLevel.SetActive(bHasNext);
        buttonHasNoNextLevel.SetActive(!bHasNext);
    }

    public void SetTexts(int MosquitosEaten, int NeededMosquitos, int MovesDone, int MovesNeeded)
    {
        if (TextMosquito)
        {
            if (MosquitosEaten >= NeededMosquitos)
            {
                TextMosquito.text = "<color=green>" + MosquitosEaten+ "</color>" + "/"+NeededMosquitos;
            }
            else
            {
                TextMosquito.text = "<color=red>" + MosquitosEaten + "</color>" + "/" + NeededMosquitos;
            }
        }
        if (TextMoves)
        {
            if (MovesDone<=MovesNeeded)
            {
                TextMoves.text = "<color=green>" + MovesDone + "</color>" + "/" + MovesNeeded;
            }
            else
            {
                TextMoves.text = "<color=red>" + MovesDone + "</color>" + "/" + MovesNeeded;

            }
            //TextMoves.text = MovesDone+"<"+ MovesNeeded;
        }
    }
}
