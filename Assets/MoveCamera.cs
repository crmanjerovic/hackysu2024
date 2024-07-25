using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    GameObject Player;
    Transform ct, pt;
    Camera c;

    public enum State {DISABLED, ENABLED};
    public State state;

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Camera>();
        ct = GetComponent<Transform>();
        Player = GameObject.FindWithTag("Player");
        pt = Player.GetComponent<Transform>(); 

        ct.position = new Vector3(-5, 50, -5);
        c.orthographicSize = 30f;

        state = State.DISABLED;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.ENABLED) {
            ct.position = new Vector3(pt.position.x - 5, pt.position.y + 8, pt.position.z - 5);
        }
    }

    public void enableCamera() {
        state = State.ENABLED;
        c.orthographicSize = 5f;
    }
}
