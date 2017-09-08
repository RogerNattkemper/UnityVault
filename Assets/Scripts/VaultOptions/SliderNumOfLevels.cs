using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderNumOfLevels : MonoBehaviour {

    Slider slider;
    public int MaxLevelsForVault;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = (float)GlobalControl.control.NumOfLevels;

        MaxLevelsForVault = CalculateMaxLevelsForVault(GlobalControl.control.initialrules, GlobalControl.control.rulesperlevel);
        
        slider.maxValue = (float) MaxLevelsForVault;        
    }

    public void UpdateNumOfLevels(float newvalue)
    {
        GlobalControl.control.NumOfLevels = Mathf.RoundToInt(newvalue);
    }

    public void SRCalculateMaxLevels(float NewSR)
    {
        MaxLevelsForVault = CalculateMaxLevelsForVault(Mathf.RoundToInt(NewSR), GlobalControl.control.rulesperlevel);
        slider.maxValue = (float) MaxLevelsForVault;

        if (GlobalControl.control.NumOfLevels > MaxLevelsForVault)
        {
            GlobalControl.control.NumOfLevels = MaxLevelsForVault;
        }
    }

    public void RPLCalculateMaxLevels(float NewRPL)
    {

        MaxLevelsForVault = CalculateMaxLevelsForVault(GlobalControl.control.initialrules, Mathf.RoundToInt(NewRPL));
        slider.maxValue = (float)MaxLevelsForVault;

        if (GlobalControl.control.NumOfLevels > MaxLevelsForVault)
        {
            GlobalControl.control.NumOfLevels = MaxLevelsForVault;
        }
    }

    int CalculateMaxLevelsForVault(int StartRules, int RulesPerLvl)
    {
        return ((RuleDatabase.MaxRules - StartRules) / RulesPerLvl) + 1;
    }
}
