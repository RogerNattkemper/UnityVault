using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VaultTitle : MonoBehaviour {

   public static bool ShowStopper;

   void Start()
   {
        /*
       if (GlobalControl.control.Difficulty == GlobalControl.Setting.Easy) EasySetting();
       if (GlobalControl.control.Difficulty == GlobalControl.Setting.Medium) MediumSetting();
       if (GlobalControl.control.Difficulty == GlobalControl.Setting.Hard) HardSetting();
       if (GlobalControl.control.Difficulty == GlobalControl.Setting.Custom) CustomSetting(); 
       */
   }

    void EndGame()
    {
        Application.Quit();
    }

    void OptionsClicked()
    {
        SceneManager.LoadScene("PC Vault Options");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PC Game Screen");
    }

    void NewGame()
    { 
        // Check to see if there is room in the save list
        // If not Load VaultSaves, with a panel asking if they want to delete anyone
        // Else pop up a panel asking for the new player's name.
        // Save name to GlobalControl, and open a new file with that name (Close it too)
        print("New Game Pressed");
    }

    /*
    void EasyPressed()
    { 
        //At new game default is Easy
        //Everytime they click on the button it will cycle Easy -> Medium -> Hard
        // If they go into Options, AND change the settings, this will display Custom
        GameObject.SetActive(false);

    }
    */
    void InfoClicked()
    {
        SceneManager.LoadScene("VaultTitle");
        print("Info button pressed");
    }

    /*

    void EasySetting()
    {
        GlobalControl.control.Difficulty = GlobalControl.Setting.Easy;
        GlobalControl.control.initialrules = 1;
        GlobalControl.control.rulesperlevel = 1;
        GlobalControl.control.NumOfLevels = 1;
        GlobalControl.control.initialrowsize = 3;
        GlobalControl.control.initialcolumnsize = 3;
        GlobalControl.control.GridExpansionRate = 3;
        GlobalControl.control.TimerPerLevel = false;
        GlobalControl.control.TimerPerVault = false;
        GlobalControl.control.MaxLevelTime = 0;
        GlobalControl.control.MaxVaultTime = 0;
    }

    void MediumSetting()
    {
        GlobalControl.control.Difficulty = GlobalControl.Setting.Medium;
        GlobalControl.control.initialrules = 2;
        GlobalControl.control.rulesperlevel = 2;
        GlobalControl.control.NumOfLevels = 6;
        GlobalControl.control.initialrowsize = 3;
        GlobalControl.control.initialcolumnsize = 3;
        GlobalControl.control.GridExpansionRate = 2;
        GlobalControl.control.TimerPerLevel = true;
        GlobalControl.control.TimerPerVault = false;
        GlobalControl.control.MaxLevelTime = 1;
        GlobalControl.control.MaxVaultTime = 0;
    }

    void HardSetting()
    {
        GlobalControl.control.Difficulty = GlobalControl.Setting.Hard;
        GlobalControl.control.initialrules = 3;
        GlobalControl.control.rulesperlevel = 2;
        GlobalControl.control.NumOfLevels = 10;
        GlobalControl.control.initialrowsize = 4;
        GlobalControl.control.initialcolumnsize = 4;
        GlobalControl.control.GridExpansionRate = 1;
        GlobalControl.control.TimerPerLevel = false;
        GlobalControl.control.TimerPerVault = true;
        GlobalControl.control.MaxLevelTime = 0;
        GlobalControl.control.MaxVaultTime = 10;
    }

    void CustomSetting()
    { 
        //Nothing yet
    }
    */


}
