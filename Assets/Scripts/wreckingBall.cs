using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wreckingBall : MonoBehaviour
{
    public float angle1 = 90; // swing angle = 2 * angle
    public float speed1 = 1.0f; // speed (6.28 means about 1 second)
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(0f, 0f, angle1 * Mathf.Sin(speed1 * Time.time));
    }
}
