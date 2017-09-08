using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderRPL : MonoBehaviour {
   
    Slider slider;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = (float)GlobalControl.control.rulesperlevel;
    }

    public void UpdateRulesPerLevel(float newvalue)
    {
        GlobalControl.control.rulesperlevel = Mathf.RoundToInt(newvalue);
    }
}
