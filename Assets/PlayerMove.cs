using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 1.0F; 

    Animator animator;
    Rigidbody rb;

    public enum State {DISABLED, ENABLED};
    public State state;

    void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        state = State.DISABLED;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.ENABLED:
                float dx = 0; // Change in x
                float dz = 0; // Change in z

                // Detect key events for different directions (including arrow key)
                if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) // Move forward
                {
                    dz = 1;
                }
                if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) // Move backward
                {
                    dz = -1;
                }
                if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) // Move left
                {
                    dx = -1;
                }
                if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) // Move right
                {
                    dx = 1;
                }
                if (Input.GetKey("r")) { // Reset position
                    transform.position = new Vector3(30, 5, 30);
                }
                if (Input.GetKey(KeyCode.Space)) {
                    rb.AddForce(new Vector3(0,15,0));
                }

                // Move in that direction (in global space) at given speed
                transform.Translate(new Vector3(dx, 0, dz) * speed * Time.deltaTime, Space.World);
            
                // Face in that direction (if moving)
                if ((dx != 0 || dz != 0))
                {
                    transform.LookAt(transform.position + new Vector3(dx, 0, dz));
                    animator.SetBool("isRunning", true);
                }
                else {
                    animator.SetBool("isRunning", false);
                }
                break;
        }
    }

    public void enableCharacter(){
        rb.useGravity = true;
        state = State.ENABLED;
        Debug.Log("CHARACTER ENABLED");
    }
}
