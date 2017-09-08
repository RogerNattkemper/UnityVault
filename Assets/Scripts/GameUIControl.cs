using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControl : MonoBehaviour
{
    GameObject GamesUIPanel;
    GameObject RulesPanel;
    GameObject TimerPanel;
    GameObject DefeatPanel;
    GameObject HighScorePanel;
    GameObject VaultDoor;

    Text GameLocksText;
    Text RulesLocksText;
    Text DefuseText;

    LevelDatabase ldatabase;
    GameCreator03 gameMaker;

    KeyUIManager KUM;
    MouseControl MC;

    bool atStart = true;

    int answersLeft;
    int ruleTotal;
    public int defusesLeft;

    // Use this for initialization
    void Awake()
    {
        VaultDoor = this.transform.parent.FindChild("Vault Door").gameObject;
        VaultDoor.SetActive(true);
        VaultDoor.GetComponent<Animator>().SetBool("GameMade", false);
    }

    void Start ()
    {
        ldatabase = GameObject.FindGameObjectWithTag("Level Database").GetComponent<LevelDatabase>();
        gameMaker = GameObject.FindGameObjectWithTag("Game Creator").GetComponent<GameCreator03>();
        MC = GameObject.Find("MouseControl").GetComponent<MouseControl>();

        GamesUIPanel = this.transform.FindChild("Game UI Panel").gameObject;
        RulesPanel = this.transform.FindChild("Rules Panel").gameObject;
        TimerPanel = this.transform.FindChild("Timer Panel").gameObject;
        DefeatPanel = this.transform.FindChild("Defeat Panel").gameObject;
        HighScorePanel = this.transform.FindChild("High Score Notification").gameObject;


        KUM = GamesUIPanel.transform.FindChild("Key Side Panel").GetComponent<KeyUIManager>();

        SetUpGamesPanel(); // FInished

        InstallGadgets();

        Text StartShowButtonText = RulesPanel.transform.FindChild("Start_Show Game Button/Text").GetComponent<Text>();
        StartShowButtonText.text = "Start Game";
        // Set up the Timer	if needed

        //NOTE: Once the GameCreator has finished GameScript should
        // Run 
        // Run "FillInRulesDisplay()"
        // Run "OpenDoorTrigger"

        ruleTotal = 0;



    }

    // This is run by the Main Canvas GameScript after the Game Creator has finished
    public void SetUpFinalUI()
    {
        this.transform.parent.FindChild("Vault Door").GetComponent<Animator>().SetBool("GameMade", true);
        KUM.InitializeKeys();
        MC.SetCursors();
    }

    public void DeactivateVaultDoor()
    {
        this.transform.parent.FindChild("Vault Door").gameObject.SetActive(false);
    }

    void InstallGadgets()
    {
        // From Global Control, determine what gadgets, if any are owned
        PlayerClass player = GlobalControl.control.GetCurrentPlayer();
        Transform GadgetPanel = GamesUIPanel.transform.FindChild("Gadget Side Panel");
        GameObject RevealGO = GadgetPanel.FindChild("Use Reveal Button").gameObject;
        GameObject ExtraTimeGO = GadgetPanel.FindChild("Time Ext Panel").gameObject;
        GameObject KeyHintGO = GadgetPanel.FindChild("Use Key Hint").gameObject;
        GameObject RepairGO = GamesUIPanel.transform.FindChild("Top Panel Display/Repairs Display").gameObject;
        GameObject DefuseGO = GamesUIPanel.transform.FindChild("Top Panel Display/Defuse Display").gameObject;

        if (player.GetRepairs() > 0)
        {
            RepairGO.SetActive(true);
            RepairGO.transform.GetComponent<Text>().text = "Repairs: " + player.GetRepairs();
        }
        else RepairGO.SetActive(false);

        if (GlobalControl.control.Defuses > 0)
        {
            DefuseGO.SetActive(true);
            DefuseText = DefuseGO.transform.GetComponent<Text>();
            DefuseText.text = "Defuses: " + GlobalControl.control.Defuses;
            defusesLeft = GlobalControl.control.Defuses;
        }
        else DefuseGO.SetActive(false);

        if (player.GetReveals() > 0)
        {
            RevealGO.SetActive(true);
            RevealGO.transform.FindChild("Text").GetComponent<Text>().text = "Reveals: " + player.GetReveals();
        }
        else RevealGO.SetActive(false);

        if (GlobalControl.control.Timer && player.GetTimeExt() > 0)
        {
            ExtraTimeGO.SetActive(true);
            ExtraTimeGO.GetComponent<Text>().text = "Extra Time: " + player.GetTimeExt();
        }
        else ExtraTimeGO.SetActive(false);

        if (player.GetKeyHints() > 0)
        {
            KeyHintGO.SetActive(true);
            KeyHintGO.transform.FindChild("Text").GetComponent<Text>().text = "Key Hints: " + player.GetKeyHints();
        }
        else ExtraTimeGO.SetActive(false);
    }

    // RUN EVERY NEW LEVEL
    public void RefreshGameData(int NumOfAnswers, int NumOfRules)
    {
        answersLeft = NumOfAnswers;
        ruleTotal += NumOfRules;

        Transform GameInfo = GamesUIPanel.transform.FindChild("Top Panel Display");
        Text GameLevelText = GameInfo.FindChild("Levels Remaining Display").GetComponent<Text>();
        Text GameLocksText = GameInfo.FindChild("Unsolved Locks Display").GetComponent<Text>();

        RulesPanel.SetActive(true);
        Transform RulesInfo = RulesPanel.transform.FindChild("Top Panel Display");
        Text RulesTotalText = RulesPanel.transform.FindChild("Rules Total").GetComponent<Text>();
        Text RulesLevelText = RulesInfo.FindChild("Levels Remaining Display").GetComponent<Text>();
        Text RulesLocksText = RulesInfo.FindChild("Unsolved Locks Display").GetComponent<Text>();
        
        // This is run every new level

        //Update Data on the Game Display
        answersLeft = GameDisplay.answerKey.Count;

        GameLevelText.text = "Level " + (GameDisplay.levelnumber + 1).ToString() + " / " + GlobalControl.control.NumOfLevels.ToString();
        GameLocksText.text = "Locks Left: " + answersLeft.ToString();
        RulesTotalText.text = "Current Rules: " + ruleTotal;
        RulesLevelText.text = "Level " + (GameDisplay.levelnumber + 1).ToString() + " / " + GlobalControl.control.NumOfLevels.ToString();
        RulesLocksText.text = "Locks Left: " + NumOfAnswers.ToString();
    }

    public void InitTimer()
    {
        if (GlobalControl.control.Timer)
        {
            TimerPanel.SetActive(true);
            //Set time
        }
        else TimerPanel.SetActive(false);
    }

    // Called when a lock is solved
    public void LockSolved()
    {
        answersLeft--;
        if (answersLeft > 0)
        {
            GameLocksText.text = "Locks Left: " + answersLeft.ToString();
            RulesLocksText.text = "Locks Left: " + answersLeft.ToString();
        }
    }

    //Defuses are used automatically, and will prevent a non-answer lock that was guessed incorrectly
    // from ending the game. The GUC 
    public void UseDefuse()
    {
        defusesLeft--;
        DefuseText.text = "Defuses: " + defusesLeft.ToString();
    }

    // Need to create Key Buttons here 
    // Comes up with "Can't Instantiate error
    // I'll probably scrap this and go with Activate/Deactive and Set Order of GameObjects instead
    //   == As Instantiation causes other problems 


    void SetUpGamesPanel()
    {
        RectTransform KeyPanelRT = GamesUIPanel.transform.FindChild("Key Side Panel").GetComponent<RectTransform>();
        RectTransform GadgetPanelRT = GamesUIPanel.transform.FindChild("Gadget Side Panel").GetComponent<RectTransform>();

        if (GlobalControl.control.isRighty)
        {
            KeyPanelRT.anchorMin = new Vector2(0.85f, 0);
            KeyPanelRT.anchorMax = new Vector2(1, 1);

            GadgetPanelRT.anchorMin = new Vector2(0, 0);
            GadgetPanelRT.anchorMax = new Vector2(0.15f, 1);
        }
        else
        {
            GadgetPanelRT.anchorMin = new Vector2(0.85f, 0);
            GadgetPanelRT.anchorMax = new Vector2(1, 1);

            KeyPanelRT.anchorMin = new Vector2(0, 0);
            KeyPanelRT.anchorMax = new Vector2(0.15f, 1);
        }
    }

    /////////////////////// BUtton FUnctions  ////////////////////////////////////////
    public void Start_ShowButton()
    {
        if (atStart)
        {
            Text StartShowButtonText = RulesPanel.transform.FindChild("Start_Show Game Button/Text").GetComponent<Text>();
            StartShowButtonText.text = "Show Game";
            atStart = false;
            //StartTimer()            
        }
        RulesPanel.SetActive(false);
        MC.SetRaycast(true);
    }

    public void ShowRulesButton()
    {
        MC.SetRaycast(false);
        RulesPanel.SetActive(true);
    }
}
