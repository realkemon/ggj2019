using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureObjects : MonoBehaviour
{
    public enum furniture { CHAIR, TABLE};
    public furniture type;
    public bool isPickedUp;
    public bool isBlack;

       
    private float angle1 = 30; // swing angle = 2 * angle
    private float speed1 = 3.0f; // speed (6.28 means about 1 second)
    //private float speed1 = 0.5f; // speed (6.28 means about 1 second)

    private float angle2 = -30; // swing angle = 2 * angle
    private float speed2 = 3.0f; // speed (6.28 means about 1 second)
    //private float speed2 = 0.5f; // speed (6.28 means about 1 second)




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            transform.localEulerAngles = new Vector3(0f,0f, angle1 * Mathf.Sin(speed1 * Time.time));
        }
    }

    public void SetColour ()
    {
        //isPlayerOne ? "black" : "white";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: FURNITURE CAN COLLIDE
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //TODO: FURNITURE CAN COLLIDE
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //TODO: FURNITURE CAN COLLIDE
    }
}
