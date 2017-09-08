using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderInitGS : MonoBehaviour {

    Slider slider;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = (float)GlobalControl.control.initialrowsize;
    }

    public void UpdateInitialGridSize(float newvalue)
    {
        int tmp = Mathf.RoundToInt(newvalue);
        GlobalControl.control.initialrowsize = tmp;
        GlobalControl.control.initialcolumnsize = tmp;
    }

}
