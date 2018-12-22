using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TweenRotateToObject : TweenBase
{
    [SerializeField]
    private GameObject _targetObj;

    private Transform _transfrom;

    private void Start()
    {
        if (_target == null)
        {
            _target = gameObject;
        }

        _transfrom = _target.transform;

        if (_playOnAwake)
        {
            StartCoroutine(RunRotate());
        }
    }

    public override void RunTween()
    {
        StartCoroutine(RunRotate());
    }

    public override void RestartTween()
    {
        StopAllCoroutines();
        StartCoroutine(RunRotate());
    }

    IEnumerator RunRotate()
    {
        yield return new WaitForSeconds(_delayTime);

        float lerpAmt = 0;

        float timeStep = 1;

        Quaternion baseRot = _transfrom.rotation;
        Vector3 direction = _targetObj.transform.position - _transfrom.position;
        direction.Normalize();

        Quaternion lookRot = Quaternion.LookRotation(direction);

        if(_timeScale != 0)
        {
            timeStep = 1 / _timeScale;
        }

        while (lerpAmt < 1)
        {
            lerpAmt += Time.deltaTime * timeStep;

            _transfrom.rotation = Quaternion.LerpUnclamped(baseRot, lookRot, -_timeCurve.Evaluate(lerpAmt));
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

                _transfrom.rotation = Quaternion.LerpUnclamped(baseRot, lookRot, -_timeCurve.Evaluate(lerpAmt));
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
