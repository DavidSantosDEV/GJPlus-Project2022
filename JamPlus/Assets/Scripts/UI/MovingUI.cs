using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI : MonoBehaviour
{
    public float timeOfWait;
    public float moveAmmount;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(MoveUI());
    }
    private void OnDisable()
    {
        StopCoroutine(MoveUI());
    }

    private void Awake()
    {
         rectTransform = GetComponent<RectTransform>();
    }

    private IEnumerator MoveUI()
    {
        Vector2 posRect = rectTransform.localPosition;
        //Vector2 pos = transform.position;

        while (isActiveAndEnabled)
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
