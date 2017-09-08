using UnityEngine;
using System.Collections;

public class SoundVolume : MonoBehaviour {

    public void UpdateVolume(float newvalue)
    {
        GlobalControl.control.SoundVol = newvalue;
    }
}
