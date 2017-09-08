using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the storage class for player data
[System.Serializable]
public class PlayerClass
{
    public string PlayerName;
    // Level Access Control
    private bool TestAccess;
    private int PCDiff;
    private int RMDiff;
    private int RichDiff;
    private int RansomDiff;
    //private GlobalControl.VaultPackage LastGame;
    private int Score; // Player's Highest Vault score
    private int Money; // How much money the player has earned
    private int KeyHints; // How many KeyHints the Player has
    private int Repair; // How many lock Repairs the player has on hand
    private int TimeExt; // How many 5 Second Time extension the player has
    private int Reveal; // How many Reveals the player has
    private List<int> treasureMaps; // List of all the Treasure IDs that can be played.
    private List<int> treasures; // List of Treasures IDs player has acquired

    private bool MusicOn; //
    private bool SoundOn;
    private float MusicVol;
    private float SoundVol;
    private bool isRightHanded;
    
    public void SetNew(string name)
    {
        PlayerName = name;

        // Game Related items
        if ((name == "Roger") || (name == "TestMaster"))
        {
            TestAccess = true;
            PCDiff = 1;
            RichDiff = 1;
            RMDiff = 1;
            RansomDiff = 1;
        }
        else
        {
            TestAccess = false;
            PCDiff = 1;
            RichDiff = 0;
            RMDiff = 0;
            RansomDiff = 0;
        }

        //LastGame = GlobalControl.VaultPackage.PocketChange_Easy;
        Score = 0;
        Money = 0;
        KeyHints = 0;
        Repair = 0;
        TimeExt = 0;
        Reveal = 0;
        treasureMaps = new List<int>();
        treasures = new List<int>();

        // Setting 
        MusicOn = true;
        SoundOn = true;
        MusicVol = 1;
        SoundVol = 1;
        isRightHanded = true;     
    }


    // All the Get/Set functions to access the private vars
    public bool HasTestAccess() { return TestAccess; }
    //public void SetTestAccess(bool set) { TestAccess = set; }
    public int PocketDiff() { return PCDiff; }
    public void SetPocketDiff(int set) { PCDiff = set; }
    public int HasRMAccess() { return RMDiff; }
    public void SetRMAccess(int set) { RMDiff = set; }
    public int HasRichAccess() { return RichDiff; }
    public void SetRichAccess(int set) { RichDiff = set; }
    public int HasRansomAccess() { return RansomDiff; }
    public void SetRansomAccess(int set) { RansomDiff = set; }
    //public GlobalControl.VaultPackage GetLastGame() { return LastGame; }
    //public void SetLastGame(GlobalControl.VaultPackage pkg) { LastGame = pkg; }
    public int GetScore() { return Score; }
    public void AddtoScore(int num) { Score += num; }
    public void SetMoney(int num) { Money = num; }
    public int GetMoney() { return Money; }
    public bool CanAfford(int cash) { return (Money >= cash); }
    public void Charge(int cash) { Money -= cash; }
    public void Pay(int cash) { Money += cash; }
    public int GetKeyHints() { return KeyHints; }
    public void AddKeyHint() { KeyHints++; }
    public void SubtractKeyHint() { KeyHints--; }
    public int GetRepairs() { return Repair; }
    public void AddRepair() { Repair++; }
    public void SubtractRepair() { Repair--; }
    public int GetTimeExt() { return TimeExt; }
    public void AddTimeExt() { TimeExt++; }
    public void SubtractTimeExt() { TimeExt--; }
    public int GetReveals() { return Reveal; }
    public void AddReveal() { Reveal++; }
    public void SubtractReveal() { Reveal--; }

    // You can only have one treasure of a type
    // If the player already has that treasure ID, as a map or treasure, this will return false;
    public bool GetNewMap(int id)
    {
        if (treasureMaps.Contains(id) || treasures.Contains(id)) return false;
        treasureMaps.Add(id);
        return true;
    }

    // This moves the treasure from Player's Maps to the treasure
    public void AcquireTreasure(int id)
    {
        treasureMaps.Remove(id);
        treasures.Add(id);
    }

    //Get/Set for the Game Settings
    public void SetMusic (bool set) { MusicOn = set; }
    public bool GetMusic () { return MusicOn; }
    public void SetSound (bool set) { SoundOn = set; }
    public bool GetSound () { return SoundOn; }
    public void SetMusicVol (float vol) { MusicVol = vol; }
    public float GetMusicVol() { return MusicVol; }
    public void SetSoundVol(float vol) { SoundVol = vol; }
    public float GetSoundVol() { return SoundVol; }
    public void SetHanded(bool isRighty) { isRightHanded = isRighty; }
    public bool GetHanded() { return isRightHanded; }

}