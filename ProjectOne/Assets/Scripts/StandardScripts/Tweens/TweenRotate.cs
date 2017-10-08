using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRotate : TweenBase
{
    [SerializeField]
    private Vector3 _startRot;
    [SerializeField]
    private Vector3 _endRot;

    private Transform _transform;

    private void Start()
    {
        if (_target == null)
        {
            _target = gameObject;
        }

        _transform = _target.transform;

        if (_playOnAwake)
        {
            StartCoroutine(RunRotate());
        }
    }

    public override void RunTween()
    {
        StartCoroutine(RunRotate());
    }

    IEnumerator RunRotate()
    {
        yield return new WaitForSeconds(_delayTime);

        float lerpAmt = 0;

        float timeStep = 1;

        if (_timeScale != 0)
        {
            timeStep = 1 / _timeScale;
        }

        while(lerpAmt < 1)
        {
            lerpAmt += Time.deltaTime * timeStep;

            _transform.localEulerAngles = Vector3.LerpUnclamped(_startRot, _endRot, _timeCurve.Evaluate(lerpAmt));
            yield return 0;
        }

        if (_wrapMode == WrapMode.LOOP)
        {
            if (!_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunRotate());
        }
        else if (_wrapMode == WrapMode.PINGPONG)
        {
            while (lerpAmt > 0)
            {
                lerpAmt -= Time.deltaTime * timeStep;
                _transform.localEulerAngles = Vector3.LerpUnclamped(_startRot, _endRot, _timeCurve.Evaluate(lerpAmt));
                yield return 0;
            }

            if (_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunRotate());
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
