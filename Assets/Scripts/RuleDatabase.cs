using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
  Rule Class
  {
  int ruleID; // The number of the Rule
  string ruleDesc;  // The rule itself
  int ruleMin; // The minimum level this rule can be implemented
	-- Whatever kind of variables I can pass to the rule checker should be here too
   }
*/
public class RuleDatabase : MonoBehaviour 
{
    public static RuleDatabase RDBcontrol;
    public static int MaxRules;
	public List<Rule> rules = new List<Rule>();
	// Use this for initialization

	// Rule Key
	void Awake ()
	{
        if (RDBcontrol == null)
        {
            DontDestroyOnLoad(gameObject);
            RDBcontrol = this;
        }
        else if (RDBcontrol != this)
        {
            print("RDB Destroyed");
            Destroy(gameObject);
        }
        
        // ID Key:  Rule List Number / Wall Color Effected / Second Color Effected / Primary Color Effected 
	    rules.Add(new Rule(1000, "In general, match a key with the same colored lock.", 0, 0));
        rules.Add(new Rule(1021, "Color1 keys now go to Color2 locks and Color2 keys go into Color1 locks.", 1, 0));
        rules.Add(new Rule(1045, "Color4 keys now go to Color5 locks and Color5 keys go into Color4 locks.", 1, 0));
        rules.Add(new Rule(1111, "Color1 locks within 2 spaces of a Color1 wall use Color1 keys, despite other rules.", 1, 0));
        rules.Add(new Rule(5555, "Color5 locks within 2 spaces of a Color5 wall use Color5 keys, despite other rules.", 1, 0));

        //___________________________
        rules.Add(new Rule(1001, "Color1 locks in the center row or center column are bombs. ", 2, 0));
        rules.Add(new Rule(2001, "Color1 locks against the sides of the grid are all bombs if the exact center lock is Color1.", 2, 0));
        rules.Add(new Rule(3401, "Color1 locks are bombs if a Color4 wall is on the same half of the grid.", 3, 0));
        rules.Add(new Rule(4031, "Color1 locks on the same row as a Color3 lock are bombs.", 2, 0));
        rules.Add(new Rule(5011, "Color1 locks are all bombs if all corners are Color1 locks.", 2, 0));
        //___________________________


        rules.Add(new Rule(1502, "Color2 locks on the side of the grid opposite of a Color5 wall are bombs.", 2, 0));
        rules.Add(new Rule(2052, "Color2 locks next to Color5 locks are bombs.", 2, 0));
        rules.Add(new Rule(3002, "Color2 locks next to a blank space are bombs.", 2, 0));
        rules.Add(new Rule(4202, "Color2 locks next to a Color2 wall are bombs.", 2, 0));
        rules.Add(new Rule(5052, "Color2 locks between two Color5 locks are a solution, despite any other rule!", 2, 0));
        //___________________________

        rules.Add(new Rule(1003, "Color3 locks are all bombs.", 2, 0));
        rules.Add(new Rule(2003, "Color3 bomb exception: Color3 locks in the corner are solutions!", 2, 0));
        rules.Add(new Rule(3033, "Color3 bomb exception: Color3 locks where all four adjacent sides are Color3 locks are solutions!", 2, 0));
        rules.Add(new Rule(4003, "Color3 bomb Exception: A Color3 lock in the exact center of the grid is an answer!", 2, 0));
        rules.Add(new Rule(5203, "Color3 bomb exception: Color3 locks next to a Color2 wall are solutions!", 2, 0));
        //___________________________

        rules.Add(new Rule(1004, "Color4 locks in a corner are bombs.", 2, 0));
        rules.Add(new Rule(2404, "Color4 locks on the opposite half of the grid from a Color4 wall are bombs.", 2, 0));
        rules.Add(new Rule(3014, "Color4 locks next to a Color1 lock are bombs.", 2, 0));
        rules.Add(new Rule(4034, "Color4 locks on the same row or column as a Color3 lock are bombs.", 2, 0));
        rules.Add(new Rule(5004, "Color4 locks on the same half of the grid as a Color1 wall are bombs.", 2, 0));

        //___________________________
        rules.Add(new Rule(1025, "Color5 locks next to a Color2 lock is a bomb.", 2, 0));
        rules.Add(new Rule(2405, "Color5 locks on the same half of the grid as a Color4 wall are bombs.", 2, 0));
        rules.Add(new Rule(3005, "Color5 locks away from the sides of the grid are bombs if the exact center lock of the grid is Color5.", 2, 0));
        rules.Add(new Rule(4205, "Color5 locks against the side of the grid opposite from a Color2 wall are bombs.", 2, 0));
        rules.Add(new Rule(5035, "Color5 locks in the same column as a Color3 lock are bombs.", 2, 0));

        //___________________________


        MaxRules = rules.Count;
	}

    // This returns a single parent for a rule that needs it to be in place first
    public int GetParent(List<int> targetList, int id)
    {        
        if (!targetList.Contains(1000)) return 1000;

        switch (id)
        {
            case 1111: 
                {
                    if (!targetList.Contains(1021)) return 1021;
                    break;
                }
            case 5555:
                {
                    if (!targetList.Contains(1045)) return 1045;
                    break;
                }
            case 5052:
                {
                    if (!targetList.Contains(2052)) return 2052;
                    else if (!targetList.Contains(1025)) return 1025;
                    break;
                }
            case 2003:
            case 3033:
            case 4003:
            case 5203:
                {
                    if (!targetList.Contains(1003)) return 1003;
                    break;
                }
            default: break;
        }

        return id;
    }

    // This returns a list of rules that need to be in place for the id to work
    public List<int> GetParentRules(List<int> targetList, int id)
    {
            List<int> tempList = new List<int>();

            if (!targetList.Contains(1000)) tempList.Add(1000);

            switch (id)
            {
                case 1111:
                    {
                        if (!targetList.Contains(1021)) tempList.Add(1021);
                        break;
                    }
                case 5555:
                    {
                        if (!targetList.Contains(1045)) tempList.Add(1045);
                        break;
                    }
                case 5052:
                    {
                        if (!targetList.Contains(2052)) tempList.Add(2052);
                        if (!targetList.Contains(1025)) tempList.Add(1025);
                        break;
                    }
                case 2003:
                case 3033:
                case 4003:
                case 5203:
                    {
                        if (!targetList.Contains(1003)) tempList.Add(1003);
                        break;
                    }
                default: break;
            }
        foreach (int num in tempList) print(num.ToString());

        return tempList;        
    }

    // This returns the Rules that are reliant on the id rule to be in place
    public List<int> GetDependents(List<int> targetList, int id)
    {
        List<int> tempList = new List<int>();

        switch (id)
        {
            case 1000:
                {
                    foreach (Rule rule in rules) tempList.Add(rule.ruleID);
                    tempList.Remove(1000);
                    break;
                }
            case 1021:
                {
                    tempList.Add(1111);
                    break;
                }
            case 1045:
                {
                    tempList.Add(5555);
                    break;
                }
            case 2052:
                {
                    tempList.Add(5052);
                    break;
                }
            case 1025:
                {
                    tempList.Add(5052);
                    break;
                }
            case 1003:
                {
                    tempList.Add(2003);
                    tempList.Add(3033);
                    tempList.Add(4003);
                    tempList.Add(5203);
                    break;
                }
            default: break;
        }

        return tempList;
    }

    // This will return all the rules that are allowed by the currently selected Key Colors
    public List<int> GetColorRules()
    {
        List<int> tempList = new List<int>();
        bool C1 = GlobalControl.control.Color1Key;
        bool C2 = GlobalControl.control.Color2Key;
        bool C3 = GlobalControl.control.Color3Key;
        bool C4 = GlobalControl.control.Color4Key;
        bool C5 = GlobalControl.control.Color5Key;

        if (C1 || C2 || C3 || C4 || C5) tempList.Add(1000);

        if (C1)
        {
            tempList.Add(1001);
            tempList.Add(2001);
            tempList.Add(3401);
            tempList.Add(5011);
        }

        if (C2)
        {
            tempList.Add(1502);
            tempList.Add(3002);
            tempList.Add(4202);
        }

        if (C3)
        {
            tempList.Add(1003);
            tempList.Add(2003);
            tempList.Add(3033);
            tempList.Add(4003);
            tempList.Add(5203);
        }

        if (C4)
        {
            tempList.Add(1004);
            tempList.Add(2404);
            tempList.Add(5004);
        }

        if (C5)
        {
            tempList.Add(2405);
            tempList.Add(3005);
            tempList.Add(4205);
        }

        if (C1 && C2)
        {
            tempList.Add(1021);
            tempList.Add(1111);
        }

        if (C1 && C3) tempList.Add(4031);

        if (C1 && C4) tempList.Add(3014);

        if (C2 && C5) 
        {
            tempList.Add(2052);
            tempList.Add(1025);
            tempList.Add(5052);
        }

        if (C3 && C4) tempList.Add(4034);

        if (C3 && C5) tempList.Add(5035);

        if (C4 && C5)
        {
            tempList.Add(1045);
            tempList.Add(5555);
        }
        return tempList;
    }

    public int GetMaxRules()
    {
        List<int> curRules = GetColorRules();
        return curRules.Count;
    }

    public string GetRuleText(int id)
    {
        foreach (Rule rule in rules)
        {
            if (rule.ruleID == id) return rule.ruleDesc;
        }

        return null;
    }

}
