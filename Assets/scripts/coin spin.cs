using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinspin : MonoBehaviour
{
    //speed the coin is spinning
    public float speed = 0.75f;
    public coinInfo coinOb;


    // Start is called before the first frame update
    void Start()
    {
        //This is for initializing the coin count = 0
        coinOb.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        //spin the coin
        transform.Rotate(0f, speed, 0f, Space.Self);
    }

    //I hit something
    void OnCollisionEnter(Collision collision)
    {
        GameObject g = collision.gameObject;

        //If the thing that hit me was the player, 
        //I need to add to coin count and stop existing.
        if (g.CompareTag("Player"))
        {
            coinOb.PickUpCoin();
            Destroy(this.gameObject);
        }

    }
}
