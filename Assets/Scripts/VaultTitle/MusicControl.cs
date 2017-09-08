using UnityEngine;
using System.Collections;

public class MusicControl : MonoBehaviour
{
    AudioSource music;
    // Use this for initialization
    void Start()
    {
        music = GetComponent<AudioSource>();
        float vol = GlobalControl.control.MusicVol;

        if (vol > 0) music.mute = false;
        else music.mute = true;

        music.volume = vol;

        if (!music.mute) music.Play();
    }

    void Update()
    {
        if (music.volume != GlobalControl.control.MusicVol)
        {
            float vol = GlobalControl.control.MusicVol;

            if (vol > 0) music.mute = false;
            else music.mute = true;

            if ((vol > 0) && (music.mute)) music.Play();
            else if ((vol == 0) && (!music.mute)) music.Stop();
          
            music.volume = vol;
        }
    }


    public void PlayClick()
    {
        GetComponent<AudioSource>().Play();
    }
}
