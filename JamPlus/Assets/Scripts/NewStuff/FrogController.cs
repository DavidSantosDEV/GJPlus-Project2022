using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]
public class AudioAndVolume
{
    public AudioClip audio;
    public float volume;
}

public class FrogController : MonoBehaviour, MovingActors
{

    [Header("Player Settings")]
    [SerializeField]
    AudioAndVolume jumpSound;
    [SerializeField]
    AudioAndVolume eatSound;
    [SerializeField]
    AudioAndVolume swimSound;


    bool bIsMoving = false;
    private PlayerActions playerInput;
    int SelectedIndex = 0;
    Point CurrentPoint;
    public UnityEvent OnPlayerJumped;
    public UnityEvent OnPlayerEatFLy;
    private Animator _animator;

    int currentAnim;

    public readonly int jumpAnim = Animator.StringToHash("Jumping");
    public readonly int idleAnim = Animator.StringToHash("Idle");
    public readonly int eatingAnim = Animator.StringToHash("Eating");
    public readonly int moonAnim = Animator.StringToHash("Moon");

    public Point GetCurrent() { return CurrentPoint; }

    AudioSource audioPlayer;
    private void Awake()
    {
        playerInput = new PlayerActions();

        playerInput.Enable();


        _animator = GetComponent<Animator>();
        //playerInput.Selection.Selection.started += OnSelectionChanged;
        //playerInput.Selection.MoveTo.started += ChosePath;

        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.spatialBlend = 0;
    }

    public void PlaySwimSound()
    {
        audioPlayer.PlayOneShot(swimSound.audio,swimSound.volume);
    }
    public void ChangeAnimation(int animation)
    {
        if (currentAnim == animation)
        {
            return;
        }
        if (currentAnim == eatingAnim && animation == idleAnim)
        {
            return;
        }
        if (currentAnim == moonAnim) return;

        if (animation == eatingAnim)
        {
            //audioPlayer.PlayOneShot(eatSound.audio,eatSound.volume);
            GameManager.Instance.PlaySoundEffect(eatSound);
        }

        currentAnim = animation;
        _animator?.CrossFade(animation, 0f, 0);
    }
    public void ChoseThis(Point thePoint)
    {
        if (!GameManager.Instance.CanMove())
        {
            return;
        }
        if (bIsMoving) return;
        if (CurrentPoint)
        {
            if (CurrentPoint.GetNextPoints().Contains(thePoint))
            {
                OnPlayerJumped.Invoke();
                StartCoroutine(MoveTowards(thePoint));
            }
        }
    }

    public void SetCurrentPoint(Point newPoint)
    {
        if (!newPoint) return;
        if (CurrentPoint)
        {
            //List<Point> nextPoint = CurrentPoint.GetNextPoints();
            foreach (Point pa in CurrentPoint.GetNextPointNoModifiers())
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
            foreach(Point pa in points)
            {
                pa.ToggleSelected(true);
            }
        }
        else
        {
            //No points
            Debug.Log("No points");
        }
        SelectedIndex = 0;
    }

    private IEnumerator MoveTowards(Point point)
    {   
        GameManager.Instance.PlaySoundEffect(jumpSound);
        bIsMoving=true;
        float Value=0f;
        Vector2 originalPosition = transform.position;
        CheckFlipping(originalPosition,point.transform.position);
        ChangeAnimation(point.GetIsFinal()? moonAnim : jumpAnim);
        foreach (Point p in CurrentPoint.GetNextPoints())
        {
            p.ToggleSelected(false);
        }
        while (Value<1)
        {
            Value += Time.deltaTime * GameManager.Instance.GetMovementSpeed();
            Value = Mathf.Clamp01(Value);
            transform.position = Vector3.Lerp(originalPosition, point.transform.position, Value);
            yield return null;
        }
        transform.position = Vector3.Lerp(originalPosition, point.transform.position, 1);
        bIsMoving = false;
        ChangeAnimation(idleAnim);
        SetCurrentPoint(point);
        Invoke(nameof(Check), 0.5f); 
    }

    void Check()
    {
        GameManager.Instance.CheckPoints();
    }

    void CheckFlipping(Vector2 posOriginal, Vector2 pos)
    {
        Vector2 value = (posOriginal - pos).normalized;
        gameObject.transform.localScale = value.x < 0 ? Vector2.one : new Vector2(-1, 1);
    }

    public bool GetIsMoving()
    {
        return bIsMoving;
        throw new System.NotImplementedException();
    }
}
