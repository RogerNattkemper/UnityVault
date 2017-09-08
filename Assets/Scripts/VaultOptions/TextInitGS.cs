using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextInitGS : MonoBehaviour {

    public Text Slidertext;

    // Use this for initialization
    void Start()
    {
        Slidertext = GetComponent<Text>();
        int holder = GlobalControl.control.initialrowsize;
        Slidertext.text = GridSizeText(holder);
    }

    public void UpdateText(float newvalue)
    {
        Slidertext = GetComponent<Text>();
        int holder = Mathf.RoundToInt(newvalue);
        Slidertext.text = GridSizeText(holder);
    }

    string GridSizeText(int num)
    {
        string tmp, snum;
        snum = num.ToString();
        tmp = snum + "X" + snum;
        return tmp;
    }
}
