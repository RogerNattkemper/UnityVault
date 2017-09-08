using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour
{
    AudioSource sounds;
    // Use this for initialization
    void Start()
    {
        sounds = GetComponent<AudioSource>();
        float vol = GlobalControl.control.SoundVol;

        if (vol > 0) sounds.mute = false;
        else sounds.mute = true;

        sounds.volume = vol;
        
    }

    void Update()
    {
        if (sounds.volume != GlobalControl.control.SoundVol)
        {
            float vol = GlobalControl.control.SoundVol;
            if (vol > 0) sounds.mute = false;
            else sounds.mute = true;

            sounds.volume = vol;
        }
    }


    public void PlayClick()
    {
        GetComponent<AudioSource>().Play();
    }

}
