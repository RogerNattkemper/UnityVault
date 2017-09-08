using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RuleDescriptor : MonoBehaviour, IPointerEnterHandler {
    public string Description;
    public int ID;
    Text RuleText;
    SetTestControlValues STCV;
    private bool isGhost;

    // Use this for initialization
    public void SetInfo(Text textObj, bool set, string desc, int id, SetTestControlValues st)
    {
        this.transform.FindChild("Toggle").GetComponent<Toggle>().isOn = set;
        this.transform.FindChild("Rule Number").GetComponent<Text>().text = id.ToString();
        RuleText = textObj;
        Description = desc;
        ID = id;
        STCV = st;
        GhostSet(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {        
        RuleText.text = Description;
    }

    public void RuleSelectorToggled()
    {
        // This will add the rule selected to the "Selected Rule" 
        // and send a message to SetTestControlValues that this rule number has been flipped
        if (isGhost) this.transform.FindChild("Toggle").GetComponent<Toggle>().isOn = false;
        if ((STCV != null) && (!isGhost)) STCV.RuleSelected(ID);        
    }

    public void GhostSet(bool set)
    {
        Text RuleT = this.transform.FindChild("Rule Number").GetComponent<Text>();
        isGhost = set;

        // If this is set to ghost, remove this rule from the selected ruls
        // and set the text to gray
        if (isGhost)
        {
            if (GlobalControl.control.selectedRules.Contains(ID)) STCV.RuleSelected(ID);                           
            RuleT.color = Color.gray;
        }
        else RuleT.color = Color.black;
               
    }


}
