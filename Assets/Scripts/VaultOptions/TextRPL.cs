using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TextRPL : MonoBehaviour {
    public Text Slidertext;
    public int holder;

    // Use this for initialization
    void Start()
    {
        Slidertext = GetComponent<Text>();
        holder = GlobalControl.control.rulesperlevel;
        Slidertext.text = holder.ToString();
    }

    public void UpdateText(float newvalue)
    {
        holder = Mathf.RoundToInt(newvalue);
        Slidertext.text = holder.ToString();
    }
}
