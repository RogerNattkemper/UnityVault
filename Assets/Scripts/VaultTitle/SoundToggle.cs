using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        bool sound;
        Toggle tg = GetComponent<Toggle>();
        sound = GlobalControl.control.SoundOn;

        if (sound) tg.isOn = true; 
	}

    public void UpdateSoundToggle(bool tog)
    {
        GlobalControl.control.SoundOn = tog;
    }
	
}