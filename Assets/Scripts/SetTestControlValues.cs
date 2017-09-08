using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This runs and start and sets the initial values of the test controls, depending on the saved last game settings
/// This will require a new save/load feature for this, and will be removed for release
/// So obviously this will need to be confined to the Test Control Area
/// </summary>


public class SetTestControlValues : MonoBehaviour
{
    Text MoneyText;
    GameObject MoneyInput;
    Text KeyHintText;
    Text RepairText;
    Text TimeExtText;
    Text RevealText;
    Text DefuseText;
    Toggle SeedTog;
    Text SeedText;
    GameObject SeedInput;
    Toggle TimerTog;
    Text TimerText;
    Text StartRuleText;
    Slider RulesPerLevelSlider;
    Text RulesPerLevelText;
    Slider LevelAmountSlider;
    Text LevelAmountText;
    List<GameObject> RuleSelectors;
    Text RuleDisplayText;
    Toggle Key1Tog;
    Toggle Key2Tog;
    Toggle Key3Tog;
    Toggle Key4Tog;
    Toggle Key5Tog;
    Slider GridExpSlider;
    Text GridExpText;
    Text InitColText;
    Text InitRowText;
    Slider AnswerAmountSlider;
    Text AnswerAmountText;

    // Use this for initialization
    void Start()
    {
        //FOr each UI Section
        // Find/Assign the UI Gizmose
        // Load the initial values from Global Control.
        // Set the Gizmo display
        PlayerClass player = GlobalControl.control.GetCurrentPlayer();

        MoneyText = this.transform.FindChild("Money Adjust/Money: $X").GetComponent<Text>();
        MoneyText.text = "Money: $" + player.GetMoney().ToString();

        MoneyInput = this.transform.FindChild("Money Adjust/Money Input Field").gameObject;
        MoneyInput.SetActive(false);

        KeyHintText = this.transform.FindChild("KeyHint Adjuster/Player has").GetComponent<Text>();
        KeyHintText.text = "Key Hints: " + player.GetKeyHints().ToString();

        RepairText = this.transform.FindChild("Repairs Adjuster/Player has").GetComponent<Text>();
        RepairText.text = "Repairs: " + player.GetRepairs().ToString();

        TimeExtText = this.transform.FindChild("Time Extension Adjuster/Player has").GetComponent<Text>();
        TimeExtText.text = "5 Sec Time Extensions: " + player.GetTimeExt().ToString();

        RevealText = this.transform.FindChild("Reveal Adjuster/Player has").GetComponent<Text>();
        RevealText.text = "Reveals: " + player.GetReveals().ToString();

        SeedTog = this.transform.FindChild("Seed Value Input/Seed Toggle").GetComponent<Toggle>();
        SeedTog.isOn = GlobalControl.control.seedUsed;

        SeedText = this.transform.FindChild("Seed Value Input/Current Seed").GetComponent<Text>();
        SeedText.text = GlobalControl.control.seedvalue;

        SeedInput = this.transform.FindChild("Seed Value Input/New Seed Input").gameObject;
        SeedInput.SetActive(false);

        TimerTog = this.transform.FindChild("Timer Controls/Buttons and Toggles/Timer Toggle").GetComponent<Toggle>();
        TimerTog.isOn = GlobalControl.control.Timer;

        TimerText = this.transform.FindChild("Timer Controls/Time Display").GetComponent<Text>();
        TimerText.text = "Seconds: " + GlobalControl.control.TimerValue.ToString();

        DefuseText = this.transform.FindChild("Diffuse Adjuster/Player has").GetComponent<Text>();
        DefuseText.text = "Defuses: " + GlobalControl.control.Defuses.ToString();

        StartRuleText = this.transform.FindChild("Start Rule Amount Adjuster/Player has").GetComponent<Text>();
        StartRuleText.text = GlobalControl.control.initialrules.ToString() + " Starting Rule(s)";

        RulesPerLevelSlider = this.transform.FindChild("Rules Per Level Adjuster/Slider").GetComponent<Slider>();
        RulesPerLevelSlider.value = GlobalControl.control.rulesperlevel;

        RulesPerLevelText = this.transform.FindChild("Rules Per Level Adjuster/Rate Description").GetComponent<Text>();
        RulesPerLevelText.text = GlobalControl.control.rulesperlevel.ToString() + " Rules per Level";

        LevelAmountSlider = this.transform.FindChild("Levels in Game Adjuster/Slider").GetComponent<Slider>();
        LevelAmountSlider.maxValue = GlobalControl.control.GetMaxLevels();
        LevelAmountSlider.value = GlobalControl.control.NumOfLevels;

        LevelAmountText = this.transform.FindChild("Levels in Game Adjuster/Rate Description").GetComponent<Text>();
        LevelAmountText.text = "This game has " + GlobalControl.control.NumOfLevels.ToString() + " level(s) to solve.";

        // Now comes the fun part, the RULES SELECTOR!
        // Go through the entire Rules database, and instantiate Rule Selector prefab for each one, and add it to 
        // the Rules Selector Container (and denoting if it is already selected.)
        Transform ruleContainer = this.transform.FindChild("Rule Container");

        RuleDisplayText = this.transform.FindChild("Rule Description").GetComponent<Text>();

        Key1Tog = this.transform.FindChild("Available Key Selector/Color 1 Select Panel/Toggle").GetComponent<Toggle>();
        Key1Tog.isOn = GlobalControl.control.Color1Key;
        Key2Tog = this.transform.FindChild("Available Key Selector/Color 2 Select Panel/Toggle").GetComponent<Toggle>();
        Key2Tog.isOn = GlobalControl.control.Color2Key;
        Key3Tog = this.transform.FindChild("Available Key Selector/Color 3 Select Panel/Toggle").GetComponent<Toggle>();
        Key3Tog.isOn = GlobalControl.control.Color3Key;
        Key4Tog = this.transform.FindChild("Available Key Selector/Color 4 Select Panel/Toggle").GetComponent<Toggle>();
        Key4Tog.isOn = GlobalControl.control.Color4Key;
        Key5Tog = this.transform.FindChild("Available Key Selector/Color 5 Select Panel/Toggle").GetComponent<Toggle>();
        Key5Tog.isOn = GlobalControl.control.Color5Key;

        RuleSelectors = new List<GameObject>();
 
        foreach (Rule rule in RuleDatabase.RDBcontrol.rules)
        {
            //Instantiate rule selector prefab
            GameObject ruleSelect = (GameObject)Instantiate(Resources.Load("Rule Toggle"));

            //Add it to the Rules Container
            ruleSelect.transform.SetParent(ruleContainer);

            // Add it to the Selectors List
            RuleSelectors.Add(ruleSelect);
        }

        GridExpSlider = this.transform.FindChild("Grid Expansion Rate/Slider").GetComponent<Slider>();
        GridExpSlider.value = GlobalControl.control.GridExpansionRate;

        InitColText = this.transform.FindChild("Initial Column Size/Initial Size").GetComponent<Text>(); 
        InitColText.text = "Initial Columns: " + GlobalControl.control.initialcolumnsize.ToString(); ;

        InitRowText = this.transform.FindChild("Initial Row Size/Initial Size").GetComponent<Text>();
        InitRowText.text = "Initial Rows: " + GlobalControl.control.initialrowsize.ToString(); ;

        AnswerAmountSlider = this.transform.FindChild("Answer Increase Rate/Slider").GetComponent<Slider>();
        AnswerAmountSlider.value = GlobalControl.control.AnswerIncreaseRate;

        AnswerAmountText = this.transform.FindChild("Answer Increase Rate/Rate Description").GetComponent<Text>();
        AnswerAmountText.text = AIRText(GlobalControl.control.AnswerIncreaseRate);
    }

    ////////////////   UI Functions    ////////////////////////////////
    //   All the code to make the UI gizmos work properly       //
    ////////////////////////////////////////////////////////////////
    public void MoneyChangeButton()
    {
        MoneyInput.SetActive(true);
    }

    public void MoneyEntered()
    {
        string value = this.transform.FindChild("Money Adjust/Money Input Field/Text").GetComponent<Text>().text;

        // See if it's numeric
        if (IsAllNumeric(value))
        {
            // Convert to Int
            int num = int.Parse(value);
            // Change player's current money
            GlobalControl.control.GetCurrentPlayer().SetMoney(num);
            // Change number in Test Control Display
            MoneyText.text = "Money: $" + GlobalControl.control.GetCurrentPlayer().GetMoney().ToString();
        }

        MoneyInput.SetActive(false);
    }

    bool IsAllNumeric(string S)
    {
        foreach (char c in S)
        {
            if (!char.IsDigit(c)) return false;
        }

        return true;
    }

    public void IncDecKeyHint(bool val)
    {
        PlayerClass p = GlobalControl.control.GetCurrentPlayer();
        if (val) p.AddKeyHint();
        else if (p.GetKeyHints() > 0) p.SubtractKeyHint();

        KeyHintText.text = "Key Hints: " + p.GetKeyHints().ToString();
    }

    public void IncDecRepairs(bool val)
    {
        PlayerClass p = GlobalControl.control.GetCurrentPlayer();
        if (val) p.AddRepair();
        else if (p.GetRepairs() > 0) p.SubtractRepair();

        RepairText.text = "Repairs: " + p.GetRepairs().ToString();
    }

    public void IncDecTimeExt(bool val)
    {
        PlayerClass p = GlobalControl.control.GetCurrentPlayer();
        if (val) p.AddTimeExt();
        else if (p.GetTimeExt() > 0) p.SubtractTimeExt();

       TimeExtText.text = "5 Sec Time Extensions: " + p.GetTimeExt().ToString();
    }

    public void IncDecReveals(bool val)
    {
        PlayerClass p = GlobalControl.control.GetCurrentPlayer();
        if (val) p.AddReveal();
        else if (p.GetReveals() > 0) p.SubtractReveal();

       RevealText.text = "Reveals: " + p.GetReveals().ToString();
    }

    public void SeedToggle()
    {
        GlobalControl.control.seedUsed = SeedTog.isOn;
    }

    public void SeedChange()
    {
        SeedInput.SetActive(true);
    }

    public void SeedEntered()
    {
        string seed = SeedInput.transform.FindChild("Text").GetComponent<Text>().text;
        if (seed != "") GlobalControl.control.seedvalue = seed;
        SeedText.text = seed;
        SeedInput.SetActive(false);
    }

    public void TimerToggle()
    {
        GlobalControl.control.Timer = TimerTog.isOn;
    }

    public void IncDec10Sec(bool val)
    {
        int time = GlobalControl.control.TimerValue;
        if (val) GlobalControl.control.TimerValue += 10;
        else if (time >= 10) GlobalControl.control.TimerValue -= 10;

        TimerText.text = "Seconds: " + GlobalControl.control.TimerValue.ToString();
    }

    public void IncDec1Sec(bool val)
    {
        int time = GlobalControl.control.TimerValue;
        if (val) GlobalControl.control.TimerValue++;
        else if (time > 0) GlobalControl.control.TimerValue--;

        TimerText.text = "Seconds: " + GlobalControl.control.TimerValue.ToString();
    }

    public void IncDecDiffuses(bool val)
    {
        int num = GlobalControl.control.Defuses;
        if (val) GlobalControl.control.Defuses++;
        else if (num > 0) GlobalControl.control.Defuses--;

        DefuseText.text = "Defuses: " + GlobalControl.control.Defuses.ToString();
    }

    public void IncDecStartRules(bool val)
    {
        int max = RuleDatabase.RDBcontrol.GetMaxRules();
        int num = GlobalControl.control.initialrules;

        if ((val) && (num < (max - 1))) GlobalControl.control.initialrules++;
        else if ((!val) && (num > 1)) GlobalControl.control.initialrules--;

        StartRuleText.text = GlobalControl.control.initialrules.ToString() + " Starting Rule(s)";
        AdjustMaxLevels();
    }


    public void RulesPerLevelValueChange()
    {
        int num = (int)RulesPerLevelSlider.value;
        GlobalControl.control.rulesperlevel = num;
        RulesPerLevelText = this.transform.FindChild("Rules Per Level Adjuster/Rate Description").GetComponent<Text>();
        RulesPerLevelText.text = num.ToString() + " Rules per Level";
        AdjustMaxLevels();
    }

    public void LevelsInGameValueChange()
    {
        int num = (int)LevelAmountSlider.value;
        GlobalControl.control.NumOfLevels = num;
        LevelAmountText = this.transform.FindChild("Levels in Game Adjuster/Rate Description").GetComponent<Text>();
        LevelAmountText.text = "This game has " + num.ToString() + " level(s) to solve.";
        AdjustMaxLevels();
    }

    void AdjustMaxLevels()
    {
        LevelAmountSlider = this.transform.FindChild("Levels in Game Adjuster/Slider").GetComponent<Slider>();
        // Adjust Max Levels 
        int max = GlobalControl.control.GetMaxLevels();
        int levels = GlobalControl.control.NumOfLevels;
        if (levels > max)
        {
            LevelAmountSlider.value = max;
            GlobalControl.control.NumOfLevels = max;
        }
        LevelAmountSlider.maxValue = max;        

        // Adjust Max Starting Rules
        max = RuleDatabase.RDBcontrol.GetMaxRules();
        int sRules = GlobalControl.control.initialrules;
        if (sRules > (max - 1)) GlobalControl.control.initialrules = max - 1;
        StartRuleText.text = GlobalControl.control.initialrules.ToString() + " Starting Rule(s)";

        // Adjust Max Rules Per Level
        int remainder = max - sRules;
        max = remainder / GlobalControl.control.NumOfLevels;
        if (remainder % GlobalControl.control.NumOfLevels != 0) max++;

        RulesPerLevelSlider.maxValue = max;
        if (GlobalControl.control.rulesperlevel > max)
        {
            GlobalControl.control.rulesperlevel = max;
            RulesPerLevelText.text = max.ToString() + " Rules per Level";
        }

    }

    public void IncDecInitCol(bool val)
    {
        int num = GlobalControl.control.initialcolumnsize;
        if (val) GlobalControl.control.initialcolumnsize++;
        else if (num > 3) GlobalControl.control.initialcolumnsize--;
       
        InitColText.text = "Initial Columns: " + GlobalControl.control.initialcolumnsize.ToString();
    }

    public void IncDecInitRow(bool val)
    {
        int num = GlobalControl.control.initialrowsize;
        if (val) GlobalControl.control.initialrowsize++;
        else if (num > 3) GlobalControl.control.initialrowsize--;

        InitRowText.text = "Initial Rows: " + GlobalControl.control.initialrowsize.ToString();
    }


    // This fills in the information in List of RuleSelectors, it is run by pushing
    // the "Test Control Center". This allows them to be instantiated, and given some time
    // before being filled in all the way.
    public void initializeSelectors()
    {
        int i = 0;
        foreach (GameObject selector in RuleSelectors)
        {
            Rule rule = RuleDatabase.RDBcontrol.rules[i++];
            string newName = "Rule" + rule.ruleID.ToString();
            bool isSet = GlobalControl.control.selectedRules.Contains(rule.ruleID);
            RuleDescriptor RD = selector.GetComponent<RuleDescriptor>();
            RD.gameObject.name = newName;
            RD.SetInfo(RuleDisplayText, isSet, rule.ruleDesc, rule.ruleID, this);
        }

        ColorSelected();
    }

    // WHen a Color is chosen, this will ghost rules and unghost them
    void ColorSelected()
    {
        // Grab list of the allowable Rules
        List<int> KeyRules = RuleDatabase.RDBcontrol.GetColorRules();

        if (RuleSelectors == null) print("RuleSelectors not found!");
        foreach (GameObject selector in RuleSelectors)
        {
            RuleDescriptor RD = selector.GetComponent<RuleDescriptor>();
            if (!KeyRules.Contains(RD.ID)) RD.GhostSet(true);
            else RD.GhostSet(false);
        }
    }


    public void C1Toggle()
    {
        GlobalControl.control.Color1Key = Key1Tog.isOn;
        if (RuleSelectors != null) ColorSelected();
        AdjustMaxLevels();
    }

    public void C2Toggle()
    {
        GlobalControl.control.Color2Key = Key2Tog.isOn;
        if (RuleSelectors != null) ColorSelected();
        AdjustMaxLevels();
    }

    public void C3Toggle()
    {
        GlobalControl.control.Color3Key = Key3Tog.isOn;
        if (RuleSelectors != null) ColorSelected();
        AdjustMaxLevels();
    }

    public void C4Toggle()
    {
        GlobalControl.control.Color4Key = Key4Tog.isOn;
        if (RuleSelectors != null) ColorSelected();
        AdjustMaxLevels();
    }

    public void C5Toggle()
    {
        GlobalControl.control.Color5Key = Key5Tog.isOn;
        if (RuleSelectors != null) ColorSelected();
        AdjustMaxLevels();
    }

    public void GERSliderValueChange()
    {
        int num = (int) GridExpSlider.value;
        GlobalControl.control.GridExpansionRate = num;
        this.transform.FindChild("Grid Expansion Rate/Rate Description").GetComponent<Text>().text = GERText(num);
    }

    public void IncDecStartColumns(bool val)
    {
        int num = GlobalControl.control.initialcolumnsize;
        if (val) GlobalControl.control.initialcolumnsize++;
        else if (num > 2) GlobalControl.control.initialcolumnsize--;

        InitColText.text = "Starting Columns: " + GlobalControl.control.initialcolumnsize.ToString();
    }

    public void IncDecStartRows(bool val)
    {
        int num = GlobalControl.control.initialrowsize;
        if (val) GlobalControl.control.initialrowsize++;
        else if (num > 2) GlobalControl.control.initialrowsize--;

        InitRowText.text = "Starting Rows: " + GlobalControl.control.initialrowsize.ToString();
    }

    public void AIRSliderValueChange()
    {
        int num = (int)AnswerAmountSlider.value;
        GlobalControl.control.AnswerIncreaseRate = num;
        this.transform.FindChild("Answer Increase Rate/Rate Description").GetComponent<Text>().text = AIRText(num);
    }

    public void RuleSelected(int ruleNum)
    {
        // This is run by the TOggle switch
        List<int> tList = GlobalControl.control.selectedRules;

        //Make temporary list
        List<int> AllRules = new List<int>();

        //Add the rule to the list
        AllRules.Add(ruleNum);
        
        if (!tList.Contains(ruleNum))
        {
            // Get all the necessary parent rules on the list
            AllRules.AddRange(RuleDatabase.RDBcontrol.GetParentRules(tList, ruleNum));

            //For each of the rules in the list
            foreach (int num in AllRules)
            {
                print("Adding " + num.ToString());

                //Find the rules selector
                string path = "Rule Container/Rule" + num.ToString() + "/Toggle";

                //Find if the selector has already been set
                Toggle t = this.transform.FindChild(path).GetComponent<Toggle>();

                if (t.isOn != true) this.transform.FindChild(path).GetComponent<Toggle>().isOn = true;

                if (!GlobalControl.control.selectedRules.Contains(num)) GlobalControl.control.selectedRules.Add(num);                             
            }
        }
        else // The rule was already in the list, so it needs to be removed
        {
            // Get the rule and all the rules that require that rule
            AllRules.AddRange(RuleDatabase.RDBcontrol.GetDependents(tList, ruleNum));
            foreach (int num in AllRules)
            {
                //See if they are in the List
                if (GlobalControl.control.selectedRules.Contains(num))
                {
                    //Find their toggle 
                    string path = "Rule Container/Rule" + num.ToString() + "/Toggle";

                    //Set the Toggle to false
                    this.transform.FindChild(path).GetComponent<Toggle>().isOn = false;

                    //Remove them from the list
                    GlobalControl.control.selectedRules.Remove(num);
                }
                
            }
        }
    }

    // This just fills in a description of what a slider setting means
    public string GERText(int val)
    {
        switch (val)
        {
            case 0: return "No Expansion";
            case 1: return "Add One Column or Row every other level";
            case 2: return "Add One Column or Row every level";
            case 3: return "Add a Column and a Row every level";
            case 4: return "Add two Columns and Rows every level";
            case 5: return "Add three Columns and Rows every level";
            default: return "The GER value doesn't make sense";
        }
    }

    // This just fills in a description of what a slider setting means
    public string AIRText(int val)
    {
        switch (val)
        {
            case 0: return "No Increase";
            case 1: return "Answer locks increased by one every four levels";
            case 2: return "Answer locks increased by one every three levels";
            case 3: return "Answer locks increased by one every other level";
            case 4: return "Additional answer lock every level";
            case 5: return "Two additional answer locks every level";
            default: return "The Answer Increase Rate value doesn't make sense";
        }
    }
}


