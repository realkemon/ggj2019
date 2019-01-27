using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 targetPosition;

    private bool goingDown;
    
    private float lerpTime = 5;
    private float currentLerpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = position1;
        goingDown = true;
        currentLerpTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingDown == true)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
                goingDown = false;
            }
        }
        else
        {
            currentLerpTime -= Time.deltaTime;
            if (currentLerpTime <= 0f)
            {
                currentLerpTime = 0f;
                goingDown = true;
            }
        }
        transform.position = Vector3.Lerp(position1, position2, currentLerpTime / lerpTime);
    }

    void OnValidate()
    {
        transform.position = position1;
    }
}
