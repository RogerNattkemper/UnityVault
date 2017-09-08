using UnityEngine;
using System.Collections;


/* *************************************************
 * The point of this script is to provide all the data for the rule
 * The minimum level, a pointer to it's rule checker.
 * **************************************************/

[System.Serializable]
public class Rule
{
	public int ruleID; // The number of the level
	public string ruleDesc;  // The rule itself
	public int ruleMin; // The minimum level this rule can be implemented
    public int absoRule; // If this rule MUST be added at a certain level (non-Zero)
	// Whatever kind of variables I can pass to the rule checker should be here too
	
	
	public Rule(int id, string rule, int min, int abso)
	{
		ruleID = id;
		ruleDesc = rule;
		ruleMin = min;
        absoRule = abso;
	}
	
	public Rule()	//THe Null Class constructor
	{
		
	} 
	
	
}