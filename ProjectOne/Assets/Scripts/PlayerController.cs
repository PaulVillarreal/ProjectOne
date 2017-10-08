using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Animation anim;
    [SerializeField]
    float _acceleration;
    [SerializeField]
    float _decceleration;
    [SerializeField]
    float _maxSpeed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float jumpDeccel;

    [SerializeField]
    Vector3 _velocity;
    [SerializeField]
    float Speed;
    Transform myTransform;
    float currJumpForce;
    bool isJumping = false;
    bool isSliding = false;
    bool facingRight = true;
    bool isRunning = false;

    private void Start()
    {
        myTransform = transform;
        //StartCoroutine(running());
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(backFlip());
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(fallingBackDeath());
        //}

        isRunning = false;
        if (!isJumping && !isSliding)
        {
            if (Input.GetKey(KeyCode.D))
            {
                isRunning = true;
                if (!facingRight)
                {
                    RotatePlayer();
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                isRunning = true;
                if (facingRight)
                {
                    RotatePlayer();
                }
            }
        }



        if (isRunning)
        {
            if (Speed < _maxSpeed)
            {
                Speed += _acceleration * Time.deltaTime;
                if (Speed >= _maxSpeed)
                    Speed = _maxSpeed;
            }
        }
        else if(!isJumping || isSliding)
        {
            Speed -= _decceleration * Time.deltaTime;
            if (Speed <= 0)
                Speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                currJumpForce = jumpForce;
                isJumping = true;
                //StartCoroutine(Jump());
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isSliding)
                StartCoroutine(Slide());
        }

        if (isJumping)
        {
            if (currJumpForce > 0)
            {
                currJumpForce -= jumpDeccel * Time.deltaTime;
            }
            else
            {
                currJumpForce = 0;
            }
        }

        _velocity = transform.forward * Speed;
        _velocity.y = (transform.up * currJumpForce).y;

        transform.position += _velocity;

        if(!isJumping && !isSliding)
            UpdateRunAnim(Speed);
    }

    void RotatePlayer()
    {
        facingRight = !facingRight;
        myTransform.Rotate(Vector3.up, 180);
        Speed *= -.25f;
    }

    void UpdateRunAnim(float Speed)
    {
        if (Speed > 0)
        {
            float runSpeed = Speed / _maxSpeed;
            if (runSpeed <= 0.1f)
                runSpeed = 0.1f;

            anim[PlayerAnimations.Run2].speed = runSpeed;
            anim.Play(PlayerAnimations.Run2);
        }
        else
        {
            anim.CrossFade(PlayerAnimations.StandingPose);
        }
    }

    IEnumerator Jump()
    {
        isJumping = true;
        anim.CrossFade(PlayerAnimations.JumpUp1);
        while (anim.isPlaying)
        {
            yield return 0;
        }
        isJumping = false;   
    }

    IEnumerator Slide()
    {
        isSliding = true;
        anim.CrossFade(PlayerAnimations.Slide);
        while (anim.isPlaying)
        {
            yield return 0;
        }
        isSliding = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
        {
            Speed -= 0.02f;
            isJumping = false;
        }
    }
}
