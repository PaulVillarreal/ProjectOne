using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TweenBase : MonoBehaviour
{
    public enum WrapMode
    {
        ONCE,
        LOOP,
        PINGPONG,
    }

    [SerializeField]
    public GameObject _target;
    [SerializeField]
    public bool _playOnAwake = false;
    [SerializeField]
    public float _delayTime = 0;
    [SerializeField]
    public float _timeScale = 1;
    [SerializeField]
    public WrapMode _wrapMode = WrapMode.ONCE;
    [SerializeField]
    protected AnimationCurve _timeCurve;
    [SerializeField]
    public bool _delayOnLoop = false;
    [SerializeField]
    public bool _destroyOnEnd = false;
    [SerializeField]
    protected UnityEvent _callOnEnd;

    private static List<IEnumerator> CoroutineInProgress = new List<IEnumerator>();

    public virtual void RunTween()
    {

    }

    public virtual void RestartTween()
    {

    }

    public void AddEndCall(UnityAction functionToCall)
    {
        _callOnEnd.AddListener(new UnityAction(functionToCall));
    }

    public void ClearEndCalls()
    {
        _callOnEnd.RemoveAllListeners();
    }

    public void RemoveEndCall(UnityAction functionToRemove)
    {
        _callOnEnd.RemoveListener(functionToRemove);
    }

    public static IEnumerator StartCoroutineInEditor(IEnumerator newCoroutine)
    {
        CoroutineInProgress.Add(newCoroutine);
        return newCoroutine;
    }
}
