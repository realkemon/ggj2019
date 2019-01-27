using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planets : MonoBehaviour
{
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 targetPosition;
    public float currentLerpTime;

    private float lerpTime = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((currentLerpTime / lerpTime) >= 1)
        {
            currentLerpTime = 0;
        }

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
        }
        transform.position = Vector3.Lerp(position2, position1, currentLerpTime / lerpTime);
    }

    void OnValidate()
    {
        transform.position = position1;
    }
}
