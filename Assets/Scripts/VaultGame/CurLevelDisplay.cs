using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurLevelDisplay : MonoBehaviour
{

    int CurLev;
    int NumLevs;
    Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        CurLev = GameDisplay.levelnumber;
        NumLevs = GlobalControl.control.NumOfLevels; 
        text.text = "Current Level: 1 / " + NumLevs;
    }

    public void Update()
    {
        if (CurLev != GameDisplay.levelnumber) 
        {
            CurLev = GameDisplay.levelnumber;
            int dispnum = CurLev + 1;
            text.text = "Current Level: " + dispnum + " / " + NumLevs;
        }
    }
}
