using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI : MonoBehaviour
{

    public float timeOfWait;
    public float moveAmmount;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(MoveUI());
    }

    private IEnumerator MoveUI()
    {
        Vector2 pos = transform.position;
        while (true)
        {
            
            transform.position= new Vector2(pos.x,pos.y+moveAmmount);
            yield return new WaitForSeconds(timeOfWait);
            transform.position = new Vector2(pos.x, pos.y - moveAmmount);
            yield return new WaitForSeconds(timeOfWait);
        }
    }
}
