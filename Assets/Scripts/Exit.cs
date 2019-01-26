using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool isOpen = false;
    public bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float moveDistance = 5f;
    private float lerpTime = 5;
    private float currentLerpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        //targetPosition = startPosition + new Vector3(0f, 3, 0f);
        targetPosition = startPosition + Vector3.up * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    isOpen = true;
        //}

        //if (isOpen == true)
        //{
        //    currentLerpTime += Time.deltaTime;
        //    if (currentLerpTime >= lerpTime)
        //    {
        //        currentLerpTime = lerpTime;
        //    }
        //}
        //transform.position = Vector3.Lerp(startPosition, targetPosition, currentLerpTime / lerpTime);

        // UP
        if (Input.GetKeyDown(KeyCode.G))
        {
            isMoving = true;
            targetPosition = startPosition + Vector3.up * moveDistance;
            currentLerpTime = 0;
        }

        // DOWN
        if (Input.GetKeyDown(KeyCode.H))
        {
            isMoving = true;
            targetPosition = startPosition;
            currentLerpTime = 0;
        }

        if (isMoving == true)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, currentLerpTime / lerpTime);
    }
}
