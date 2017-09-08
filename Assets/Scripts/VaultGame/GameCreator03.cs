using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


/******************************************************************
 * Game plan
 * At Start, create the game rule list, all the way to MAX levels.
 * 	For EACH level
 * 	- Each rule is added, ensured it meets the minimum level reqs
 * 	- A grid is created and a random location and color is chosen
 * 			as the answer.
 * 	- Each OTHER location is selected, and one random rule from the 
 * 		list is applied to that location so that it fails the rule
 * 		while also ensuring it does not violate the answer space.
 * 			- THis continues until all other spaces are invalidated.
 * 
 * 	- The grid, walls and answer is then saved as a record, and stored in the 
 * 	- level Levellist.
 * 	- THis repeats until the list of levels meets the MAXIMUMLEVELs criteria.
 * 
 * During the play,
 * 	- Gamecontrol then creates the board and pegs. 
 * 	- When a peg is moved and put into an incorrect slot, a second rule 
 *          checking is done to note all the rules that the player 
 *          violated, and decrement the player's lives by one. 
 * 	- If the player runs out of lives, then the game is over. 
 *  - If the player places the peg in the correct slot, then the 
 *          game congratulates the player and the next level is 
 *          loaded.
 * 
 * You should be able to save a game eventually.
 * ***************************************************************/

public class GameCreator03 : MonoBehaviour 
{
    public int seedvalue; //<-------------------GLOBALS (Used for Treasure Maps)
    public int MAXLEVEL;  //<--------------GLOBALS
    public int numAnswers = 1;
    public int initialRules;  //<-------------------GLOBALS
    public int rulesPerLevel; //<-------------------GLOBALS
    public bool ShowStopper = false;   //<------------ GLOBALS
    public int initialRowSize;             //<----------------GLOBALS
    public int initialColumnSize;     //<---------------GLOBALS
    public int maxColumns;
    public int maxRows;
    public int GridExpansionRate;  //<--------------GLOBALS
    public bool Timer;              //<-------------GLOBALS
    public bool increaseGrid = false; // ONly used with G.E.R. 1 
        
	public Color[] RandColors = {Color.black, Color.blue, Color.yellow, Color.red, Color.green};
	public static Color Color1;
	public string Color1text;
	public static Color Color2;
	public string Color2text;
	public static Color Color3;
	public string Color3text;
	public static Color Color4;
	public string Color4text;
	public static Color Color5;
	public string Color5text;
	public static Color white = Color.white;
	public Color badcolor = Color.white;
	public Color black = Color.black;

	public RuleDatabase rdatabase;
	public LevelDatabase ldatabase;

    private const int UPPERWALL = 0;
    private const int RIGHTWALL = 1;
    private const int BOTTOMWALL = 2;
    private const int LEFTWALL = 3;

    // Answer Increase Rate
    /* A.I.R. 0: No Increase 
*  A.I.R. 1: Additional answer every 4 levels
*  A.I.R. 2: Additional answer every 3 levels
*  A.I.R. 3: Additional answer every 2 levels
*  A.I.R. 4: Additional answer every level
*  A.I.R. 5: Two additional answers every level*/
    private int AnswerIncreaseRate;

    // Time Calculation for the Vault = ((Answers * timePerAnswer) + (ruleMultiplier * number of rules)) per level;
    public float TimeAllowed = 0;
    public const float timePerAnswer = 10.0f; // Time allowed per answer in seconds
    public const float ruleMultiplier = 0.1f; // Extra time per rule
    public const float gridSizeMuliplier = 0.05f; // Time allowance for grid size  
     

    public List<GridSpace> gamegrid;
    public List<AnswerKey> answerKey;
    public List<WallList> wallList;
		
	public bool r1000, r1021, r1045, r1111, r5555 = false; // Color Rules
	public bool r1001, r2001, r3401, r4031, r5011 = false; // Color1 lock rules
	public bool r1502, r2052, r3002, r4202, r5052 = false; // Color2 lock rules
	public bool r1003, r2003, r3033, r4003, r5203 = false; // Color3 lock rules
	public bool r1004, r2404, r3014, r4034, r5004 = false; // Color4 lock rules
	public bool r1025, r2405, r3005, r4205, r5035 = false; // Color5 lock rules
  	
	void Start () 
	{

        // If the Sein GlobalControl not set, grab a random one
        int rando = 0;
        if ((GlobalControl.control.seedUsed) && (GlobalControl.control.seedvalue != ""))
        {
            //Convert seed value to an int and assign rando to it
            string seedString = GlobalControl.control.seedvalue;
            seedString = Regex.Replace(seedString, "[^\\w\\._]", "");
            byte[] bArray = System.Convert.FromBase64String(seedString);
            for (int i = 0; i < bArray.Length; i++) rando += bArray[i];
            Random.InitState(rando);
        }
        else
            Random.InitState((int)System.DateTime.Now.Ticks);

        MAXLEVEL = GlobalControl.control.NumOfLevels;
        initialRules = GlobalControl.control.initialrules;
        rulesPerLevel = GlobalControl.control.rulesperlevel;
        initialRowSize = GlobalControl.control.initialrowsize;
        initialColumnSize = GlobalControl.control.initialcolumnsize;
        GridExpansionRate = GlobalControl.control.GridExpansionRate;
        AnswerIncreaseRate = GlobalControl.control.AnswerIncreaseRate;



        // If this Vault attempt is being timed, this calculates how much time is allowed
        if (Timer)
        {
            int answers = 1;
            int gridcol = initialColumnSize;
            int gridrow = initialRowSize;

            for (int i = 0; i < MAXLEVEL; i++)
            {
                float ruletime = 0;
                float answertime = 0;
                bool increase = false;

                ruletime = (initialRules + (i * rulesPerLevel)) * ruleMultiplier;

                switch (AnswerIncreaseRate)
                {
                    case 0: break;
                    case 1: //Every 4th level
                        {
                            if ((i + 1) % 4 == 0) answers++;
                            break;    
                        }
                    case 2: //Every 3rd level
                        {
                            if ((i + 1) % 3 == 0) answers++;
                            break;
                        }
                    case 3: // Every other level
                        {
                            if (i % 2 != 0) answers++;
                            break;
                        }
                    case 4: // Every level
                        {
                            answers++;
                            break;
                        }
                    case 5: // +2 answer per level
                        {
                            answers += 2;
                            break;
                        }
                    default: break;
                }
                answertime = answers * timePerAnswer;

                if (i > 0)
                {
                    switch (GridExpansionRate)
                    {
                        case 0: break; // No Increase
                        case 1: //Add column or row every other level
                            {
                                if (increase)
                                {
                                    if (gridcol > gridrow) gridrow++;
                                    else gridcol++;
                                }
                                increase = !increase;
                                break;
                            }
                        case 2: //Add column or row every level
                            {
                                if (gridcol > gridrow) gridrow++;
                                else gridcol++;
                                break;
                            }
                        case 3: //Add column or row every level
                        case 4: //Add 2 columns and rows every level
                        case 5: //Add 3 columns and row every level
                            {
                                gridcol += (GridExpansionRate - 2);
                                gridrow += (GridExpansionRate - 2);
                                break;
                            }
                        default: break;
                    }
                }
                TimeAllowed += answertime + ruletime + ((gridrow * gridcol) * gridSizeMuliplier);            
            }
        }

    rdatabase = GameObject.FindGameObjectWithTag("Rule Database").GetComponent<RuleDatabase>();
    ldatabase = GameObject.FindGameObjectWithTag("Level Database").GetComponent<LevelDatabase>();


//*******************************************************************************
// **************      Randomize Whole Game Elements Section      ***************
// ******************************************************************************
    int rand = Random.Range(0, RandColors.Length);
    Color temp;
    for (int i = 0; i < (RandColors.Length - 1); i++)  
    {
        temp = RandColors [i];
        RandColors [i] = RandColors [rand];
        RandColors [rand] = temp;
        rand = Random.Range (i, RandColors.Length);
    }

    Color1 = RandColors [0];
    Color1text = ColorTextulizer (Color1);
    Color2 = RandColors [1];
    Color2text = ColorTextulizer (Color2);
    Color3 = RandColors [2];
    Color3text = ColorTextulizer (Color3);
    Color4 = RandColors [3];
    Color4text = ColorTextulizer (Color4);
    Color5 = RandColors [4];
    Color5text = ColorTextulizer (Color5);
    ////////////////////////////////////////////////////////
    // Initialize the Level List //////////////////////////
    for (int i = 0; i < MAXLEVEL; i++)
    {
        ldatabase.levels.Add (new Level());
    }
    ////////////////////////////////////////////////////////

    FalsifyAllRules();

        // Randomize the rule list and Fill Rule ID array //////////////////////////
        /* This completely fills the ruleIDs for the game. */

        // This also must add the selected rules from Globals


        //***************************************************************************************
        //******************** Rule Randomizer Section! *****************************************
        //***************************************************************************************
        List<int> ruleIDarray = new List<int>();
        List<int> temprulelist = new List<int> ();
       
        // Get the number of allowed Rules
        int MaxRules = RuleDatabase.RDBcontrol.GetMaxRules();
        int rnd; // Random index
		int tmp; // Number at the index
        int par; // The Parent rule sent back from Checking dependencies

        //How many rules are needed for this game?  (You subtract the multis, to make room for them in the array)
        int numrules = MAXLEVEL * rulesPerLevel + initialRules;

        //If we are asking for more rules than actually exist, set to the max
        if (numrules > MaxRules) numrules = MaxRules;

        // Are there Selected Rules?
        int selCount = GlobalControl.control.selectedRules.Count;

        List<int> AllAllowedRules = RuleDatabase.RDBcontrol.GetColorRules();

        if (selCount > 0)
        {
            temprulelist.AddRange(GlobalControl.control.selectedRules);
            print("temprulelist is " + temprulelist.Count + " large");

            for (int i = 0; i < selCount; i++)
            {
                rnd = Random.Range(0, temprulelist.Count); // Grab a random index in the range
                tmp = temprulelist[rnd]; // Pull out the Rule ID at that index
                par = RuleDatabase.RDBcontrol.GetParent(ruleIDarray, tmp);

                // If there was a parent that needs to go first
                if (par != tmp) tmp = par;

                ruleIDarray.Add(tmp);  // Add it to the Rule ID Array
                temprulelist.Remove(tmp); 
                AllAllowedRules.Remove(tmp);
            }            
        }

        temprulelist = AllAllowedRules; // List of all rules left that can be added

        for (int i = 0; i < (numrules - selCount); i++) // For all rules in the game
        {
            rnd = Random.Range(0, temprulelist.Count); // Grab a random index in the range
            tmp = temprulelist[rnd]; // Pull out the Rule ID at that index
            par = RuleDatabase.RDBcontrol.GetParent(ruleIDarray, tmp);

            // If there was a parent that needs to go first
            if (par != tmp) tmp = par;     
            
            ruleIDarray.Add(tmp);  // Add it to the Rule ID Array
            temprulelist.Remove(tmp);
        }

        print("Rule ID Array");
        foreach (int i in ruleIDarray) print(i);

        // End of Commented out Rule Randomizer Section      

        ////////////////////////////////////////////////////////
        /************************    GAME LEVEL CREATOR LOOP    **********************************************
	 * The big game creator routine
	 * 
	 * This loop will take the randomized rule array and create the whole
	 * game.
	 * 1. Each loop is a level.
	 * 2. For each loop, generate and save the following and save them in the current gamearray iteration.		
	 * 3. Run all the appropriate rules in play against the answer spaces, ensuring that the answer spaces are valid.
	 * 4. Go through all the rest of the squares in the array, and run a random rule that is in play to invalidate it.
	 * 5. Save all of these into the levels array list
	 * 6. Done!
	 *********************************************************************************/


        int levCol =  initialColumnSize;
		int levRow = initialRowSize;
        int numSquares;
		string tempstring;
		int ruleindex = 0; // What number rule this is in game
        int attempt = 0;
        bool restart = false;		

		for (int lev = 0; lev < MAXLEVEL; lev++)
        { // The major level loop, each lev iteration represents a new level in the game.

            numSquares = levCol * levRow;
            print("<b>NOW CREATING LEVEL: " + lev + " ************************************************************************</b>");
            print("MAXLEVEL = " + MAXLEVEL);
            if (attempt > 0) print(attempt + " attempt");

            restart = false;

            //Get a random answer location, stick it into the level array
            answerKey = new List<AnswerKey> (); // Start with a fresh Answerlocation and Color list
            wallList = new List<WallList>();

            ldatabase.levels[lev].levCol = levCol;
            ldatabase.levels[lev].levRow = levRow;

            if (attempt == 0)
            {
                List<Level.RuleData> ruleList = new List<Level.RuleData>();
                //List<string> RuleTexts = new List<string>();
                //Get all the rules for the level
                int rulesInLevel = (lev == 0) ? initialRules : rulesPerLevel; // Number of rules needed for this level

                // This allows for levels  with less than the "rulesperlevel" at the end
                if (lev == (MAXLEVEL - 1))
                {
                    if (rulesPerLevel > (ruleIDarray.Count - ruleindex)) rulesInLevel = ruleIDarray.Count - ruleindex;            
                }

                rulesInLevel += ruleindex;
 
                // Go through all the new added rules, get their descriptions, and convert
                for (int i = ruleindex; i < rulesInLevel; i++)
                {
                    Rule rl = new Rule();
                    rl = GetRule(ruleIDarray[i]);
                    tempstring = rl.ruleDesc;
                    ruleindex++;

                    // Convert the new rule into understandable text, and stick it into the level database
                    tempstring = tempstring.Replace("Color1", Color1text);
                    tempstring = tempstring.Replace("Color2", Color2text);
                    tempstring = tempstring.Replace("Color3", Color3text);
                    tempstring = tempstring.Replace("Color4", Color4text);
                    tempstring = tempstring.Replace("Color5", Color5text);
                    tempstring = FirstLetterToUpper(tempstring);

                    Level.RuleData rd = new Level.RuleData(ruleIDarray[i], tempstring);
                    ruleList.Add(rd);

                    //RuleTexts.Add(tempstring);
                    //Activate new chosen rule    
                    print("Flipping rule: " + rl.ruleID);                 
                    FlipRuleBool(rl.ruleID);                      
                }

                //Check A.I.R. against current level, and increase number of answers accordingdly.

                //Increment Answers Needed every four levels
                if (AnswerIncreaseRate == 1) numAnswers = ((lev + 1) % 4 == 0) ? numAnswers + 1 : numAnswers;

                //Every 3rd level
                if (AnswerIncreaseRate == 2) numAnswers = ((lev + 1) % 3 == 0) ? numAnswers + 1 : numAnswers;

                // Every other level
                if (AnswerIncreaseRate == 3) numAnswers = ((lev + 1) % 2 == 0) ? numAnswers + 1 : numAnswers;

                // Every level
                if (AnswerIncreaseRate == 4) numAnswers++;

                // +2 answers per level
                if (AnswerIncreaseRate == 5) numAnswers += 2;

                //ANSWER NUMBER ERROR CHECK
                // There can never be more answers than half the amount of grid squares
                if (numAnswers >= (numSquares / 2)) numAnswers = numSquares / 2; 

                // Save the List of rules strings to the level database
                ldatabase.levels[lev].levRuleData = ruleList;
            }

             

            // Create and initialize the color control array (colorpermission) which helps keep the rules from tangling each other up.
            // This includes the wall colors which are found in the array at numsquares + wall offset: + 0 (top), + 1 (right), + 2 (bottom) and + 3 (left).
            // AnswerSpot is if the grid space is allowed to be an Answer             
            gamegrid = new List<GridSpace>();  // New Set up with List

            List<Color> initWallColorList = new List<Color>();
            initWallColorList.Add(white);
            initWallColorList.Add(black);
            initWallColorList.Add(Color.blue);
            initWallColorList.Add(Color.green);
            initWallColorList.Add(Color.red);
            initWallColorList.Add(Color.yellow);

            WallList wl;
            //Initialize the Wall List
            for (int j = 0; j < 4; j++)
            {                       
                wl = gameObject.AddComponent<WallList>();
                wl.placed = false;
                wl.colors = new List<Color>(initWallColorList);
                wallList.Add(wl);
            }

            List<Color> initColorList = new List<Color>();
            initColorList.Add(white);
            initColorList.Add(black);
            initColorList.Add(Color.blue);
            initColorList.Add(Color.green);
            initColorList.Add(Color.red);
            initColorList.Add(Color.yellow);

            if (!GlobalControl.control.Color1Key) initColorList.Remove(Color1);
            if (!GlobalControl.control.Color2Key) initColorList.Remove(Color2);
            if (!GlobalControl.control.Color3Key) initColorList.Remove(Color3);
            if (!GlobalControl.control.Color4Key) initColorList.Remove(Color4);
            if (!GlobalControl.control.Color5Key) initColorList.Remove(Color5);

            int column;
            int row;
            // Initialize the gamearray            
            GridSpace gstmp;
            for (int j = 0; j < numSquares; j++) 
            {
                row = j / levCol;
                column = j - (row * levCol);
                gstmp = gameObject.AddComponent<GridSpace>();
                gstmp.position = j;
                gstmp.column= column;
                gstmp.row = row;
                gstmp.answer = false;
                gstmp.placed = false;

                gstmp.colors = new List<Color>(initColorList);             
                gstmp.texture = "Lock Plate";
                gamegrid.Add(gstmp);
                
            }
            // Create Position Checker, to see if there are ANY valid answer spots
            List<bool> posDirty = new List<bool>();
            // Create a list of all the spaces, and randomize them
            List<int> randPosList = new List<int>();
            for (int i = 0; i < numSquares; i++)
            {
                randPosList.Add(i);
                posDirty.Add(false);
            }

            int n = randPosList.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                int value = randPosList[k];
                randPosList[k] = randPosList[n];
                randPosList[n] = value;
            }
          

            // *******************************************************************************
            // *************** Initialize AnswerKey and Locations ****************************
            AnswerKey iak;
            int answerloc = 0;
            for (int i = 0; i < numAnswers; i++)
            {   
               do // Get a random location in the grid
                {
                    answerloc = Random.Range(0, numSquares);                       
                } while (answerKey.Exists(x => x.position == answerloc)); //Ensure it is unique

               iak = gameObject.AddComponent<AnswerKey>();
               iak.position = answerloc;
               iak.placed = false;
               iak.colors = new List<Color>(initColorList);
               iak.solved = false;
               iak.RemoveColor(white);

                answerKey.Add(iak);
               if (gamegrid[answerloc].colors.Count != 6)
               {
                   print("In AnswerKey Initializer: The corresponding Game Grid space is initialized with " + gamegrid[answerloc].colors.Count + " colors");
                   print("numAnswer = " + numAnswers);
                   print("position = " + answerloc);
                   print("Gridspace colors available: " + iak.colors.Count);
               }                                 
            }

           
            // **********************************************************************************************
           //  *********************    ANSWER VALIDATOR LOOP   *********************************************
            // The Answer Validator will take the answer color and loc, ensure it works with the grid, 
            //		if not it will change colors and try again, or return white which indicates a problem
            // The AnswerValidator also changes the permissions and the wall colors as necessary.
            // The only array color it changes is the color at AnswerLoc
            //************************************************************************************************

           foreach (AnswerKey ak in answerKey)
           {
               Color keycolor;
               Color lockcolor = white;
               do
                {                
                    keycolor = ak.GetRandomAllowedColor(true);
                    print("Before AV"); // debug
                    lockcolor = AnswerValidator(ak.position, levCol, levRow, keycolor); // Test, if it's good get out of loop
                    print("After AV"); // debug

                    // If the answer lock is down to no choices, then find a new location at random.
                    if ((lockcolor == white) && (ak.colors.Count == 0))
                    {
                        print("Answer at " + ak.position + " ran out of choices, choosing a new location.");
                        int newloc;
                        do // Get a random location in the grid
                        {
                            newloc = Random.Range(0, numSquares);
                        } while (answerKey.Exists(x => x.position == newloc)); //Ensure it is unique (Checks to see if that newloc has been used)

                        //Give answerkey a new Color list and the new position
                        ak.colors = new List<Color>(initColorList);
                        ak.position = newloc;
                        print("New location is at " + ak.position);
                    }
               }
                while (lockcolor == white);
                ak.InjectColor(keycolor);
                ak.RemoveAllColorsExcept(keycolor); // Ensure nothing but the allowed color exists in the Answer key          
                print("AnswerColor at " + ak.position +" is " + ColorTextulizer(ak.GetColor()));
           }

            ldatabase.levels[lev].levAnswerKey = answerKey; //Put the AnswerKey List into the Level (It's finally baked)
            // So now the grid merely has some permissions changed to either narrow down, or restrict what colors are allowed

            Color wallcolor;
            foreach(WallList wal in wallList)
            {
                wallcolor = wal.GetRandomAllowedColor();
                wal.SetColor(wallcolor);
            }

            ldatabase.levels[lev].levWalls = wallList;

            // Go through each square, skipping the Answer squares, grab a color and see if it can be invalidated.
            // THis need to be rewritten entirely! Currently it grabs a random rule to invalidate, but instead it should get a random color.!!!!


            //**********************************************************************************************
            ///  ****************       GAME GRID CREATION LOOP           **********************************
            ///  ******************************************************************************************
            
            /* Create an int List, the size of the grid size, with all the pos */
            foreach(int num in randPosList)
            {
                bool pass = false;
                if (!gamegrid[num].placed)
                {
                    do
                    {
                        print("Before VSR"); //debug
                        pass = ValidatedSlotRule(gamegrid[num].position, levCol, levRow, lev);
                        print("After VSR"); //debug
                        if (!pass)
                        {
                            if (gamegrid[num].colors.Count > 0) print("Slot: " + gamegrid[num].position + " has " + gamegrid[num].colors.Count + " colors left.");
                            else
                            {
                                print("<b>ERROR! Slot: " + gamegrid[num].position + " ran out of colors!</b>");
                                print("Restart the grid!");
                                restart = true;
                                attempt++;
                                lev--;
                                break;
                            }
                        }
                            
                    } while (!pass);

                    if (restart) break;
                }
                print("LockColor at " + gamegrid[num].position + " is Color " + gamegrid[num].GetMixColorInt() + " and " + ColorTextulizer(gamegrid[num].GetColor()));
            } // 


            if (!restart)
            {
                attempt = 0;
                ldatabase.levels[lev].levGameGrid = gamegrid;

                // *********      Calculate next grid size    ******************
                /* G.E.R. 0: No Expansion
                 * G.E.R. 1: 1 Column or Row every other level
                 * G.E.R. 2: 1 Column or Row every level
                 * G.E.R. 3: 1 Column and Row every level
                 * G.E.R. 4: 2 Columns and Rows every level
                 * G.E.R. 5: 3 Columns and Rows every level
                 * 
                 * There is no maximum size
                 * Incidentally, all GER != 1 or 2, will always have a center
                 */
                 switch(GridExpansionRate)
                {
                    case 0: break;
                    case 1:
                        {
                            if (increaseGrid)
                            {
                                if (levCol > levRow) levRow++;
                                else levCol++;
                            }
                            increaseGrid = !increaseGrid;
                            break;
                        }
                    case 2:
                        {
                            if (levCol > levRow) levRow++;
                            else levCol++;
                            break;
                        }
                    case 3:
                        {
                            levCol++;
                            levRow++;
                            break;
                        }
                    case 4:
                        {
                            levCol += 2;
                            levRow += 2;
                            break;
                        }
                    case 5:
                        {
                            levCol += 3;
                            levRow += 3;
                            break;
                        }
                    default: break;
                }
                //Calculate the next size for the gamearray
            }
         } // Giant LEVEL for loop

        print("<b>Game Creator Finished!</b>");

        //Signal the World that Game Creator is finished
        GameUIControl GUC = GameObject.Find("Canvas/Game UI").GetComponent<GameUIControl>();
        GUC.SetUpFinalUI();
        GUC.InitTimer();
        GameObject.Find("GameDisplay").GetComponent<GameDisplay>().InitializeDisplay();
	}// Start
	
	
 /**************************************************************************************
 *     Start Functions
 * ************************************************************************************/

    //FalsifyAllRules - This makes all the Rule Booleans false as an initializer. 
    // For some reason I am having some pop up true later.
    void FalsifyAllRules()
    {
        r1000 = false;
        r1021 = false;
        r1045 = false;
        r1111 = false;
        r5555 = false;
        r1001 = false;
        r2001 = false;
        r3401 = false;
        r4031 = false;
        r5011 = false;
        r1502 = false;
        r2052 = false;
        r3002 = false;
        r4202 = false;
        r5052 = false;
        r1003 = false;
        r2003 = false;
        r3033 = false;
        r4003 = false;
        r5203 = false;
        r1004 = false;
        r2404 = false;
        r3014 = false;
        r4034 = false;
        r5004 = false;
        r1025 = false;
        r2405 = false;
        r3005 = false;
        r4205 = false;
        r5035 = false;
    }


	// AnswerValidator goes through all the rules chosen so far, and modifies the grid and colors
	// to ensure the answer is correct, and that the surrounding grid supports it
	// It will return white if an error occurs
    // lockcolor = AnswerValidator(ak.position, levCol, levRow, keycolor); // Test, if it's good get out of loop
	Color AnswerValidator(int position, int Tcol, int Trow, Color AnswerColor)
	{
		// I need to run the Answer1 through the Answer Validator before I can assign Answer2's color
		// REMEMBER ANSWERCOLOR = Key Color
		// This function changes the Answer space's Lock color and modifies or restricts the locks around this position
        // As well as whittles down the allowable Wall Colors

        int AnswerLoc = position; // Location where an answer is being attempted
		int arow = AnswerLoc / Tcol;  // The row the attempted answer is being tried
		int acol = AnswerLoc - (arow * Tcol);  // THe column the answer is being attempted
		int nrow = (Trow - 1) - arow;  // The number of rows away from the bottom
		int ncol = (Tcol - 1) - acol; // THe number of columns away from the right side
		int numsquares = Tcol * Trow;  // THe amount of squares in the grid
		bool pass = false; // The Answer color and location pass their attempt at placement

        int Above = position - Tcol; // The position above the attempted location
        int Right = position + 1;  // The position to the right of the 
        int Below = position + Tcol;
        int Left = position - 1;

        int ULCorner = 0;
        int URCorner = Tcol - 1;
        int BLCorner = numsquares - Tcol;
        int BRCorner = numsquares - 1;

        // OddX means their are an odd number of X 
        print("Tcol = " + Tcol);
        print("Trow % 2 = " + (Trow % 2));
        print("Tcol % 2 = " + (Tcol % 2));
        print("Trow / 2 = " + (Trow / 2));
        print("Tcol / 2 = " + (Tcol / 2));


        bool OddRow = (Trow % 2 != 0) ? true : false;
        bool OddCol = (Tcol % 2 != 0) ? true : false;

        // If there is a grid center, this will be set to that value, otherwise it will be set to 0
        int Center = (OddRow && OddCol) ? ConvertToArrayNum(Tcol / 2, Trow / 2, Tcol) : 0;
        if (Center != 0) print("Center found at " + Center);

        bool Color1NoSwitch = false;

        GridSpace gs = gamegrid[position];  // The GridSpace at this Location
        AnswerKey ak = answerKey.Find(x => x.position == AnswerLoc); // This position's AnswerKey

        // AnswerValidator Generic Rules -----------------------------------
        //Color1 key now only works in Color2 locks, and Color2 keys only go in Color1 locks.
        // Built into the other rules.

        /* AnswerValidator Color1 Rules -----------------------------------
        1021: Color1 locks within 2 spaces of a Color1 wall still use Color1 keys.
        1001: Color1 locks in the center row or column are bombs. 
        2001: Color1 locks against the sides of the grid are all traps if  the exact center lock is Color1.
        3401: Color1 locks are traps if a Color4 wall is on the same half of the grid.
        4031: Color1 locks on the same row as a Color3 lock are traps.
        5011: Color1 locks are all traps if all corners are Color1 locks.
        */
        if (((AnswerColor == Color1) && (!r1021)) || ((AnswerColor == Color2) && (r1021)) || ((AnswerColor == Color1) && (r1111)))
        {
            print("Color1 Rules being considered as an answer");
            bool wallCanBeSet = false;
            bool failColor1 = false;
            bool desetCenter = false;
            bool desetRow = false;
            bool desetWall = false;
            bool desetCol1Wall = false;
            bool desetCorner = false;

     /// Rule Check Area
            //1111: Color1 locks within 2 spaces of a Color1 wall still use Color1 keys.
            // If the Answer is COlor2, fail!
            if (r1111)
            {
                // If the answer is Color1, and it's close to a wall
                print("Color1 Rule 1111 is being considered"); // Debug
                if ((AnswerColor == Color1) && ((arow < 2) || (acol < 2) || (nrow < 2) || (ncol < 2)))
                {
                    // Can any of the walls in range be set to Color1
                    if (arow < 2) wallCanBeSet = wallList[UPPERWALL].IsColorAllowed(Color1);
                    if ((acol < 2) && !wallCanBeSet) wallCanBeSet = wallList[LEFTWALL].IsColorAllowed(Color1);
                    if ((nrow < 2) && !wallCanBeSet) wallCanBeSet = wallList[BOTTOMWALL].IsColorAllowed(Color1);
                    if ((ncol < 2) && !wallCanBeSet) wallCanBeSet = wallList[RIGHTWALL].IsColorAllowed(Color1);

                    if (wallCanBeSet) print("Color1 Answer and wallCanBeSet = true");// Debug
                    else
                    {
                        print("Failed 1111 because too far from a wall, or it can't be set to COlor1");
                        failColor1 = true;
                    }
                }
                // Else if the Answer is COlor2, and it is close to a wall, make sure the walls can be deset from Color1
                else if ((AnswerColor == Color2) && ((arow < 2) || (acol < 2) || (nrow < 2) || (ncol < 2)))
                {
                    bool wallIsSet = false;                  
                    // Go through each wall within range and see if any are already set to Color1
                    if (arow < 2) wallIsSet = wallList[UPPERWALL].IsColorSet(Color1);
                    if ((acol < 2) && !wallIsSet) wallIsSet = wallList[LEFTWALL].IsColorSet(Color1);
                    if ((nrow < 2) && !wallIsSet) wallIsSet = wallList[BOTTOMWALL].IsColorSet(Color1);
                    if ((ncol < 2) && !wallIsSet) wallIsSet = wallList[RIGHTWALL].IsColorSet(Color1);

                    // If the key color is Color2, then this is a fail
                    if (wallIsSet)
                    {
                        print("Failed COlor1 Rule 1111 check because answer is Color2");// Debug
                        failColor1 = true;
                    }
                    else desetCol1Wall = true;
                }                
            }
            

            // 1001: Color1 locks in the center row or column are bombs.
            // Check to see if this location is in a center column or row, if so failcolor1 is true
            if (r1001)
            {
                print("Color1 1001"); // 
                if ((OddCol) && (acol == Tcol / 2)) failColor1 = true;
                if ((OddRow) && (arow == Trow / 2)) failColor1 = true;

                if (failColor1) print("Failed Color1 1001 check"); // Debug
                else print("Passed 1001");
            }

            //2001: Color1 locks against the sides of the grid are all traps if  the exact center lock is Color1.
            // First see if the answer is supposed to be on the side
            if (!failColor1 && r2001 && ((acol == 0) || (arow == 0) || (ncol == 0) || (nrow == 0)))
            {
                print("Color1 2001");// Debug
                // Then see if there is a center grid square (both row and column are odd numbers)
                // Since the arrays start at zero, odd numbers of rows or columns will have an
                // even number total (first position is 0,0)
                if (Center != 0)
                {
                    print("THere is a center");
                    // Find out if it can be deset from Color1
                    if (gamegrid[Center].IsColorSet(Color1)) failColor1 = true;
                    else desetCenter = true;
                    if (failColor1) print("Failed Color1 2001 check"); // Debug

                    if (!failColor1) print("Passed 2001, and need to deset center");
                } 
            }

            // RULE CHECK
            //3401: Color1 locks are traps if a Color4 wall is on the same half of the grid.
            if (!failColor1 && r3401)
            {
                print("COlor1 3401"); // Debug
                // first determine which walls are on the same half of the grid (or if this damn 
                // position is in the exact center, which make ALL the walls suspect
                // if row posiiton is less then halfway, top wall is suspect
                if ((arow <= Trow / 2) && wallList[UPPERWALL].IsColorSet(Color4)) failColor1 = true;
                if ((acol <= Tcol / 2) && wallList[LEFTWALL].IsColorSet(Color4)) failColor1 = true;
                if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && (wallList[BOTTOMWALL].IsColorSet(Color4))) failColor1 = true;
                if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && (wallList[RIGHTWALL].IsColorSet(Color4))) failColor1 = true;
                if (!failColor1)
                {
                    desetWall = true;
                    print("Deset Wall set");
                }

                if (failColor1) print("Failed Color1 3401 check"); // Debug
            }

            // RULE CHECK
            //4031: Color1 locks on the same row as a Color3 lock are traps.
            if (!failColor1 && r4031)
            {
                print("Color1 4031");// Debug
                // Are any of the locks on this row set to Color3?
                foreach (GridSpace tgs in gamegrid)
                {
                    if ((tgs.row == arow) && (tgs.IsColorSet(Color3))) failColor1 = true;
                }
                if (!failColor1) desetRow = true;

                if (failColor1) print("Failed COlor1 4031 Check"); // Debug
            }

            // RULE CHECK
            //5011: Color1 locks are all traps if all corners are Color1 locks.
            if (!failColor1 && r5011)
            {
                print("Color1 5011"); // Debug
                bool ignore5011 = false;
                // This detects whether any of the corners has ALREADY been deset from COlor1
                if ((!gamegrid[ULCorner].IsColorAllowed(Color1)) ||
                    (!gamegrid[URCorner].IsColorAllowed(Color1)) ||
                    (!gamegrid[BLCorner].IsColorAllowed(Color1)) ||
                    (!gamegrid[BRCorner].IsColorAllowed(Color1)))
                    ignore5011 = true;

                if (!ignore5011)
                {
                    //In the unlikely event all four corners are set to Color1 this will fail
                    if ((gamegrid[ULCorner].IsColorSet(Color1)) &&
                        (gamegrid[URCorner].IsColorSet(Color1)) &&
                        (gamegrid[BLCorner].IsColorSet(Color1)) &&
                        (gamegrid[BRCorner].IsColorSet(Color1)))
                        failColor1 = true;

                    if (failColor1) print("Failed Color1 5011"); // Debug

                    if (!failColor1)
                    {
                        // If any of the corners can be deset, and they are not the current answer location
                        if (((!gamegrid[ULCorner].IsColorSet(Color1)) && (position != ULCorner)) ||
                            ((!gamegrid[URCorner].IsColorSet(Color1)) && (position != URCorner)) ||
                            ((!gamegrid[BLCorner].IsColorSet(Color1)) && (position != BLCorner)) ||
                            ((!gamegrid[BRCorner].IsColorSet(Color1)) && (position != BRCorner)))
                            desetCorner = true;
                    }
                }
            }
            // END COLOR1 RULE CHECK

            // *** COLOR1 GRID MODIFICATION AREA ****
            if (!failColor1)
            {
                // If wall is set, then inform Color2 rules to ignore this one

                // Rule 1111, the AnswerColor is COlor1, and it's close enough to a wall to switch one
                if (wallCanBeSet)
                {                  
                    bool done = false;
                    //Set one of the in-range walls to Color1
                    if (arow < 2)
                    {
                        wallList[UPPERWALL].SetColor(Color1);
                        done = true;
                    }
                    if ((acol < 2) && !done)
                    {
                        wallList[LEFTWALL].SetColor(Color1);
                        done = true;
                    }
                    if ((nrow < 2) && !done)
                    {
                        wallList[BOTTOMWALL].SetColor(Color1);
                        done = true;
                    }
                    if ((ncol < 2) && !done)
                    {
                        wallList[RIGHTWALL].SetColor(Color1);
                        done = true;
                    }                              
                }

                // Rule1111 again, but the Answer color is Color2, so you have to deset all the COlor1 from nearby walls
                if (desetCol1Wall)
                {
                    if (arow <= Trow / 2)
                    {
                        wallList[UPPERWALL].RemoveColor(Color1);
                        print("UpperWall deset from Color1");
                    }
                    if (acol <= Tcol / 2)
                    {
                        wallList[LEFTWALL].RemoveColor(Color1);
                        print("LeftWall deset from Color1");
                    }
                    if (arow > Trow / 2)
                    {
                        wallList[BOTTOMWALL].RemoveColor(Color1);
                        print("Bottom Wall deset from Color1");
                    }
                    if (acol > Tcol / 2)
                    {
                        wallList[RIGHTWALL].RemoveColor(Color1);
                        print("RightWall deset from Color1");
                    }
                }

                // Remove Color1 from the Center Grid SPace
                if (desetCenter)
                {
                        
                    gamegrid[Center].RemoveColor(Color1);
                    print("Deset Color1 from center, at position " + Center);
                }

                // Remove Color1 from a random corner
                // I didn't allow for the contingency of a Corner already being assigned to Color1 
                // that just happens to be picked for desetting.
                if (desetCorner)
                {
                    int CornerNum;

                    if ((AnswerLoc != ULCorner) && (!gamegrid[ULCorner].IsColorSet(Color1))) CornerNum = ULCorner;
                    else if ((AnswerLoc != URCorner) && (!gamegrid[URCorner].IsColorSet(Color1))) CornerNum = URCorner;
                    else if ((AnswerLoc != BLCorner) && (!gamegrid[BLCorner].IsColorSet(Color1))) CornerNum = BLCorner;
                    else CornerNum = BRCorner;

                    gamegrid[CornerNum].RemoveColor(Color1);
                }

                // Remove Color4 Wall
                if (desetWall)
                {
                    print("Arow: " + arow + "  Acol: " + acol);
                    if (arow <= Trow / 2)
                    {
                        wallList[UPPERWALL].RemoveColor(Color4);
                        print("UpperWall deset from Color4");
                    }
                    if (acol <= Tcol / 2)
                    {
                        wallList[LEFTWALL].RemoveColor(Color4);
                        print("LeftWall deset from Color4");
                    }
                    if (arow > Trow / 2)
                    {
                        wallList[BOTTOMWALL].RemoveColor(Color4);
                        print("Bottom Wall deset from Color4");
                    }
                    if (acol > Tcol / 2)
                    {
                        wallList[RIGHTWALL].RemoveColor(Color4);
                        print("RightWall deset from Color4");
                    }
                }

                // Remove Color3 from the answer row
                if (desetRow)
                {
                    // Find all the spaces on the row, and remove Color3 from them.
                    foreach (GridSpace tgs in gamegrid)
                    {
                        if (tgs.row == arow) tgs.RemoveColor(Color3);
                    }
                }

                if (AnswerColor == Color1) print("PASS: Color1 Answer Accepted!"); // This needs to affect the KEY solution color, not the lock!
                else print("PASS: Color2 Answer Accepted (swap color)");
                gs.SetAnswer(Color1);
                ak.FixAnswer(AnswerColor);
                pass = true;
            }
           
            if (failColor1)
            {
                pass = false;
                if (AnswerColor == Color1) print("FAIL: Color1 Answer Rejected!"); // This needs to affect the KEY solution color, not the lock!
                else print("FAIL: Color2 Answer Rejected (swap color)");
                ak.RemoveColor(AnswerColor);
            }
        }

        /* AnswerValidator Color2 Rules -----------------------------------
        1021: Color1 keys now go to Color2 locks and Color2 keys  go into Color1 locks.
        1502: Color2 locks on the side of the grid opposite of a Color5 wall are traps.
        2052: Color2 locks next to Color5 locks are traps.
        3002: Color2 locks next to a blank space are traps.
        4202: Color2 locks next to a Color2 wall are traps.
        5052: A Color2 lock between two Color5 locks is a solution, despite other rules!
        */
        if (((AnswerColor == Color2) && (!r1021)) || ((AnswerColor == Color1) && (r1021) && (!Color1NoSwitch)))
        {
            print("Color2 being considered as an answer"); // Debug
            bool failColor2 = false;
            bool desetfarwall = false;
            bool desetCol5 = false;
            bool desetWhite = false;
            bool desetWall = false;
            bool wierdRuleH = false;
            bool wierdRuleV = false;

            // RULE CHECK AREA            
            // Wierd Rule Section
            //5052: A Color2 lock between two Color5 locks uses a Color2 key as a solution, despite any other rule;
            // Let me think on this a little bit, this should be fairly rare
            // I just have to MAKE SURE THAT this doesn't happen often
            if ((r5052) && (Random.value <= 0.1))  //10% chance of this triggering
            {
                print("Color2 5052 (Wierd Rule) chosen"); // Debug
                // If location is in the middle somewhere
                if ((acol != 0) && (ncol != 0) && (arow != 0) && (nrow != 0))
                {
                    if (gamegrid[Above].IsColorAllowed(Color5) && (gamegrid[Below].IsColorAllowed(Color5))) wierdRuleV = true;
                    if (gamegrid[Left].IsColorAllowed(Color5) && (gamegrid[Right].IsColorAllowed(Color5))) wierdRuleH = true;

                    if ((wierdRuleV) && (wierdRuleH))
                    {
                        if (Random.value < 0.5) wierdRuleV = false;
                        else wierdRuleH = false;
                    }
                }

                // Else it might be on the right or left side
                else if (((arow != 0) && (nrow != 0)) && (gamegrid[Above].IsColorAllowed(Color5) && (gamegrid[Below].IsColorAllowed(Color5)))) wierdRuleV = true;

                // Or maybe it's on the top or bottom
                else if ((acol != 0) && (ncol != 0) && gamegrid[Left].IsColorAllowed(Color2) && (gamegrid[Right].IsColorAllowed(Color2))) wierdRuleH = true;

                if (wierdRuleH && wierdRuleV) print("Color2 5052 Wierd rule will be put in place"); // Debug
            } // End the Wierd Rule

            //1502: Color2 locks on the side of the grid opposite of a Color5 wall are traps.
            if ((r1502) && ((arow == 0) || (acol == 0) || (nrow == 0) || (ncol == 0)) && (!wierdRuleV && !wierdRuleH))
            {
                print("Color2 1502"); // Debug
                //Check to see if the opposing side wall is set to Color5
                if ((arow == 0) && wallList[BOTTOMWALL].IsColorSet(Color5)) failColor2 = true;
                if ((acol == 0) && wallList[RIGHTWALL].IsColorSet(Color5)) failColor2 = true;
                if ((nrow == 0) && wallList[UPPERWALL].IsColorSet(Color5)) failColor2 = true;
                if ((ncol == 0) && wallList[LEFTWALL].IsColorSet(Color5)) failColor2 = true;
                if (!failColor2) desetfarwall = true;

                if (failColor2) print("Color2 1502 failed"); // Debug
            }

            //3002: Color2 locks next to a blank space are traps.
            if ((r3002) && (!wierdRuleV && !wierdRuleH))
            {
                print("COlor2 3002"); // Debug
                if ((arow > 0) && gamegrid[Above].IsColorSet(white)) failColor2 = true;
                if ((ncol > 0) && gamegrid[Right].IsColorSet(white)) failColor2 = true;
                if ((nrow > 0) && gamegrid[Below].IsColorSet(white)) failColor2 = true;
                if ((acol > 0) && gamegrid[Left].IsColorSet(white)) failColor2 = true;
                if (!failColor2) desetWhite = true;

                if (failColor2) print("Color2 3002 failed"); // Debug
            }

            //4202: Color2 locks next to a Color2 wall are traps
            if ((r4202) && (!wierdRuleV && !wierdRuleH))
            {
                print("Color2 4202"); // Debug
                if ((arow == 0) && (wallList[UPPERWALL].IsColorSet(Color2))) failColor2 = true;
                if ((acol == 0) && (wallList[LEFTWALL].IsColorSet(Color2))) failColor2 = true;
                if ((nrow == 0) && (wallList[RIGHTWALL].IsColorSet(Color2))) failColor2 = true;
                if ((ncol == 0) && (wallList[BOTTOMWALL].IsColorSet(Color2))) failColor2 = true;
                if ((!failColor2) && ((arow == 0) || (acol == 0) || (nrow == 0) || (ncol == 0))) desetWall = true;

                if (failColor2) print("Color2 4202 failed"); // Debug
            }

            /// RULE CHECK SECTION DONE!



            //2052: Color2 locks next to Color5 locks are traps.
            if ((r2052) && (!wierdRuleV && !wierdRuleH))
            {
                print("Color2 2052");// Debug
                if (gamegrid[Above].IsColorSet(Color5) && (arow != 0)) failColor2 = true;
                if (gamegrid[Right].IsColorSet(Color5) && (ncol != 0)) failColor2 = true;
                if (gamegrid[Below].IsColorSet(Color5) && (nrow != 0)) failColor2 = true;
                if (gamegrid[Left].IsColorSet(Color5) && (acol != 0)) failColor2 = true;
                if (!failColor2) desetCol5 = true;

                if (failColor2) print("Color2 2052 failed"); // Debug
            }

            /// *** COLOR 2 GRID MODIFICATION AREA ****   
            if (wierdRuleH || wierdRuleV)
            {
                if (wierdRuleH)
                {
                    gamegrid[Right].SetLock(Color5);
                    gamegrid[Left].SetLock(Color5);
                }
                else
                {
                    gamegrid[Above].SetLock(Color5);
                    gamegrid[Below].SetLock(Color5);
                }

            }


            if (!failColor2 && !wierdRuleH && !wierdRuleV)
            {

                // Just remove COlor5 from the far wall
                if (desetfarwall)
                {
                    if (arow == 0) wallList[BOTTOMWALL].RemoveColor(Color5);
                    if (acol == 0) wallList[RIGHTWALL].RemoveColor(Color5);
                    if (nrow == 0) wallList[UPPERWALL].RemoveColor(Color5);
                    if (ncol == 0) wallList[LEFTWALL].RemoveColor(Color5);
                }

                // Remove Color5 from the surrounding locks
                if (desetCol5)
                {
                    if (arow != 0) gamegrid[Above].RemoveColor(Color5);
                    if (ncol != 0) gamegrid[Right].RemoveColor(Color5);
                    if (nrow != 0) gamegrid[Below].RemoveColor(Color5);
                    if (acol != 0) gamegrid[Left].RemoveColor(Color5);
                }

                // Remove blanks from the surrounding locks
                if (desetWhite)
                {
                    if (arow != 0) gamegrid[Above].RemoveColor(white);
                    if (ncol != 0) gamegrid[Right].RemoveColor(white);
                    if (nrow != 0) gamegrid[Below].RemoveColor(white);
                    if (acol != 0) gamegrid[Left].RemoveColor(white);
                }

                // Remove Color2 from the nearby wall
                if (desetWall)
                {
                    if (arow == 0) wallList[UPPERWALL].RemoveColor(Color2);
                    if (acol == 0) wallList[LEFTWALL].RemoveColor(Color2);
                    if (nrow == 0) wallList[BOTTOMWALL].RemoveColor(Color2);
                    if (ncol == 0) wallList[RIGHTWALL].RemoveColor(Color2);
                }
            }

            if (!failColor2)
            {

                if (AnswerColor == Color2) print("PASS: Color2 Answer accepted!");
                else print("PASS: Color1 Answer accepted (swapped)!");
                pass = true;
                ak.FixAnswer(AnswerColor); /// Set the KEY COLOR to whatever
                gs.SetAnswer(Color2);  /// Set the LOCK COLOR to Color2 (Not the key color)
            }
            else
            {
                if (AnswerColor == Color2) print("FAIL: Color2 Answer rejected!");
                else print("FAIL: Color1 Answer rejected (swapped)");
                ak.RemoveColor(AnswerColor); // Remove the KEY COLOR from consideration from this LOCK COLOR position
            }
        }

        // Color2 END

        /* AnswerValidator Color3 Rules ------------------------------
        1003: Color3 locks are all traps.
        2003: Color3 trap exception: Color3 locks in the corner are solutions!
        3033: Color3 trap exception: Color3 locks where all four adjacent sides are Color3 locks are solutions!
        4003: Color3 trap exception: A Color3 lock in the exact center of the grid is a solution!
        5203: Color3 trap exception: Color3 locks next to a Color2 wall are solutions!
        */
        if (AnswerColor == Color3)
    {
        print ("Color3 rules being considered as an answer"); // debug
        bool Color3Pass = false;        
        bool pass3033 = false;  // This says that 3033 can work on this lock, regardless if it failed for something else
        bool setWall = false; 


        //if the original rules is not in place, we are done!
        if (!r1003) Color3Pass = true;
        else
        {
            // 2003: Color3 trap exception: Color3 locks in the corner are solutions!
            if (r2003 && ((position == ULCorner) || (position == URCorner) || (position == BLCorner) || (position == BRCorner))) Color3Pass = true;

            // 3033: Color3 trap exception: Color3 locks where all four adjacent sides are Color3 locks are solutions!
            // This is a painful one, I need to make sure all four locks don't wind up in an answer position
            // First, this will only work if the answer is away from a wall
            // I'm also ensuring if this happens to be a center lock, to NOT add the four locks
            if ((r3033) && ((acol != 0) && (arow != 0) && (ncol != 0) && (nrow != 0)) && (position != Center))
            {
                // This says that this position cannot be used as a 3033 answer, it doesn't mean Color3 failed
                bool fail3033 = false;  
                
                // Make sure none of the four surrounding locks are set to be answer spaces
                foreach (AnswerKey akey in answerKey)
                {
                    if ((akey.position == Right) ||
                        (akey.position == Left) ||
                        (akey.position == Above) ||
                        (akey.position == Below)) fail3033 = true;
                }

                // Make sure none of the locks are in the exact center of the grid
                if ((r4003) && ((Right == Center) || (Left == Center) || (Above == Center) || (Below == Center))) fail3033 = true;

                // Make sure none of the locks wind up next to a Color2 wall
                if (r5203)
                {
                    if ((acol == 1 && wallList[LEFTWALL].IsColorSet(Color2))) fail3033 = true;
                    if ((arow == 1 && wallList[UPPERWALL].IsColorSet(Color2))) fail3033 = true;
                    if ((ncol == 1 && wallList[RIGHTWALL].IsColorSet(Color2))) fail3033 = true;
                    if ((nrow == 1 && wallList[BOTTOMWALL].IsColorSet(Color2))) fail3033 = true;
                }

                //FInally check all four locks to see if they've already been set to something else
                if (!fail3033)
                {
                    if ((gamegrid[Left].IsColorAllowed(Color3)) && 
                        (gamegrid[Right].IsColorAllowed(Color3)) && 
                        (gamegrid[Above].IsColorAllowed(Color3)) && 
                        (gamegrid[Left].IsColorAllowed(Color3)))
                        pass3033 = true;
                }
            }

            // 4003: Color3 trap exception: A Color3 lock in the exact center of the grid is a solution!
            if ((r4003) && (position == Center)) Color3Pass = true;

            // 5203: Color3 trap exception: Color3 locks next to a Color2 wall are solutions!
            // If this position is on the side
            if ((r5203) && ((arow == 0) || (acol == 0) || (nrow == 0) || (ncol == 0)))
            {
                //Next ensure the wall can be changed to Color2
                if ((arow == 0) && (wallList[UPPERWALL].IsColorAllowed(Color2))) setWall = true;
                if ((acol == 0) && (wallList[LEFTWALL].IsColorAllowed(Color2))) setWall = true;
                if ((nrow == 0) && (wallList[BOTTOMWALL].IsColorAllowed(Color2))) setWall = true;
                if ((ncol == 0) && (wallList[RIGHTWALL].IsColorAllowed(Color2))) setWall = true;
            }

        }

   /// COLOR3 MODIFY GRID SECION ///
            
        // Set up for r3033
        if (pass3033)
        {
            // Deset any close walls first
            if (acol == 1) wallList[LEFTWALL].RemoveColor(Color2);
            if (arow == 1) wallList[UPPERWALL].RemoveColor(Color2);
            if (ncol == 1) wallList[RIGHTWALL].RemoveColor(Color2);
            if (nrow == 1) wallList[BOTTOMWALL].RemoveColor(Color2);

            // Then set the surrounding locks to COlor3
            gamegrid[Left].SetLock(Color3);
            gamegrid[Right].SetLock(Color3);
            gamegrid[Above].SetLock(Color3);
            gamegrid[Left].SetLock(Color3);
            Color3Pass = true;
        } 

        // Set up for r5203
        if (setWall)
        {
            bool done = false;
            //change Wall to Color2
            if (arow == 0)
            {
                wallList[UPPERWALL].SetColor(Color2);
                done = true;
            }
            if ((acol == 0) && !done)
            {
                wallList[LEFTWALL].SetColor(Color2);
                done = true;
            }
            if ((nrow == 0) && !done)
            {
                wallList[BOTTOMWALL].SetColor(Color2);
                done = true;
            }
            if ((ncol == 0) && !done) wallList[RIGHTWALL].SetColor(Color2);

            Color3Pass = true;
        }


        if (Color3Pass)
        {
            print("Color3 Accepted!");
            gs.SetAnswer(AnswerColor);
            ak.FixAnswer(AnswerColor);
            pass = true;
        }
        else
        {
            print("Color3 Lock Rejected! New Key color Needed!"); // This needs to affect the KEY solution color, not the lock!
            ak.RemoveColor(Color3);
        }
    } //Answer Color3 END

        /* AnswerValidator Color4 Rules ----------------------------------
            1045: 1045: Color4 keys now go to Color5 locks and Color5 keys go into Color4 locks.
            1004: Color4 locks in a corner are traps.
            2404: Color4 locks on the opposite half of the grid from a Color4 wall are traps (including center row or column).
            3024: Color4 locks next to a Color2 lock are traps.
            4034: Color4 locks on the same row or column as a Color3 lock are traps.
            5004: Color4 locks on the same half of the grid as a Color1 wall are traps.
            5555: Color5 locks within 2 spaces of a Color5 wall still use Color5 keys

                if (((AnswerColor == Color4) && (!r1045)) || ((AnswerColor == Color5) && (r1045) && (!Color1NoSwitch)))
            */

        if ((AnswerColor == Color4 && !r1045) || ((AnswerColor == Color5) && (r1045)))
        {
            print ("Color4 lock being considered.");
            bool failColor4 = false;
            bool desetOppWall = false;
            bool desetCol1 = false;
            bool desetCol3 = false;
            bool desetCol5Wall = false;
            bool desetCol1Wall = false;


            // 5555: Color5 locks within 2 spaces of a Color5 wall still use Color5 keys
            // I need to check that if this is a Color5, and it is closer than two spaces to a wall
            // that if the wall is set to Color5 to fail this one, otherwise I'll need to deset the walls
            // from COlor5
            if (r5555 && (AnswerColor == Color5))
            {
                if ((arow < 2) && (wallList[UPPERWALL].IsColorSet(Color5))) failColor4 = true;
                if ((nrow < 2) && (wallList[BOTTOMWALL].IsColorSet(Color5))) failColor4 = true;
                if ((acol < 2) && (wallList[LEFTWALL].IsColorSet(Color5))) failColor4 = true;
                if ((ncol < 2) && (wallList[RIGHTWALL].IsColorSet(Color5))) failColor4 = true;

                // If this didn't fail COlor5, I give this a 50% chance to deset, or fail it anyway
                if ((!failColor4) && Random.value < 0.5) desetCol5Wall = true;
                else failColor4 = true;
            }

            // 1004: Color4 locks in a corner are traps.
            if ((r1004) && ((position == URCorner) || (position == ULCorner) || (position == BLCorner) || (position == BRCorner))) failColor4 = true;

            // 2404: Color4 locks on the opposite half of the grid from a Color4 wall are traps.
            if (r2404)
            {
                if ((arow <= Trow / 2) && wallList[BOTTOMWALL].IsColorSet(Color4)) failColor4 = true;
                if ((acol <= Tcol / 2) && wallList[RIGHTWALL].IsColorSet(Color4)) failColor4 = true;
                if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && wallList[UPPERWALL].IsColorSet(Color4)) failColor4 = true;
                if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && wallList[LEFTWALL].IsColorSet(Color4)) failColor4 = true;
                if (!failColor4) desetOppWall = true;
            }

            // 3014: Color4 locks next to a Color1 lock are traps.
            if ((r3014) && !failColor4)
            {
                if (gamegrid[Above].IsColorSet(Color1) && (arow != 0)) failColor4 = true;
                if (gamegrid[Right].IsColorSet(Color1) && (ncol != 0)) failColor4 = true;
                if (gamegrid[Below].IsColorSet(Color1) && (nrow != 0)) failColor4 = true;
                if (gamegrid[Left].IsColorSet(Color1) && (acol != 0)) failColor4 = true;
                if (!failColor4) desetCol1 = true;
            }

            // 4034: Color4 locks on the same row or column as a Color3 lock are traps.
            if ((r4034) && !failColor4)
            {
                // Are any of the locks on this row set to Color3?
                foreach (GridSpace tgs in gamegrid)
                {
                    if (((tgs.row == arow) || (tgs.column == acol)) && (tgs.IsColorSet(Color3))) failColor4 = true;
                }
                    if (!failColor4) desetCol3 = true;
            }

            // 5004: Color4 locks on the same half of the grid as a Color1 wall are traps.
            if ((r5004) && !failColor4)
            {
                if ((arow <= Trow / 2) && wallList[UPPERWALL].IsColorSet(Color1)) failColor4 = true;
                if ((acol <= Tcol / 2) && wallList[LEFTWALL].IsColorSet(Color1)) failColor4 = true;
                if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && (wallList[BOTTOMWALL].IsColorSet(Color1))) failColor4 = true;
                if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && (wallList[RIGHTWALL].IsColorSet(Color1))) failColor4 = true;
                if (!failColor4) desetCol1Wall = true;
            }

            /// COLOR4 MODIFY GRID SECION ///

            if (!failColor4)
            {
                if (desetOppWall)
                {
                    if (arow <= Trow / 2) wallList[BOTTOMWALL].RemoveColor(Color4);
                    if (acol <= Tcol / 2) wallList[RIGHTWALL].RemoveColor(Color4);
                    if (arow > Trow / 2) wallList[UPPERWALL].RemoveColor(Color4);
                    if (acol > Tcol / 2) wallList[LEFTWALL].RemoveColor(Color4);
                }

                if (desetCol1)
                {
                    if (arow != 0) gamegrid[Above].RemoveColor(Color1);
                    if (ncol != 0) gamegrid[Right].RemoveColor(Color1);
                    if (nrow != 0) gamegrid[Below].RemoveColor(Color1);
                    if (acol != 0) gamegrid[Left].RemoveColor(Color1);
                }

                if (desetCol3)
                {
                    foreach (GridSpace tgs in gamegrid)
                    {
                        if ((tgs.row == arow) || (tgs.column == acol)) tgs.RemoveColor(Color3);
                    }
                }

                if (desetCol1Wall)
                {
                    if (arow <= Trow / 2) wallList[UPPERWALL].RemoveColor(Color1);
                    if (acol <= Tcol / 2) wallList[LEFTWALL].RemoveColor(Color1);
                    if (arow > Trow / 2) wallList[BOTTOMWALL].RemoveColor(Color1);
                    if (acol > Tcol / 2) wallList[RIGHTWALL].RemoveColor(Color1);
                }

                if (desetCol5Wall)
                {
                    if (arow < 2) wallList[UPPERWALL].RemoveColor(Color5);
                    if (acol < 2) wallList[LEFTWALL].RemoveColor(Color5);
                    if (nrow < 2) wallList[BOTTOMWALL].RemoveColor(Color5);
                    if (ncol < 2) wallList[RIGHTWALL].RemoveColor(Color5);
                }

            }

            if (!failColor4)
            {

                if (AnswerColor == Color4) print("PASS: Color4 Answer accepted!");
                else print("PASS: Color5 Answer accepted (swapped)!");
                pass = true;
                ak.FixAnswer(AnswerColor); /// Set the KEY COLOR to whatever
                gs.SetAnswer(Color4);  /// Set the LOCK COLOR to Color4 (Not the key color)
            }
            else
            {
                if (AnswerColor == Color4) print("FAIL: Color4 Answer rejected!");
                else print("FAIL: Color5 Answer rejected (swapped)");
                ak.RemoveColor(AnswerColor); // Remove the KEY COLOR from consideration from this LOCK COLOR position
            }

        } // If Answer is Color4

        /* AnswerValidator Color5 Rules -----------------------------------
        1015: Color5 locks next to a Color1 lock are traps.
        2405: Color5 locks on the same half of the grid as a Color4 wall are traps.
        3005: Color5 locks away from the sides of the grid are traps if the exact center lock of the grid is Color5.
        4205: Color5 locks against the side of the grid opposite from a Color2 wall are traps.
        5035: Color5 locks in the same column as a Color3 lock are traps.
        */

        if (((AnswerColor == Color5) && (!r1045 || r5555)) || ((AnswerColor == Color4) && (r1045)))
    {
        print("Color5 lock being considered.");
            bool failColor5 = false;
            bool desetCol2 = false;
            bool desetWall = false;
            bool desetCenter = false;
            bool desetFarWall = false;
            bool desetCol3 = false;
            bool setCol5Wall = false;

            //If this is a COlor5 (with 1045 and 5555 in place then I need to check that
            // this position is within 2 of a COlor5 settable wall.
            if ((r5555) && (AnswerColor == Color5))
            {
                if ((arow < 2) && (wallList[UPPERWALL].IsColorAllowed(Color5))) setCol5Wall = true;
                if ((nrow < 2) && (wallList[BOTTOMWALL].IsColorAllowed(Color5))) setCol5Wall = true;
                if ((acol < 2) && (wallList[LEFTWALL].IsColorAllowed(Color5))) setCol5Wall = true;
                if ((ncol < 2) && (wallList[RIGHTWALL].IsColorAllowed(Color5))) setCol5Wall = true;

                if (!setCol5Wall) failColor5 = true;
            }

            //1015: Color5 locks next to a Color1 lock are traps.
            if ((r1025) && !failColor5)
            {
                if (gamegrid[Above].IsColorSet(Color2) && (arow != 0)) failColor5 = true;
                if (gamegrid[Right].IsColorSet(Color2) && (ncol != 0)) failColor5 = true;
                if (gamegrid[Below].IsColorSet(Color2) && (nrow != 0)) failColor5 = true;
                if (gamegrid[Left].IsColorSet(Color2) && (acol != 0)) failColor5 = true;
                if (!failColor5) desetCol2 = true;
            }

            //2405: Color5 locks on the same half of the grid as a Color4 wall are traps.
            if ((r2405) && !failColor5)
            {
                if ((arow <= Trow / 2) && wallList[UPPERWALL].IsColorSet(Color4)) failColor5 = true;
                if ((acol <= Tcol / 2) && wallList[LEFTWALL].IsColorSet(Color4)) failColor5 = true;
                if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && wallList[BOTTOMWALL].IsColorSet(Color4)) failColor5 = true;
                if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && wallList[RIGHTWALL].IsColorSet(Color4)) failColor5 = true;
                if (!failColor5) desetWall = true;
            }

            //3005: Color5 locks away from the sides of the grid are traps if the exact center lock of the grid is Color5.
            if ((r3005) && ((arow != 0) && (acol != 0) && (nrow != 0) && (ncol != 0)) && !failColor5)
            {
                if (gamegrid[Center].IsColorSet(Color5)) failColor5 = true;
                if (!failColor5) desetCenter = true;
            }

            //4205: Color5 locks against the side of the grid opposite from a Color2 wall are traps.
            if ((r4205) && ((arow == 0) || (acol == 0) || (nrow == 0) || (ncol == 0)) && !failColor5)
            {
                if ((arow == 0) && wallList[RIGHTWALL].IsColorSet(Color2)) failColor5 = true;
                if ((acol == 0) && wallList[BOTTOMWALL].IsColorSet(Color2)) failColor5 = true;
                if ((nrow == 0) && wallList[UPPERWALL].IsColorSet(Color2)) failColor5 = true;
                if ((ncol == 0) && wallList[LEFTWALL].IsColorSet(Color2)) failColor5 = true;
                if (!failColor5) desetFarWall = true;
            }

            //5035: Color5 locks in the same column as a Color3 lock are traps.
            if ((r5035) && !failColor5)
            {
                // Are any of the locks on this column set to Color3?
                foreach (GridSpace tgs in gamegrid)
                {
                    if ((tgs.column == acol) && (tgs.IsColorSet(Color3))) failColor5 = true;
                }
                if (!failColor5) desetCol3 = true;
            }

            /// COlor5 Modify Grid Area ///

            if (!failColor5)
            {
                if (setCol5Wall)
                {
                    bool done = false;
                    if (arow < 0)
                    {
                        wallList[UPPERWALL].SetColor(Color5);
                        done = true;
                    }

                    if ((acol == 0) && !done)
                    {
                        wallList[LEFTWALL].SetColor(Color5);
                        done = true;
                    }
                    if ((nrow == 0) && !done)
                    {
                        wallList[BOTTOMWALL].SetColor(Color5);
                        done = true;
                    }
                    if ((ncol == 0) && !done) wallList[RIGHTWALL].SetColor(Color5);
                      
                }

                if (desetCol2)
                {
                    if (arow != 0) gamegrid[Above].RemoveColor(Color2);
                    if (ncol != 0) gamegrid[Right].RemoveColor(Color2);
                    if (nrow != 0) gamegrid[Below].RemoveColor(Color2);
                    if (acol != 0) gamegrid[Left].RemoveColor(Color2);
                }

                if (desetWall)
                {
                    if (arow <= Trow / 2) wallList[UPPERWALL].RemoveColor(Color4);
                    if (acol <= Tcol / 2) wallList[LEFTWALL].RemoveColor(Color4);
                    if (((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) wallList[BOTTOMWALL].RemoveColor(Color4);
                    if (((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) wallList[RIGHTWALL].RemoveColor(Color4);
                }

                if (desetCenter)
                {
                    gamegrid[Center].RemoveColor(Color5);
                }

                if (desetFarWall) 
                {
                    if (arow == 0) wallList[RIGHTWALL].RemoveColor(Color2);
                    if (acol == 0) wallList[BOTTOMWALL].RemoveColor(Color2);
                    if (nrow == 0) wallList[UPPERWALL].RemoveColor(Color2);
                    if (ncol == 0) wallList[LEFTWALL].RemoveColor(Color2);
                }

                if (desetCol3)
                {
                    foreach (GridSpace tgs in gamegrid)
                    {
                        if (tgs.column == acol) tgs.RemoveColor(Color3);
                    }
                }
            }

            if (!failColor5)
            {

                if (AnswerColor == Color5) print("PASS: Color5 Answer accepted!");
                else print("PASS: Color4 Answer accepted (swapped)!");
                pass = true;
                ak.FixAnswer(AnswerColor); /// Set the KEY COLOR to whatever
                gs.SetAnswer(Color5);  /// Set the LOCK COLOR to Color2 (Not the key color)
            }
            else
            {
                if (AnswerColor == Color5) print("FAIL: Color5 Answer rejected!");
                else print("FAIL: Color4 Answer rejected (swapped)");
                ak.RemoveColor(AnswerColor); // Remove the KEY COLOR from consideration from this LOCK COLOR position
            }

        } // If COlor5 END

    if (pass) return AnswerColor;
    return white;
} //AnswerValidator END

    //ValidateSlotRule is passed a position and the grid size.
    // Big CHanges from the last iteration
    // The ValidatedSlotRule 
    // If it discoveres a slot with all the options forbidden it will error out
    // THis is for choosing an available rule to apply to an "Other" slot to 
    // invalidate it.
    bool ValidatedSlotRule(int position, int Tcol, int Trow, int lv)
    {
        //Get information on the location of the slot
        int arow = position / Tcol;
        int acol = position - (arow * Tcol);
        int nrow = (Trow - 1) - arow;
        int ncol = (Tcol - 1) - acol;
        int numsquares = Tcol * Trow;
        bool solved = false;

        int ULCorner = 0;
        int URCorner = Tcol - 1;
        int BLCorner = numsquares - Tcol;
        int BRCorner = numsquares - 1;

        int Above = position - Tcol;
        int Right = position + 1;
        int Below = position + Tcol;
        int Left = position - 1;

        // OddX means the max number is odd. (0, 1, 2, 3) = Odd
        bool OddRow = (Trow % 2 != 0) ? true : false;
        bool OddCol = (Tcol % 2 != 0) ? true : false;

        // If there is a grid center, this will be set to that value, otherwise it will be set to 0
        int Center = (OddRow && OddCol) ? ConvertToArrayNum(Tcol / 2, Trow / 2, Tcol) : 0;

        GridSpace gs = gamegrid[position];

        Color TryColor = gs.GetRandomAllowedColor(true);
        
        // NOTE: To make my grids consistantly more colorful, all colors will be tried first before setting to white (blank)
        // I have changed my rules so that there are no reasons that you can't change a non-answer lock to a blank.
        // The AnswerValidator already removes white as a possibility when it interferes with an answer
        // I was going to add a list of rule violations, but since the grid is made square by square, the reason would change.
        // I will have to write a "Violation Assessment" script that fires off once the wrong square is picked.
        int trycolornum = ColorToInt(TryColor);
 
        print("Slot " + position + ": " + "Attempting Color" + trycolornum);
        switch (trycolornum)
        {
            //ValidatedSlotRule  WHITE RULES -------------------------
            case 0: // If white is being tried
                {
                    solved = true;
                    print("White Accepted!");
                    break;
                }
            //ValidatedSlotRule  COLOR1 RULES ------------------------
            // Close Color1 wall, not in all corners, not in the row with Color3, and less than four white slots in grid. Don't forget the swap with COlor2         
            case 1: // If slot is Color1
                    /*
                        1121: Color1 locks within 2 spaces of a Color1 wall still use Color1 keys. - NA!
                        1001: Color1 locks in the center row or column are bombs. 
                        2001: Color1 locks against the sides of the grid are all traps if the exact center lock is Color1.
                        3401: Color1 locks are traps if a Color4 wall is on the same half of the grid.
                        4031: Color1 locks on the same row as a Color3 lock are traps.
                        5011: Color1 locks are all traps if all corners are Color1 locks.
                    */
                    // First CHECK!
                print("Bomblock Check Color1");// Debug
                if (r5011)
                {
                    print("Rule 5011");// Debug
                    // This one requires a little finesse
                    // First check to see if all corner locks are 5011, if so, then solved
                    // If not check to see if ALL the corners can be set to COlor1, and then set them
                    // The Answer Validator will have already deset a corner if Color1 is an answer
                    if ((gamegrid[ULCorner].IsColorSet(Color1)) &&
                        (gamegrid[URCorner].IsColorSet(Color1)) &&
                        (gamegrid[BLCorner].IsColorSet(Color1)) &&
                        (gamegrid[BRCorner].IsColorSet(Color1)))
                        solved = true;
                    // If all the corner locks can be set to Color1
                    else if ((gamegrid[ULCorner].IsColorAllowed(Color1)) &&
                        (gamegrid[URCorner].IsColorAllowed(Color1)) &&
                        (gamegrid[BLCorner].IsColorAllowed(Color1)) &&
                        (gamegrid[BRCorner].IsColorAllowed(Color1)))
                        {
                            gamegrid[ULCorner].SetLock(Color1);
                            gamegrid[URCorner].SetLock(Color1);
                            gamegrid[BLCorner].SetLock(Color1);
                            gamegrid[BRCorner].SetLock(Color1);
                            solved = true;
                        print("5011 ALL the Corner locks have been set to Color1");// Debug
                        }
                    if (solved) print("Color1 bomb set by 5011");// Debug

                }

                if ((r1001) && !solved)
                {
                    print("Rule 1001"); // Debug
                    // Simply check to see if this location is in the center row or column
                    if ((OddRow) && (arow == Trow / 2)) solved = true;
                    if ((OddCol) && (acol == Tcol / 2)) solved = true;
                    if (solved) print("Color1 bomb set by 1001 (Middle row or column)");// Debug
                }

                if ((r2001) && !solved)
                {
                    print("Rule 2001");  //Debug
                    // FIrst check to see if this is a center position, if so make it COlor1, and solved!
                    // If it isn't a center position, then see if this position is on the side AND the center position has been made COlor1, if so then solved!
                    print("Center is " + Center);
                    if ((Center != 0) && (position == Center))
                    {
                        print("Center lock set to bomb for Color1");
                        solved = true;
                    }
                    else if ((gamegrid[Center].IsColorSet(Color1) && ((arow == 0) || (acol == 0) || (nrow == 0) || (ncol == 0))))
                    {
                        print("This pos is on the side, and center lock is COlor1, so this is a bomb!");
                        solved = true;
                    }

                    if (solved) print("Color1 bomb set by 2001 (Center lock is Color1)"); //Debug
                }

                if ((r3401) && !solved)
                {
                    print("Rule 3401"); //Debug
                    //Check to see if there is a Color4 wall on this grid half
                    if ((arow <= Trow / 2) && wallList[UPPERWALL].IsColorSet(Color4)) solved = true;
                    if ((acol <= Tcol / 2) && wallList[LEFTWALL].IsColorSet(Color4)) solved = true;
                    if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && wallList[BOTTOMWALL].IsColorSet(Color4)) solved = true;
                    if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && wallList[RIGHTWALL].IsColorSet(Color4)) solved = true;

                        if (solved) print("Color1 bomb set by 3401 (Color4 wall in the same half)"); //Debug
                }

                if ((r4031) && !solved)
                {
                    print("Rule 4031"); //Debug
                    // Check to see if there is a Color3 lock set on this row
                    foreach (GridSpace tgs in gamegrid)
                    {
                        if ((gs.row == arow) && (gs.IsColorSet(Color3))) solved = true;
                    }

                    if (solved) print("Color1 bomb set by 4031 (A Color3 lock found on the same row)"); //Debug
                }

                if (solved) print("Color1 Accepted at " + position); //Debug
                else print("Color1 Rejected at " + position); //Debug
                break;


            // ValidatedSlotRule  COLOR2 RULES ------------------------
            case 2:
                /*
                1502: Color2 locks on the side of the grid opposite of a Color5 wall are traps.
                2052: Color2 locks next to Color5 locks are traps.
                3002: Color2 locks next to a blank space are traps.
                4202: Color2 locks next to a Color2 wall are traps.
                5052: A Color2 lock between two Color5 locks uses a Color2 key as a solution!
                */
                {
                    print("Color2 bomb being considered"); //Debug
                    if (r1502)
                    {
                        print("Rule 1502");  //Debug
                        // CHeck to see if this position is on the side, and the opposite wall is Color5
                        if ((arow == 0) && (wallList[BOTTOMWALL].IsColorSet(Color5))) solved = true;
                        if ((nrow == 0) && (wallList[UPPERWALL].IsColorSet(Color5))) solved = true;
                        if ((acol == 0) && (wallList[RIGHTWALL].IsColorSet(Color5))) solved = true;
                        if ((ncol == 0) && (wallList[LEFTWALL].IsColorSet(Color5))) solved = true;
                        if (solved) print("Color2 bomb set by 1502 (This lock is on a side opposite from Color 5 wall");  //Debug
                    }

                    if ((r2052) && !solved)
                    {
                        print("Rule 2052"); //Debug
                        // Simple, just check to see if any of the adjacent spaces are COlor5
                        if ((arow != 0) && gamegrid[Above].IsColorSet(Color5)) solved = true;
                        if ((acol != 0) && gamegrid[Left].IsColorSet(Color5)) solved = true;
                        if ((nrow != 0) && gamegrid[Below].IsColorSet(Color5)) solved = true;
                        if ((ncol != 0) && gamegrid[Right].IsColorSet(Color5)) solved = true;
                        if (solved) print("Color2 bomb set by 2052 (A Color5 lock is adjacent");  //Debug
                    }

                    if ((r3002) && !solved)
                    {
                        print("Rule 3002"); //Debug
                        // Simple, just check to see if any of the adjacent spaces are COlor5
                        if ((arow != 0) && gamegrid[Above].IsColorSet(white)) solved = true;
                        if ((acol != 0) && gamegrid[Left].IsColorSet(white)) solved = true;
                        if ((nrow != 0) && gamegrid[Below].IsColorSet(white)) solved = true;
                        if ((ncol != 0) && gamegrid[Right].IsColorSet(white)) solved = true;
                        if (solved) print("Color2 bomb set by 3002 (Blank space adjacent)"); //Debug
                    }

                    if ((r4202) && !solved)
                    {
                        print("Rule 4202");  //Debug
                        // Check to see if this is on a side, and the adjacent wall is Color2
                        if ((arow == 0) && (wallList[UPPERWALL].IsColorSet(Color2))) solved = true;
                        if ((nrow == 0) && (wallList[BOTTOMWALL].IsColorSet(Color2))) solved = true;
                        if ((acol == 0) && (wallList[LEFTWALL].IsColorSet(Color2))) solved = true;
                        if ((ncol == 0) && (wallList[RIGHTWALL].IsColorSet(Color2))) solved = true;
                        if (solved) print("Color2 bomb set by 4202 (Color2 wall is next to it");  //Debug
                    }

                    if (r5052)
                    {
                        print("Rule 5052");  //Debug
                        // This one is tricky, since it could possibly make a solution on accident
                        // I will need to make sure to make sure that it can't be set Color5 horiz, or vert
                        bool solvedH = false;
                        bool solvedV = false;

                        if ((acol == 0) || (ncol == 0)) solvedH = true;
                        else if (!gamegrid[Right].IsColorAllowed(Color5) || !gamegrid[Left].IsColorAllowed(Color5)) solvedH = true;

                        if ((arow == 0) || (acol == 0)) solvedV = true;
                        else if (!gamegrid[Above].IsColorAllowed(Color5) || !gamegrid[Below].IsColorAllowed(Color5)) solvedV = true;

                        if (!solvedH || !solvedV) solved = false;
                        if (!solved) print("Yikes, can't use this space for Color2 bomb, two adjaecent sides could be Color5");
                    }

                } // if !solved

                if (solved) print("Color2 Accepted at " + position);
                else print("Color2 Rejected at " + position);
                break;

            // ValidatedSlotRule  COLOR3 RULES ----------------------
            /*
            1003: Color3 locks are all traps.
            2003: Color3 trap exception: Color3 locks in the corner are solutions!
            3033: Color3 trap exception: Color3 locks where all four adjacent sides are Color3 locks are solutions!
            4003: Color3 trap exception: A Color3 lock in the exact center of the grid is a solution!
            5203: Color3 trap exception: Color3 locks next to a Color2 wall are solutions!
            */
            case 3:
                {
                    bool fail3 = false;

                    if (r1003)
                    {
                        /// Go through each of the exceptions, and make sure they aren't inadvertantly made into answers
                        if ((r2003) && ((position == ULCorner) || (position == URCorner) || (position == BLCorner) || (position == BRCorner))) fail3 = true;

                        // This will ensure that at least one of the adjacent points are NOT set Color3
                        if ((r3033) && ((arow > 0) && (ncol > 0) && (nrow > 0) && (acol > 0)))
                        {
                            if ((gamegrid[Above].IsColorAllowed(Color3)) &&
                                (gamegrid[Right].IsColorAllowed(Color3)) &&
                                (gamegrid[Left].IsColorAllowed(Color3)) &&
                                (gamegrid[Below].IsColorAllowed(Color3)))
                                fail3 = true;
                        }

                        if ((r4003) && (Center > 0) && (position == Center)) fail3 = true;

                        if (r5203)
                        {
                            if ((arow == 0) && (wallList[UPPERWALL].IsColorSet(Color2))) fail3 = true;
                            if ((nrow == 0) && (wallList[BOTTOMWALL].IsColorSet(Color2))) fail3 = true;
                            if ((acol == 0) && (wallList[LEFTWALL].IsColorSet(Color2))) fail3 = true;
                            if ((ncol == 0) && (wallList[RIGHTWALL].IsColorSet(Color2))) fail3 = true;
                        }

                        if (!fail3) solved = true;

                    }
                }
                if (solved) print("Color3 Accepted at " + position);
                else print("Color3 Rejected at " + position);
                break;

            // ValidatedSlotRule  COLOR4 RULES ----------------------------
            /*
            1004: Color4 locks in a corner are traps.
            2404: Color4 locks on the opposite half of the grid from a Color4 wall are traps (including center row or column).
            3024: Color4 locks next to a Color2 lock are traps.
            4034: Color4 locks on the same row or column as a Color3 lock are traps.
            5004: Color4 locks on the same half of the grid as a Color1 wall are traps.
            */
            case 4:
                { 
                    // Color4 lock in the corners are TRAPS!
                    if ((r1004) && ((position == ULCorner) || (position == URCorner) || (position == BLCorner) || (position == BRCorner))) solved = true;

                    if ((r2404) && !solved)
                    {
                        //Check to see if there is a Color4 wall on opposite half of this grid
                        if ((arow <= Trow / 2) && wallList[BOTTOMWALL].IsColorSet(Color4)) solved = true;
                        if ((acol <= Tcol / 2) && wallList[RIGHTWALL].IsColorSet(Color4)) solved = true;
                        if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && wallList[UPPERWALL].IsColorSet(Color4)) solved = true;
                        if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && wallList[LEFTWALL].IsColorSet(Color4)) solved = true;
                    }

                    // Color4 locks next to Color2 locks are TRAPS!!
                    if ((r3014) && !solved)
                    {
                        if ((acol > 0) && (gamegrid[Left].IsColorSet(Color1))) solved = true;
                        if ((arow > 0) && (gamegrid[Above].IsColorSet(Color1))) solved = true;
                        if ((ncol > 0) && (gamegrid[Right].IsColorSet(Color1))) solved = true;
                        if ((nrow > 0) && (gamegrid[Below].IsColorSet(Color1))) solved = true;
                    }

                    // Color4 locks on the same column or row as Color3 locks are TRAPS!!		
                    if ((r4034) && !solved)
                    {
                        foreach (GridSpace tgs in gamegrid)
                        {
                            if ((tgs.column == acol) && tgs.IsColorSet(Color3)) solved = true;
                            if ((tgs.row == arow) && (tgs.IsColorSet(Color3))) solved = true;
                        }
                    }

                    if ((r5004) && !solved)
                    {
                        //Check to see if there is a Color1 wall on this half of this grid
                        if ((arow <= Trow / 2) && wallList[UPPERWALL].IsColorSet(Color1)) solved = true;
                        if ((acol <= Tcol / 2) && wallList[LEFTWALL].IsColorSet(Color1)) solved = true;
                        if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && wallList[BOTTOMWALL].IsColorSet(Color1)) solved = true;
                        if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && wallList[RIGHTWALL].IsColorSet(Color1)) solved = true;
                    }
                }

                if (solved) print("Color4 Accepted at " + position);
                else print("Color4 Rejected at " + position);

                break;

            // ValidatedSlotRule  COLOR5 RULES -----------------------------------
            /*
            5052: A Color5 lock between two Color2 locks uses a Color2 key as a solution!
            1015: Color5 locks next to a Color1 lock are traps.
            2405: Color5 locks on the same half of the grid as a Color4 wall are traps.
            3005: Color5 locks away from the sides of the grid are traps if the exact center lock of the grid is Color5.
            4205: Color5 locks against the side of the grid opposite from a Color2 wall are traps.
            5035: Color5 locks in the same column as a Color3 lock are traps. 
            */
            case 5:
                {
                    bool fail5 = false;

                    // 5052: A Color5 lock between two Color2 locks uses a Color2 key as a solution!
                    if (r5052)
                    {                            
                        // I just have to make sure I don't accidently set this as an answer
                        // I have to check to see if there is a Color2 lock on both sides, and if so then this won't work
                        if ((arow > 0) && gamegrid[Above].IsColorSet(Color2) && (nrow > 0) && gamegrid[Below].IsColorSet(Color2)) fail5 = true;
                        if ((acol > 0) && gamegrid[Left].IsColorSet(Color2) && (ncol > 0) && gamegrid[Right].IsColorSet(Color2)) fail5 = true;

                        if (!fail5)
                        {
                            if ((arow > 0) && gamegrid[Above].IsColorSet(Color2) && (nrow > 0)) gamegrid[Below].RemoveColor(Color2);
                            if ((nrow > 0) && gamegrid[Below].IsColorSet(Color2) && (arow > 0)) gamegrid[Above].RemoveColor(Color2);
                            if ((acol > 0) && gamegrid[Left].IsColorSet(Color2) && (ncol > 0)) gamegrid[Right].RemoveColor(Color2);
                            if ((ncol > 0) && gamegrid[Right].IsColorSet(Color2) && (nrow > 0)) gamegrid[Left].RemoveColor(Color2);
                        }
                    }

                    if (!fail5)
                    {
                        // Check to see if this position is in the center, and set it, solved,
                        // Or check to see if this position is away from the side, 
                        if (r3005)
                        {                               
                            // FIrst check to see if this is a center position, if so make it COlor1, and solved!
                            // If it isn't a center position, then see if this position is on the side AND the center position has been made COlor1, if so then solved!
                            if ((Center != 0) && (position == Center)) solved = true;
                            else if ((gamegrid[Center].IsColorSet(Color5) && ((arow > 0) && (acol > 0) && (nrow > 0) && (ncol > 0)))) solved = true;                              
                        }

                        // Look around and see if there are any Color1 locks
                        if ((r1025) && !solved)
                        {
                            if ((arow > 0) && gamegrid[Above].IsColorSet(Color2)) solved = true;
                            if ((acol > 0) && gamegrid[Left].IsColorSet(Color2)) solved = true;
                            if ((nrow > 0) && gamegrid[Below].IsColorSet(Color2)) solved = true;
                            if ((ncol > 0) && gamegrid[Right].IsColorSet(Color2)) solved = true;
                        }

                        // Check for a Color4 Wall on this half of the grid
                        if ((r2405) && !solved)
                        {
                            if ((arow <= Trow / 2) && wallList[UPPERWALL].IsColorSet(Color4)) solved = true;
                            if ((acol <= Tcol / 2) && wallList[LEFTWALL].IsColorSet(Color4)) solved = true;
                            if ((((OddRow) && (arow > Trow / 2)) || ((!OddRow) && (arow >= Trow / 2))) && wallList[BOTTOMWALL].IsColorSet(Color4)) solved = true;
                            if ((((OddCol) && (acol > Tcol / 2)) || ((!OddCol) && (acol >= Tcol / 2))) && wallList[RIGHTWALL].IsColorSet(Color4)) solved = true;
                        }

                        // 4205: Color5 locks against the side of the grid opposite from a Color2 wall are traps.
                        if ((r4205) && !solved && ((arow == 0) || (nrow == 0) || (acol == 0) || (ncol == 0)))
                        {
                            if ((arow == 0) && wallList[BOTTOMWALL].IsColorSet(Color2)) solved = true;
                            if ((acol == 0) && wallList[RIGHTWALL].IsColorSet(Color2)) solved = true;
                            if ((nrow == 0) && wallList[UPPERWALL].IsColorSet(Color2)) solved = true;
                            if ((ncol == 0) && wallList[LEFTWALL].IsColorSet(Color2)) solved = true;
                        }

                        // 5035: Color5 locks in the same column as a Color3 lock are traps. 
                        if ((r5035) && !solved)
                        {
                            foreach (GridSpace tgs in gamegrid)
                            {
                                if ((tgs.column == acol) && (tgs.IsColorSet(Color3))) solved = true;
                            }
                        }
                    }
                }

                if (solved) print("Color5 Accepted at " + position);
                else print("Color5 Rejected at " + position);

                break;
            // End of Color5 Validated Rule slots


            default:
                break;
        }// Switch rln

        if (!solved) gs.RemoveColor(TryColor);
        else gs.SetLock(TryColor);
        return solved; 
    }// ValidatedSlotRule END


    // FlipRuleBool This function just flips the correct boolean for each rule, thus tracking which rules have been chosen 
    void FlipRuleBool(int rn)
    {
        if (rn == 1000) r1000 = true;
        if (rn == 1021) r1021 = true;
        if (rn == 1045) r1045 = true;
        if (rn == 1111) r1111 = true;
        if (rn == 5555) r5555 = true;
        if (rn == 1001) r1001 = true;
        if (rn == 2001) r2001 = true;
        if (rn == 3401) r3401 = true;
        if (rn == 4031) r4031 = true;
        if (rn == 5011) r5011 = true;
        if (rn == 1502) r1502 = true;
        if (rn == 2052) r2052 = true;
        if (rn == 3002) r3002 = true;
        if (rn == 4202) r4202 = true;
        if (rn == 5052) r5052 = true;
        if (rn == 1003) r1003 = true;
        if (rn == 2003) r2003 = true;
        if (rn == 3033) r3033 = true;
        if (rn == 4003) r4003 = true;
        if (rn == 5203) r5203 = true;
        if (rn == 1004) r1004 = true;
        if (rn == 2404) r2404 = true;
        if (rn == 3014) r3014 = true;
        if (rn == 4034) r4034 = true;
        if (rn == 5004) r5004 = true;
        if (rn == 1025) r1025 = true;
        if (rn == 2405) r2405 = true;
        if (rn == 3005) r3005 = true;
        if (rn == 4205) r4205 = true;
        if (rn == 5035) r5035 = true;		
    }




    // This function returns a Color for a given integer
    Color IntToColor(int c)
    {
        if (c == 0) return white;
        if (c == 1) return Color1;
        if (c == 2) return Color2;
        if (c == 3) return Color3;
        if (c == 4) return Color4;
        if (c == 5) return Color5;
        return white;			
    }

    // This function returns an integer that is representative of that color
    int ColorToInt(Color cler)
    {
        if (cler == white) return 0;
        if (cler == Color1) return 1;
        if (cler == Color2) return 2;	
        if (cler == Color3) return 3;	
        if (cler == Color4) return 4; 	
        if (cler == Color5) return 5; 

        return -1;
    }


    // This function returns the appropriate rule in the RuleDatabase, by the input ID number.
    Rule GetRule(int id)
    {
        int x;
        for (x = 0; x < rdatabase.rules.Count; x++)
        {
            if (rdatabase.rules[x].ruleID == id)
            {
                return rdatabase.rules[x];
            }
        }
        return null;
    }

    // This function merely capitalizes the first letter of a string. (I am surprised this doesn't just exist already.)
    string FirstLetterToUpper(string str)
    {
        if (str == null)
            return null;
        if (str.Length > 1)
            return char.ToUpper (str [0]) + str.Substring (1);
        return str.ToUpper ();
    }

    // This function converts an input column, row location to its place on a one dimensional array.
    int ConvertToArrayNum(int col, int row, int Totalcol)
    {
        // This assumes col and row are in 0 to n format
        // and Totalcol is in 1 to n format
        int ans = 0;
        ans = row * Totalcol + col;
        return ans;
    }

    string ColorTextulizer(Color colour)
    {
        if (colour == Color1) return "Color1";
        if (colour == Color2) return "Color2";
        if (colour == Color3) return "Color3";
        if (colour == Color4) return "Color4";
        if (colour == Color5) return "Color5";
        return "Color not recognized";
    }



}