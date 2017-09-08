using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderStartingRules : MonoBehaviour
{
    Slider slider;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = (float)GlobalControl.control.initialrules;
    }

    public void UpdateInitialRuleNum(float newvalue)
    {
        GlobalControl.control.initialrules = Mathf.RoundToInt(newvalue);
    }

}
