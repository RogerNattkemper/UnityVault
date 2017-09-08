using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextStartRulesNum : MonoBehaviour {

    public Text Slidertext;

	// Use this for initialization
	void Start () 
    {
        Slidertext = GetComponent<Text>();
	    int holder = GlobalControl.control.initialrules;
        Slidertext.text = holder.ToString();
	}
	
    public void UpdateText(float newvalue)
    {
        Slidertext = GetComponent<Text>();
        int holder = Mathf.RoundToInt(newvalue);
        Slidertext.text = holder.ToString();
     }
}