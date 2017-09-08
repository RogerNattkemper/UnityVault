using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleColorController : MonoBehaviour
{

    public void SetRuleDisplay(int num, string ruletext)
    {
        Transform ImagePanel = this.transform.FindChild("Image Panel/Color Holder");
        if (ImagePanel == null) print("On " + this.transform.name + " Color Holder not found");
        

        // First get all the images in the prefab
        Transform tempImageT = ImagePanel.FindChild("Color1");
        if (tempImageT != null) tempImageT.GetComponent<Image>().color = GameCreator03.Color1;

        tempImageT = ImagePanel.FindChild("Color2");
        if (tempImageT != null) tempImageT.GetComponent<Image>().color = GameCreator03.Color2;

        tempImageT = ImagePanel.FindChild("Color3");
        if (tempImageT != null) tempImageT.GetComponent<Image>().color = GameCreator03.Color3;

        tempImageT = ImagePanel.FindChild("Color4");
        if (tempImageT != null) tempImageT.GetComponent<Image>().color = GameCreator03.Color4;

        tempImageT = ImagePanel.FindChild("Color5");
        if (tempImageT != null) tempImageT.GetComponent<Image>().color = GameCreator03.Color5;

        this.transform.FindChild("Rule Text").GetComponent<Text>().text = ruletext;

        this.transform.FindChild("Rule Number").GetComponent<Text>().text = num.ToString();
    }
}
