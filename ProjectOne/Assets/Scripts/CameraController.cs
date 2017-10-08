using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform Target;
    [SerializeField]
    float maxDist;

    Transform myTransform;
    float lastXPos = 0;

    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        //float distance = Target.position.x - myTransform.position.x;

        //if (distance > maxDist)
        //{
            Vector3 tempPos = myTransform.position;
            tempPos.x = Target.position.x;
            
            myTransform.position = tempPos;
        //}
    }
}
