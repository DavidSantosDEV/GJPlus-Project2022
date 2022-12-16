using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI : MonoBehaviour
{
    public float timeOfWait;
    public float moveAmmount;
    RectTransform rectTransform;
    Vector2 posRect;

    public bool bShouldMove = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(bShouldMove)
        StartCoroutine(MoveUI());

    }
    private void OnDisable()
    {
        if(bShouldMove)
        StopCoroutine(MoveUI());
    }

    private void Awake()
    {     
        rectTransform = GetComponent<RectTransform>();
        posRect = rectTransform.localPosition;
    }

    private IEnumerator MoveUI()
    {
        
        //Vector2 pos = transform.position;

        while (isActiveAndEnabled && bShouldMove)
        {
            if (rectTransform) {
                rectTransform.localPosition = new Vector2(posRect.x, posRect.y + moveAmmount);
                yield return new WaitForSeconds(timeOfWait);
                rectTransform.localPosition = new Vector2(posRect.x, posRect.y - moveAmmount);
                //transform.position = new Vector2(pos.x, pos.y - moveAmmount);
            }
            yield return new WaitForSeconds(timeOfWait);

        }
    }
}
