using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Animation anim;

    private void Start()
    {
        StartCoroutine(running());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StopAllCoroutines();
            StartCoroutine(backFlip());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopAllCoroutines();
            StartCoroutine(fallingBackDeath());
        }
    }

    IEnumerator running()
    {
        //anim.CrossFade(PlayerAnimations.Walking);
        //yield return new WaitForSecondsRealtime(5);

        float speed = 0.1f;
        anim[PlayerAnimations.Run2].speed = speed;
        anim.CrossFade(PlayerAnimations.Run2);

        while (anim[PlayerAnimations.Run2].speed < 1)
        {
            speed += Time.deltaTime;
            anim[PlayerAnimations.Run2].speed = speed;
            yield return 0;
        }

        yield return 0;
    }

    IEnumerator backFlip()
    {
        anim[PlayerAnimations.Run2].time = 0;
        anim.CrossFade(PlayerAnimations.BackFlip);
        while (anim.isPlaying)
            yield return 0;

        StartCoroutine(running());
    }

    IEnumerator fallingBackDeath()
    {
        anim.CrossFade(PlayerAnimations.Death1);
        yield return 0;
    }
}
