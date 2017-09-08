using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/* *************************************************
 * The point of this script is to provide all the data for the level
 * as well as link to the next level. This keeps the levels sorted,
 * and should make it easier for me to ensure that it is correct.
 * 
 * Contains Rule, Wall Colors, Square Array, Winning move 
 * and next Level choices.
 * **************************************************/

[System.Serializable]
public class Level
{
    public int levNumRules; // How many rules in this level
    public List<RuleData> levRuleData;
	public int levCol; // Number of Columns for this level
	public int levRow; // Number of Rows for this level
    public List<WallList> levWalls; // List of Walls for each level
    public List <AnswerKey> levAnswerKey;  //LIST of Answer keys {Grid space, Key Color}
    public List <GridSpace> levGameGrid; //List Array of GridSpaces

    public class RuleData
    {
        public int ruleID;
        public string ruleText;

        public RuleData(int id, string text)
        {
            ruleID = id;
            ruleText = text;
        }
    }


	// Constructor without the square array (working on it)
	public Level(int numr, List <RuleData> ruleData, int col, int row, List <WallList> lw, List <AnswerKey> answerkey, List <GridSpace> gg)
	{
        levNumRules = numr;
        levRuleData = ruleData;
 		levCol = col;
		levRow = row;
        levWalls = lw;
        levAnswerKey = answerkey;
        levGameGrid = gg;		
	}


	public Level()	//THe Null Class constructor
	{

	}

    public bool LevelClearCheck()
    {
        foreach (AnswerKey ak in levAnswerKey)
        { 
            if (ak.solved == false) return false;
        }
        return true;
    }
}
