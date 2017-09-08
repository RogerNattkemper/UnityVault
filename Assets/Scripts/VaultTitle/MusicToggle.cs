using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        bool music;
        Toggle tg = GetComponent<Toggle>();
        music = GlobalControl.control.MusicOn;

        if (music) tg.isOn = true; 
	}
	
	// Update is called once per frame
	public void UpdateMusicToggle (bool tog) 
    {
        GlobalControl.control.MusicOn = tog;
	}
}
