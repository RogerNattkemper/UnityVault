using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureClass : MonoBehaviour
{
    public string TreasureName; // THe name of the treasure
    public float TreasureValue; // What is this treasure worth? (Not sure how I'd use this yet)
    public int TreasureMaxQty; // How many of these can the player have
    public int TreasureQty; // How many the player does have
    public string TreasureID; // The code for this treasure

    public TreasureClass(string name, float value, int max, string ID)
    {
        TreasureName = name;
        TreasureValue = value;
        TreasureMaxQty = max;
        TreasureQty = 1; // We'll just add this number to whatever the number is that the player already has.
        TreasureID = ID;
    }
}
