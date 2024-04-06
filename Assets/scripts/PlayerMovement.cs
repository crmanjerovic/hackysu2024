using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardspeed = 5.0f;
    public float turnspeed = 20.0f;


    // Update is called once per frame
    void Update()
    {
        //move along local z axis
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0, forwardspeed * Time.deltaTime));
        }

        //rotating 
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, turnspeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -turnspeed * Time.deltaTime, 0));
        }
    }
}
