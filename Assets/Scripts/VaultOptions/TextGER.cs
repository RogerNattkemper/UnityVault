using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextGER : MonoBehaviour {
    Text Slidertext;

    // Use this for initialization
    void Start()
    {
        Slidertext = GetComponent<Text>();
        int holder = GlobalControl.control.GridExpansionRate;
        Slidertext.text = GridExpansionText(holder);
    }

    public void UpdateText(float newvalue)
    {
        Slidertext = GetComponent<Text>();
        int holder = Mathf.RoundToInt(newvalue);
        Slidertext.text = GridExpansionText(holder);
    }

    string GridExpansionText(int num)
    {
        string tmp = "";
        string snum = num.ToString();

        switch (num)
        {
            case 0: 
                tmp = "Col AND Row every Level";
                break;
            case 1: 
                tmp = "Col OR Row every Level";
                break;
            default: 
                tmp = "Col OR Row every " + snum + " Levels";
                break;
        }
        
        return tmp;
    }
}
