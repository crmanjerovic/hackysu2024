using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add to asset menu
[CreateAssetMenu(menuName = "Coins")]

public class coinInfo : ScriptableObject
{
    int coinsOwned = 0;

    public void StartGame()
    {
        coinsOwned = 0;
    }

    public void PickUpCoin()
    {
        coinsOwned = coinsOwned + 1;
    }

    public int DisplayCoins()
    {
        return coinsOwned;
    }

}
