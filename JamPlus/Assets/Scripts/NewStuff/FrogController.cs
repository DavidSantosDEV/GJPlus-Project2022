using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FrogController : MonoBehaviour
{

    [Header("Player Settings")]

    bool bIsMoving = false;

    private PlayerActions playerInput;

    int SelectedIndex = 0;
    Point CurrentPoint;
    Point selectedPoint;

    public UnityEvent OnPlayerJumped;

    private Animator _animator;


    public readonly int jumpAnim = Animator.StringToHash("Jumping");
    public readonly int idleAnim = Animator.StringToHash("Idle");
    public readonly int eatingAnim = Animator.StringToHash("Eating");


    private void Awake()
    {
        playerInput = new PlayerActions();

        playerInput.Enable();


        _animator = GetComponent<Animator>();
        //playerInput.Selection.Selection.started += OnSelectionChanged;
        //playerInput.Selection.MoveTo.started += ChosePath;
    }


    public void ChangeAnimation(int animation)
    {
        _animator?.CrossFade(animation, 0f, 0);
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

    } //Legacy keyboard code

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
        if (!newPoint) return;
        if (CurrentPoint)
        {
            foreach (Point pa in CurrentPoint.GetNextPoints())
            {
                pa.ToggleSelected(false);
            }
            CurrentPoint.OnLeave();
            
        }
        
        CurrentPoint = newPoint;
        newPoint.ToggleSelected(false);
        newPoint.OnPlayerEntered();
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
        CheckFlipping(originalPosition,selectedPoint.transform.position);
        ChangeAnimation(jumpAnim);
        while (Value<1)
        {
            Value += Time.deltaTime * GameManager.Instance.GetMovementSpeed();
            Value = Mathf.Clamp01(Value);
            transform.position = Vector3.Lerp(originalPosition, selectedPoint.transform.position, Value);
            yield return null;
        }
        transform.position = Vector3.Lerp(originalPosition, selectedPoint.transform.position, 1);
        bIsMoving = false;
        ChangeAnimation(idleAnim);
        SetCurrentPoint(selectedPoint);
    }

    void CheckFlipping(Vector2 posOriginal, Vector2 pos)
    {
        Vector2 value = (posOriginal - pos).normalized;
        gameObject.transform.localScale = value.x < 0 ? Vector2.one : new Vector2(-1, 1);
    }

}
