using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;                          // Basic xml attributes
using System.Xml.Serialization;             // access xml serializer
using System.Linq;
using System.Text;

/*******************************************************************************************************
 * Game Setting Functions
 * 
 * void SaveGameSettings()
 * void LoadGameSettings()
 * void SetDefaults()
 * void SetGame()
 * void SetGame(GameSettings GS)
 * void SetGame(VaultPackage VP)
 * void PackageSet(int ir, int rpl, int nol, int irs, int ics, int air, int ger, int dif, bool tOn, bool c1, bool c2, bool c3, bool c4, bool c5)
 * int TimerCalc()
 * string PackageDescription(VaultPackage VP)
 ******************************************************************************************************/
/*******************************************************************************************************
 * Player Account Functions
 * 
 *  List<string> GetPlayerNames()
 *  PlayerClass GetCurrentPlayer()
 *  void AddNewPlayer(string)
 *  void DeletePlayer(string)
 *  void ChangePlayer(string)
 *  void LoadPlayerData()
 *  void SavePlayerData()
 *  void SetPlayerDefaults()
 ******************************************************************************************************/
/*******************************************************************************************************
* Class GameSettings
    public String SeedValue;
    public bool SeedUsed;
    public int InitialRules;
    public int RulesPerLevel;
    public List<int> SelectedRules;
    public int NumOfLevels; // How many levels selected?     
    public int InitialRowSize;
    public int InitialColumnSize;
    public int AnswerIncreaseRate;
    public int GridExpansionRate;
    public int DefuseAmount;  // The amount of traps player can trigger without Defeat
    public bool TimerOn; //Is the Vault Attempt Timed? (Time is calculated by difficult of levels) 
    public int TimerValue; // 
    public bool C1Key;
    public bool C2Key;
    public bool C3Key;
    public bool C4Key;
    public bool C5Key;
******************************************************************************************************/

public class GlobalControl : MonoBehaviour 
{
    public static GlobalControl control;

    public List<PlayerClass> players;

    // Player Save info
    private PlayerClass currentPlayer;
    // PlayerPrefs default player
    public string PlayerName; 

    public string seedvalue;
    public bool seedUsed;
    public int initialrules;    
    public int rulesperlevel;
    public List<int> selectedRules;  
    public int NumOfLevels; // How many levels selected?     
    public bool ShowStopper = false;    
    public int initialrowsize;       
    public int initialcolumnsize;
    public int AnswerIncreaseRate; 
    public int GridExpansionRate;
    public int Defuses;  // The amount of traps player can trigger without Defeat
    public bool Timer; //Is the Vault Attempt Timed? (Time is calculated by difficult of levels)    
    public int TimerValue;
    public bool Color1Key = true;
    public bool Color2Key = true;
    public bool Color3Key = true;
    public bool Color4Key = true;
    public bool Color5Key = true;
    public bool MusicOn;
    public bool SoundOn;
    public float MusicVol;
    public float SoundVol;
    public bool isRighty;

    GameSettings currentSettings;
    public string GameDescription;

    GameSettings TestInit;
    GameSettings PC_Easy;
    GameSettings PC_Medium;
    GameSettings PC_Hard;
    GameSettings RM_Easy;
    GameSettings RM_Medium;
    GameSettings RM_Hard;
    GameSettings Rich_Easy;
    GameSettings Rich_Medium;
    GameSettings Rich_Hard;
    GameSettings King_Easy;
    GameSettings King_Medium;
    GameSettings King_Hard;

    // The Vault Package determines both what is in the Vault, and 
    // the difficulty level of the Vault
    public enum VaultPackage
    {
        Test,
        PocketChange_Easy,
        PocketChange_Medium,
        PocketChange_Hard,
        RealMoney_Easy,
        RealMoney_Medium,
        RealMoney_Hard,
        Riches_Easy,
        Riches_Medium,
        Riches_Hard,
        KingsTreasure_Easy,
        KingsTreasure_Medium,
        KingsTreasure_Hard,
        PricelessRelic
    };

    void Awake()
    {

        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }

        else if (control != this)
        {
            Destroy(gameObject);
        }

        // ir, rpl, nol, irs, ics, air, ger, dif,   tOn,    c1,    c2,    c3,    c4,    c5)
        TestInit = new GameSettings(1, 1, 1, 3, 3, 0, 3, 3, false, true, true, true, true, true);
        PC_Easy = new GameSettings(1, 1, 3, 3, 3, 0, 3, 3, false, true, true, true, false, false);
        PC_Medium = new GameSettings(1, 1, 4, 3, 3, 0, 3, 2, false, true, true, true, false, false);
        PC_Hard = new GameSettings(2, 1, 5, 4, 4, 1, 4, 1, false, true, true, true, false, false);
        RM_Easy = new GameSettings(2, 1, 5, 4, 4, 1, 2, 2, false, true, true, true, true, false);
        RM_Medium = new GameSettings(2, 2, 6, 5, 5, 2, 3, 1, false, true, true, true, true, false);
        RM_Hard = new GameSettings(2, 2, 7, 5, 5, 2, 4, 0, false, true, true, true, true, false);
        Rich_Easy = new GameSettings(3, 2, 7, 4, 4, 3, 3, 1, false, true, true, true, true, true);
        Rich_Medium = new GameSettings(3, 3, 8, 5, 5, 3, 4, 0, false, true, true, true, true, true);
        Rich_Hard = new GameSettings(3, 3, 9, 5, 5, 3, 5, 0, false, true, true, true, true, true);
        King_Easy = new GameSettings(3, 2, 9, 3, 3, 4, 2, 0, true, true, true, true, true, true);
        King_Medium = new GameSettings(3, 3, 10, 4, 4, 4, 3, 0, true, true, true, true, true, true);
        King_Hard = new GameSettings(3, 3, 11, 5, 5, 4, 4, 0, true, true, true, true, true, true);

        // Load the Player List
        LoadPlayerData();
        LoadGameSettings();

        // Set the current player to the Default Player
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string name = PlayerPrefs.GetString("PlayerName");
            if (name != null) ChangePlayer(name);
        }
        else
        {
            currentPlayer = null;
            print("PlayerName PlayerPref not found");
        }


        if (GameObject.Find("Canvas/Vault Background") != null)
        {
            GameObject.Find("Canvas/Vault Background").GetComponent<TitleSetUp>().RefreshTitle();
        }
    }

    /*******************************************************************************************************
    * Game Setting Functions
    * 
    * void SaveGameSettings()
    * void LoadGameSettings()
    * void SetDefaults()
    * void SetGame(GameSettings GS)
    * void SetGame(VaultPackage VP)

    * int TimerCalc()
    ******************************************************************************************************/


    // This saves the current Global Control GameSettings.
    // This is called when the player selects Play or Back from the Test Control Center
    public void SaveGameSettings()
    {
        GameSettings GS = new GameSettings(seedvalue, seedUsed, initialrules, rulesperlevel, selectedRules, NumOfLevels, initialrowsize, initialcolumnsize, AnswerIncreaseRate, GridExpansionRate, Defuses, Timer, Color1Key, Color2Key, Color3Key, Color4Key, Color5Key );        

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameSettings.dat", FileMode.OpenOrCreate);
        bf.Serialize(file, GS);
        file.Close();
    }


    // This loads the TestClass of the last test game, and sets the GlobalControls appropriately. Called when
    // Player selects the Test Control Center in the Game Select
    public void LoadGameSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSettings.dat", FileMode.Open);
            currentSettings = (GameSettings)bf.Deserialize(file);
            file.Close();
            SetGame(currentSettings);
        }
        else
        {
            print("Setting to Test Init");
            SetGame(TestInit);
        }       
    }

    void SetGame(GameSettings GS)
    {
        if (GS == null) print("YOu sent me nOTHING!!");
        seedvalue = GS.SeedValue;
        seedUsed = GS.SeedUsed;
        initialrules = GS.InitialRules;
        rulesperlevel = GS.RulesPerLevel;
        selectedRules = GS.SelectedRules;
        NumOfLevels = GS.NumOfLevels;
        initialrowsize = GS.InitialRowSize;
        initialcolumnsize = GS.InitialColumnSize;
        AnswerIncreaseRate = GS.AnswerIncreaseRate;
        GridExpansionRate = GS.GridExpansionRate;
        Defuses = GS.DefuseAmount;
        Timer = GS.TimerOn;
        Color1Key = GS.C1Key;
        Color2Key = GS.C2Key;
        Color3Key = GS.C3Key;
        Color4Key = GS.C4Key;
        Color5Key = GS.C5Key;
    }

    public void SetGame(VaultPackage VP)
    {
        currentSettings = GetSetting(VP);
        SetGame(currentSettings);
    }

    public GameSettings GetSetting(VaultPackage VP)
    {     
        switch (VP)
        {
            case VaultPackage.PocketChange_Easy: return PC_Easy;
            case VaultPackage.PocketChange_Medium: return PC_Medium;
            case VaultPackage.PocketChange_Hard: return PC_Hard;
            case VaultPackage.RealMoney_Easy: return RM_Easy;
            case VaultPackage.RealMoney_Medium: return RM_Medium;
            case VaultPackage.RealMoney_Hard: return RM_Hard;
            case VaultPackage.Riches_Easy: return Rich_Easy;
            case VaultPackage.Riches_Medium: return Rich_Medium;
            case VaultPackage.Riches_Hard: return Rich_Hard;
            case VaultPackage.KingsTreasure_Easy: return King_Easy;
            case VaultPackage.KingsTreasure_Medium: return King_Medium;
            case VaultPackage.KingsTreasure_Hard: return King_Hard;
            default: return TestInit;
        }
    }

    int TimerCalc()
    {
        /// I need to fill this with calculations dependent on game settings
        return 100;
    }

/*******************************************************************************************************
 * Player Account Functions
 * 
 *  List<string> GetPlayerNames()
 *  PlayerClass GetCurrentPlayer()
 *  void AddNewPlayer(string)
 *  void DeletePlayer(string)
 *  void ChangePlayer(string)
 *  void LoadPlayerData()
 *  void SavePlayerData()
 *  void SetPlayerDefaults()
 ******************************************************************************************************/  
    public List<string> GetPlayerNames()
    {
        List<string> names = new List<string>();
        foreach (PlayerClass pc in players)
        {
            names.Add(pc.PlayerName);
        }

        return names;
    }
    //----------------------------------

    public PlayerClass GetCurrentPlayer()
    {
        return currentPlayer;
    }
    //----------------------------------

    public void AddNewPlayer(string name)
    { 
        // Grap the TItleSetup
        //PlayerClass newplayer = gameObject.AddComponent<PlayerClass>();
        PlayerClass newplayer = new PlayerClass();
        newplayer.SetNew(name);
        players.Add(newplayer);
        ChangePlayer(name);
        foreach (PlayerClass pc in players) print(pc.PlayerName);
        SavePlayerData();
        GameObject.Find("Canvas").transform.FindChild("Vault Background").GetComponent<TitleSetUp>().RefreshTitle();
    }
    //----------------------------------

    public void DeletePlayer(string name)
    {
        PlayerClass toBeDeleted = null;
        
        foreach (PlayerClass pc in players)
        {
            if (pc.PlayerName == name) toBeDeleted = pc;
        }

        if (toBeDeleted != null)
        {
            // If Deleting the Default Player change Default to the highest one on the list, or to nothing if none left
            if (toBeDeleted == currentPlayer)
            {
                // If there is more than one player left    
                if (players.Count > 1)
                {
                    // If the tobedeleted player is the top one, change to the next one down                 
                    if (players.IndexOf(toBeDeleted) == 0) ChangePlayer(players[1].PlayerName);
                    // else change to the top one
                    else ChangePlayer(players[0].PlayerName);
                }
                // else remove the current player, and set the default player to nothing
                else
                {
                    currentPlayer = null;
                    PlayerPrefs.DeleteKey("PlayerName");
                }
            }
            players.Remove(toBeDeleted);
        }                

        GameObject.Find("Canvas").transform.FindChild("Vault Background").GetComponent<TitleSetUp>().RefreshTitle();
    }
    //----------------------------------

    // This changes the current player and saves it as default as 
    // Well as saving the current PLayers list
    public void ChangePlayer(string name)
    {
        foreach (PlayerClass pc in players)
        {
            if (pc.PlayerName == name)
            {
                currentPlayer = pc;
                PlayerPrefs.SetString("PlayerName", name);
                PlayerName = pc.PlayerName;
                SetPlayerDefaults();
                SavePlayerData();
            }
        }
    }
    //----------------------------------

    // This Saves the current List<PlayerClass> players, and saves the current player as the default
    public void SavePlayerData()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerList.dat", FileMode.OpenOrCreate);
        bf.Serialize(file, players);
        file.Close();
        PlayerPrefs.SetString("PlayerName", currentPlayer.PlayerName);
    }
    //----------------------------------

    // This Loads the saved List<PlayerClass> players (And nothing else)
    public void LoadPlayerData()
    {
        print("save file is at " + Application.persistentDataPath + "/playerList.dat");
        if (File.Exists(Application.persistentDataPath + "/playerList.dat"))
        {

            players = new List<PlayerClass>();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerList.dat", FileMode.Open);
            players = (List<PlayerClass>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            PlayerName = "";
        }
    }
    //----------------------------------

    void SetPlayerDefaults()
    {
        MusicOn = currentPlayer.GetMusic();
        SoundOn = currentPlayer.GetSound();
        MusicVol = currentPlayer.GetMusicVol();
        SoundVol = currentPlayer.GetSoundVol();
        isRighty = currentPlayer.GetHanded();
    }

    // This determines the maximum allowable levels in a vault
    //
    public int GetMaxLevels()
    {
        //This factors in several things
        int maxRules = RuleDatabase.RDBcontrol.GetMaxRules();
        if (maxRules == 0) return 0;

        if (rulesperlevel > 0)
        {
            int ans = ((maxRules - initialrules) / rulesperlevel) + 1;
            if ((maxRules - initialrules) % rulesperlevel > 0) ans++;
            return ans;
        }

        return 1;
    }
}

[System.Serializable]
public class GameSettings
{
    public string SeedValue;
    public bool SeedUsed;
    public int InitialRules;
    public int RulesPerLevel;
    public List<int> SelectedRules;
    public int NumOfLevels; // How many levels selected?     
    public int InitialRowSize;
    public int InitialColumnSize;
    public int AnswerIncreaseRate;
    public int GridExpansionRate;
    public int DefuseAmount;  // The amount of traps player can trigger without Defeat
    public bool TimerOn; //Is the Vault Attempt Timed? (Time is calculated by difficult of levels) 
    public bool C1Key;
    public bool C2Key;
    public bool C3Key;
    public bool C4Key;
    public bool C5Key;
    public string Description;

    // This constructor is used to make the package settings 
    public GameSettings(int ir, int rpl, int nol, int irs, int ics, int air, int ger, int dif, bool tOn, bool c1, bool c2, bool c3, bool c4, bool c5)
    {
        SeedValue = "";
        SeedUsed = false;
        InitialRules = ir;
        RulesPerLevel = rpl;
        SelectedRules = new List<int>();
        NumOfLevels = nol; // How many levels selected?     
        InitialRowSize = irs;
        InitialColumnSize = ics;
        AnswerIncreaseRate = air;
        GridExpansionRate = ger;
        DefuseAmount = dif;  // The amount of traps player can trigger without Defeat
        TimerOn = tOn; //Is the Vault Attempt Timed? (Time is calculated by difficult of levels) 
        C1Key = c1;
        C2Key = c2;
        C3Key = c3;
        C4Key = c4;
        C5Key = c5;

        string temp = "";
        temp = NumOfLevels.ToString() + " Levels\n";
        if (TimerOn) temp = temp + "Timer On with  ";
        temp = temp + CountKeys().ToString() + " Keys";
        Description = temp;
    }

    // This constructor is used to load/saving current setting and creating custom Priceless Relics settings (Stored within them)
    public GameSettings(string seed, bool sUsed, int ir, int rpl, List<int> selRules, int nol, int irs, int ics, int air, int ger, int dif, bool tOn, bool c1, bool c2, bool c3, bool c4, bool c5)
    {
        SeedValue = seed;
        SeedUsed = sUsed;
        InitialRules = ir;
        RulesPerLevel = rpl;
        SelectedRules = selRules;
        NumOfLevels = nol; // How many levels selected?     
        InitialRowSize = irs;
        InitialColumnSize = ics;
        AnswerIncreaseRate = air;
        GridExpansionRate = ger;
        DefuseAmount = dif;  // The amount of traps player can trigger without Defeat
        TimerOn = tOn; //Is the Vault Attempt Timed? (Time is calculated by difficult of levels) 
        C1Key = c1;
        C2Key = c2;
        C3Key = c3;
        C4Key = c4;
        C5Key = c5;

        string temp = "";
        if (SeedUsed) temp = "Seed: " + SeedValue + "  ";
        temp = temp + NumOfLevels.ToString() + " Levels\n";
        if (TimerOn) temp = temp + "Timer On with  ";
        temp = temp + CountKeys().ToString() + " Keys";
        Description = temp;
    }

    int CountKeys()
    {
        int num = 0;
        if (C1Key) num++;
        if (C2Key) num++;
        if (C3Key) num++;
        if (C4Key) num++;
        if (C5Key) num++;
        return num;
    }

    public string GetDescription()
    {
        return Description;
    }


}
