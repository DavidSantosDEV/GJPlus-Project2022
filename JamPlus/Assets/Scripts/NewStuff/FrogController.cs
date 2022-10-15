using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FrogController : MonoBehaviour
{

    [Header("Player Settings")]
    [SerializeField]
    private float moveSpeed = 10f;

    bool bIsMoving = false;

    private PlayerActions playerInput;

    int SelectedIndex = 0;
    Point CurrentPoint;
    Point selectedPoint;

    public UnityEvent OnPlayerJumped;


    private void Awake()
    {
        playerInput = new PlayerActions();

        playerInput.Enable();

        //playerInput.Selection.Selection.started += OnSelectionChanged;
        //playerInput.Selection.MoveTo.started += ChosePath;
    }

    void OnUpdateIndex()
    {
        selectedPoint = CurrentPoint.GetNextPoints()[SelectedIndex];
    }

    void OnSelectionChanged(InputAction.CallbackContext context)
    {

        if (bIsMoving) return;

        Vector2 newValue = context.ReadValue<Vector2>();

        /// - or +
        // 1-2-0-1-2
        if (newValue.x<0)
        {
            //--
            SelectedIndex--;
            if (SelectedIndex<0)
            {
                SelectedIndex = CurrentPoint.GetNextPoints().Count-1;
            }
        }
        else
        {
            SelectedIndex++;
            if (SelectedIndex> CurrentPoint.GetNextPoints().Count - 1)
            {
                SelectedIndex=0;
            }
            //++
        }
        Debug.Log("Current Index:" + SelectedIndex);
        OnUpdateIndex();

    }

    private void Update()
    {
        if (selectedPoint)
        {
            //Vector2.Lerp(SelectorMouse.transform.position, selectedPoint.transform.position, Time.deltaTime);
        }
    }

    void ChosePath(InputAction.CallbackContext context)
    {
        if (bIsMoving) return;
        StartCoroutine(MoveTowards());
        //gameObject.transform.position = SelectedIndex
    }

    public void ChoseThis(Point thePoint)
    {
        if (bIsMoving) return;
        if (CurrentPoint)
        {
            if (CurrentPoint.GetNextPoints().Contains(thePoint))
            {
                selectedPoint = thePoint;
                OnPlayerJumped.Invoke();
                StartCoroutine(MoveTowards());
            }
        }
    }

    public void SetCurrentPoint(Point newPoint)
    {
        if (CurrentPoint)
        {
            foreach (Point pa in CurrentPoint.GetNextPoints())
            {
                pa.ToggleSelected(false);
            }
        }
        CurrentPoint = newPoint;
        List<Point> points = newPoint.GetNextPoints();
        if (points.Count>0)
        {
            selectedPoint = points[0];
            foreach(Point pa in points)
            {
                pa.ToggleSelected(true);
            }
        }
        SelectedIndex = 0;
    }

    
    private IEnumerator MoveTowards()
    {
        bIsMoving=true;
        float Value=0f;
        Vector2 originalPosition = transform.position;
        while (Value<1)
        {
            Value += Time.deltaTime * moveSpeed;
            Value = Mathf.Clamp01(Value);
            transform.position = Vector3.Lerp(originalPosition, selectedPoint.transform.position, Value);
            yield return null;
        }
        bIsMoving = false;
        SetCurrentPoint(selectedPoint);
    }

}
