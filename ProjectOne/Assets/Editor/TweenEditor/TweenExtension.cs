using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenAlpha))]
public class TweenExtension : Editor
{
    private static List<IEnumerator> CoroutineInProgress = new List<IEnumerator>();
    private static List<IEnumerator> CoroutinesToRemove = new List<IEnumerator>();
    int currentCoroutine = 0;

    private void Awake()
    {
        EditorApplication.update += ExecuteCoroutine;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TweenBase _tweenBase = (TweenBase)target;

        if (GUILayout.Button("Run Tween"))
        {
            _tweenBase.RestartTween();
        }
    }

    public static IEnumerator StartCoroutineEditor(IEnumerator newCoroutine)
    {
        CoroutineInProgress.Add(newCoroutine);
        return newCoroutine;
    }

    private void ExecuteCoroutine()
    {
        if (CoroutineInProgress.Count <= 0)
        {
            return;
        }

        foreach (IEnumerator corou in CoroutineInProgress)
        {
            if (!corou.MoveNext())
            {
                CoroutinesToRemove.Add(corou);
            }
        }

        foreach (IEnumerator corou in CoroutinesToRemove)
        {
            CoroutineInProgress.Remove(corou);
        }

        CoroutinesToRemove.Clear();
    }
}
