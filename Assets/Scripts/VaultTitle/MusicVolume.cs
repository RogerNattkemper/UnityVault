using UnityEngine;
using System.Collections;

public class MusicVolume : MonoBehaviour {

    public void UpdateVolume(float newvalue)
    {
        GlobalControl.control.MusicVol = newvalue;
    }
}
