using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script is placed on the Canvas, and the TMP object has to be dragged in
public class junkUI : MonoBehaviour
{
    public coinInfo coins;
    
    public TMP_Text coinText; // The TextMeshPro object to display

    // Update is called once per frame
    void Update()
    {
        int count = coins.DisplayCoins();

        coinText.SetText("Coin Count: " + count);
    }
}
