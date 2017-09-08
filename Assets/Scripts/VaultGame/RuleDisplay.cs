using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


/*
Locate this in the "Rules Container Panel"

Contains the list of the UI Rule GameObjects

The Rule Update Functions are triggered by the Game UI Controller
  ○ After game creation
  ○ At each level increase

This controls the activations of each rule UI, triggers the setting and text change function and orders them in the panel by index in the Container.
*/

public class RuleDisplay : MonoBehaviour 
{
    int NoOfRules;
    Transform Container;
    string Col1Name;
    string Col2Name;
    string Col3Name;
    string Col4Name;
    string Col5Name;

    void Start()
    {
        Container = this.transform.FindChild("Scroll View/Viewport/Content");
        foreach (Transform t in Container)
        {
            t.gameObject.SetActive(false);
        }
        NoOfRules = 0;
    }

    public void SetRuleList(List<Level.RuleData> ruleList)
    {
        Col1Name = Colorizer(GameCreator03.Color1);

        foreach (Level.RuleData ruleData in ruleList)
        {
            Transform t = Container.FindChild("Rule" + ruleData.ruleID.ToString());
            if (t == null) print("Couldn't find RuleBox for Rule" + ruleData.ruleID.ToString());
            else
            {
                t.gameObject.SetActive(true);
                string temp = ConvertString(ruleData.ruleText);

                t.gameObject.GetComponent<RuleColorController>().SetRuleDisplay(++NoOfRules, temp);
                t.gameObject.transform.SetAsFirstSibling();
            }            
        }
    }

    string ConvertString(string text)
    {
        text = text.Replace("Color1", Colorizer(GameCreator03.Color1));
        text = text.Replace("Color2", Colorizer(GameCreator03.Color2));
        text = text.Replace("Color3", Colorizer(GameCreator03.Color3));
        text = text.Replace("Color4", Colorizer(GameCreator03.Color4));
        text = text.Replace("Color5", Colorizer(GameCreator03.Color5));
        text = FirstLetterToUpper(text);
        return text;
    }

    string Colorizer(Color colour)
    {
        if (colour == Color.black) return "black";
        if (colour == Color.blue) return "blue";
        if (colour == Color.green) return "green";
        if (colour == Color.red) return "red";
        if (colour == Color.yellow) return "yellow";
        return "white";
    }

    // This function merely capitalizes the first letter of a string. (I am surprised this doesn't just exist already.)
    string FirstLetterToUpper(string str)
    {
        if (str == null)
            return null;
        if (str.Length > 1)
            return char.ToUpper(str[0]) + str.Substring(1);
        return str.ToUpper();
    }


}


