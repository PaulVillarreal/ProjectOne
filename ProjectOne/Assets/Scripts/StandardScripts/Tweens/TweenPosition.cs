using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenPosition : TweenBase
{
    [SerializeField]
    private Vector3 _startPos;
    [SerializeField]
    private Vector3 _endPos;
    [SerializeField]
    private bool _useWorldSpace = false;

    private Transform _transForm;

    private void Start()
    {
        if (_target == null)
        {
            _target = gameObject;
        }

        _transForm = _target.transform;

        if (_playOnAwake)
        {
            StartCoroutine(RunMove());
        }
    }

    public override void RunTween()
    {
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

            if (_useWorldSpace)
            {
                _transForm.position = Vector3.LerpUnclamped(_startPos, _endPos, _timeCurve.Evaluate(lerpAmt));
                yield return 0;
            }
            else
            {
                _transForm.localPosition = Vector3.LerpUnclamped(_startPos, _endPos, _timeCurve.Evaluate(lerpAmt));
                yield return 0;
            }
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

                if (_useWorldSpace)
                {
                    _transForm.position = Vector3.LerpUnclamped(_startPos, _endPos, _timeCurve.Evaluate(lerpAmt));
                    yield return 0;
                }
                else
                {
                    _transForm.localPosition = Vector3.LerpUnclamped(_startPos, _endPos, _timeCurve.Evaluate(lerpAmt));
                    yield return 0;
                }
            }
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
