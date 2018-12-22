using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TweenPositionUI : TweenBase
{
    [SerializeField]
    private Vector3 _startPos;
    [SerializeField]
    private Vector3 _endPos;

    private RectTransform _transform;

    private void Start()
    {
        if (_target == null)
        {
            _target = gameObject;
        }

        _transform = _target.GetComponent<RectTransform>();

        if (_playOnAwake)
        {
            StartCoroutine(RunMove());
        }
    }

    public override void RunTween()
    {
        StartCoroutine(RunMove());
    }

    public override void RestartTween()
    {
        StopAllCoroutines();
        StartCoroutine(RunMove());
    }

    IEnumerator RunMove()
    {
        yield return new WaitForSeconds(_delayTime);

        float lerpAmt = 0;

        float timeStep = 1;

        if (_timeScale != 0)
        {
            timeStep = 1 / _timeScale;
        }

        while (lerpAmt < 1)
        {
            lerpAmt += Time.deltaTime * timeStep;
            _transform.anchoredPosition = Vector3.LerpUnclamped(_startPos, _endPos, _timeCurve.Evaluate(lerpAmt));
            yield return 0;
        }

        if (_wrapMode == WrapMode.LOOP)
        {
            if (!_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunMove());
        }
        else if (_wrapMode == WrapMode.PINGPONG)
        {
            while (lerpAmt > 0)
            {
                lerpAmt -= Time.deltaTime * timeStep;
                _transform.anchoredPosition = Vector3.LerpUnclamped(_startPos, _endPos, _timeCurve.Evaluate(lerpAmt));
                yield return 0;
            }

            if (!_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunMove());
        }
        else
        {
            if (_callOnEnd != null)
            {
                _callOnEnd.Invoke();
            }
            if (_destroyOnEnd)
            {
                Destroy(_target);
            }
        }
    }
}
