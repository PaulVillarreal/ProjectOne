using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAlpha : TweenBase
{
    [SerializeField]
    private float _startAlpha;
    [SerializeField]
    private float _endAlpha;

    private CanvasGroup _targetGroup;

    private void Start()
    {
        if (_target == null)
        {
            _target = gameObject;
        }
        _targetGroup = _target.GetComponent<CanvasGroup>();

        if (_playOnAwake)
        {
            StartCoroutine(RunAlpha());
        }
    }

    public override void RunTween()
    {
        StartCoroutine(RunAlpha());
    }

    public void RunTween(float startAlpha, float endAlpha)
    {
        _startAlpha = startAlpha;
        _endAlpha = endAlpha;

        StartCoroutine(RunAlpha());
    }

    IEnumerator RunAlpha()
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

            _targetGroup.alpha = _timeCurve.Evaluate(lerpAmt);
            yield return 0;
        }

        if (_wrapMode == WrapMode.LOOP)
        {
            if (!_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunAlpha());
        }
        else if (_wrapMode == WrapMode.PINGPONG)
        {
            while (lerpAmt > 0)
            {
                lerpAmt -= Time.deltaTime * timeStep;
                _targetGroup.alpha = _timeCurve.Evaluate(lerpAmt);
                yield return 0;
            }

            if (!_delayOnLoop)
            {
                _delayTime = 0;
            }
            StartCoroutine(RunAlpha());
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
