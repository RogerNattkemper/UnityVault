using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerNameDisplay : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () 
    {
        text = GetComponent<Text>();
        string name = GlobalControl.control.PlayerName;
        text.text = name;
	}
	
    public void ChangePlayerName(string nme)
    {
        text = GetComponent<Text>();
        string name = nme;
        text.text = name;

    }
}
