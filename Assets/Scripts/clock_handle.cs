using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_handle : MonoBehaviour
{
    public float speed1 = 10.0f; // speed (6.28 means about 1 second)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed1 * Time.deltaTime);
    }
}
