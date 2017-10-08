using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : TweenBase
{
    [SerializeField]
    private Vector3 startScale;
    [SerializeField]
    private Vector3 endScale;

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
            StartCoroutine(RunScale());
        }
    }

    public override void RunTween()
    {
        StartCoroutine(RunScale());
    }

    IEnumerator RunScale()
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

            _transform.localScale = Vector3.LerpUnclamped(startScale, endScale, _timeCurve.Evaluate(lerpAmt));
            yield return 0;
        }

        if (_wrapMode == WrapMode.LOOP)
        {
            if (!_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunScale());
        }
        else if (_wrapMode == WrapMode.PINGPONG)
        {
            while (lerpAmt > 0)
            {
                lerpAmt -= Time.deltaTime * timeStep;
                _transform.localScale = Vector3.LerpUnclamped(startScale, endScale, _timeCurve.Evaluate(lerpAmt));
                yield return 0;
            }

            if (_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunScale());
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
