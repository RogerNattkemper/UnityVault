using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/// <summary>
///  Title Set Up
///  Controls the Player Name Dropdown display and Player HighScore
/// </summary>
public class TitleSetUp : MonoBehaviour
{
    public static int MaxPlayerAccounts = 10;
    Dropdown PlayerDropDown;
    Text PlayerScore;
    List<string> PlayerList;
    string nameHolder;
    int score;

    GameObject NewPlayerPanel;
    GameObject DelPlayerPanel;
    GameObject MessageDialogue;
    GameObject DeleteConfirmation;
    GameObject VaultDoor;

    // Use this for initialization
    void Start ()
    {
        NewPlayerPanel = this.transform.FindChild("New Player Dialogue").gameObject;
        NewPlayerPanel.SetActive(false);

        MessageDialogue = this.transform.FindChild("Message Dialogue").gameObject;
        MessageDialogue.SetActive(false);

        DelPlayerPanel = this.transform.FindChild("Delete Player Dialogue").gameObject;
        DelPlayerPanel.SetActive(false);

        DeleteConfirmation = this.transform.FindChild("Delete Confirmation").gameObject;
        DeleteConfirmation.SetActive(false);

        PlayerDropDown = this.transform.FindChild("Player Name DropDown").GetComponent<Dropdown>();
        PlayerList = GlobalControl.control.GetPlayerNames();
        PlayerScore = this.transform.FindChild("Player Score Display").GetComponentInChildren<Text>();

        if (PlayerList.Count == 0) PlayerList.Add("");        
        PlayerList.Add("[NEW PLAYER]");
        PlayerList.Add("[DELETE PLAYER]");
        nameHolder = GlobalControl.control.PlayerName;

        PlayerDropDown.value = 0;      

        //Set up the Dropdown list
        PlayerDropDown.ClearOptions();
        PlayerDropDown.AddOptions(PlayerList);
        if (PlayerDropDown == null) print("Not found!!");
    }

    //Send a message to PLayer through the message Dialogue
    public void SendNotice(Color color, string msg)
    {
        MessageDialogue.SetActive(true);
        Text txt = MessageDialogue.transform.FindChild("Text").GetComponent<Text>();
        txt.color = color;
        txt.text = msg;
    }

    // Handles the Close Button on the Message Dialogue
    public void MessageClose()
    {
        // Close the notification
        MessageDialogue.SetActive(false);
    }

    public void DropDownChange()
    {
        string temp = PlayerDropDown.transform.FindChild("Label").GetComponent<Text>().text;

        // If the new Value is "New Player" open New Player dialogue
        if (temp == "[NEW PLAYER]")  
        {
            if (PlayerList.Count < MaxPlayerAccounts)
            {
                EnterNewPlayer(Color.black, "Enter a new player name.");
            }
            else // Open notification that no more players can be created
            {
                PlayerDropDown.value = PlayerList.IndexOf(nameHolder);
                SendNotice(Color.red, "The maximum allowed players have been entered./n To add a new player, you will have to delete one.");
            }
            
        }
        // If the new Value is "Delete Player" open Delete Player dialogue
        else if (temp == "[DELETE PLAYER]")
        {
            DeletePlayerDialogue();            
        }

        // If the new Value is a player, change the current Player in Globals, and change all the default settings
        else
        {
            if (PlayerList.Contains(temp))
            {
                GlobalControl.control.ChangePlayer(temp);
                nameHolder = temp;
            }
            else
            {
                print("Somehow selected something not available");
            }
        }        
    }

    public void RefreshTitle()
    {
        //Select the default player if there is one
        int newValue = 0;
        PlayerList = GlobalControl.control.GetPlayerNames();
        PlayerDropDown = this.transform.FindChild("Player Name DropDown").GetComponent<Dropdown>();
        if (PlayerDropDown == null) print("NO Player Drop DOwn found");
        PlayerDropDown.options.Clear();
                
        // IF there are any players saved
        if (PlayerList.Count > 0)
        {
            // If there is a default player
            if (GlobalControl.control.GetCurrentPlayer() != null)
            {
                nameHolder = GlobalControl.control.GetCurrentPlayer().PlayerName;
                newValue = PlayerList.IndexOf(nameHolder);
            }
            // Or if the Default Player doesn't exist
            else
            {
                nameHolder = PlayerList[0];
                print("Default player didn't exist");
                GlobalControl.control.ChangePlayer(nameHolder);
            }
                      
            score = GlobalControl.control.GetCurrentPlayer().GetScore();
            PlayerScore = this.transform.FindChild("Player Score Display").GetComponentInChildren<Text>();
            PlayerScore.text = "High Score: " + score.ToString();
        }
        //IF there are no Players at all, add a blank
        else
        {
            PlayerList.Add("");
            nameHolder = "";
            PlayerScore.text = "";
        }

        PlayerList.Add("[NEW PLAYER]");
        PlayerList.Add("[DELETE PLAYER]");
        PlayerDropDown.AddOptions(PlayerList);
        PlayerDropDown.value = newValue;
    }

    public void EnterNewPlayer(Color color, string msg)
    {
        NewPlayerPanel.SetActive(true);
        NewPlayerPanel.transform.FindChild("InputField").FindChild("Text").GetComponent<Text>().text = "";
        Text message = NewPlayerPanel.transform.FindChild("Messages").GetComponent<Text>();
        message.color = color;
        message.text = msg;
    }

    public void NewNameEntered()
    {
        GameObject IF = NewPlayerPanel.transform.FindChild("InputField").gameObject;
        Text newname = IF.transform.FindChild("Text").GetComponent<Text>();
        Text message = NewPlayerPanel.transform.FindChild("Messages").GetComponent<Text>();

        // If player enters a blank, close the dialogue and do nothing
        if (newname.text == "")
        {            
            NewPlayerPanel.SetActive(false);
            RefreshTitle();
        }
        // else if they enter a name that already exists
        else if (PlayerList.Contains(newname.text))
        {
            message.color = Color.red;
            message.text = "Player name already exists, enter a different name.";
            newname.text = "";
        }
        else
        {           
            GlobalControl.control.AddNewPlayer(newname.text);
            NewPlayerPanel.SetActive(false);
        }

        InputField input = IF.GetComponent<InputField>();
        input.text = string.Empty;
    }

    void DeletePlayerDialogue()
    {
        DelPlayerPanel.SetActive(true);
        Dropdown DelDropDown = DelPlayerPanel.transform.FindChild("Player List DropDown").GetComponent<Dropdown>();

        // Get the Player List (sans the New Player/Delete Player)
        List<string> playahs = GlobalControl.control.GetPlayerNames();
        // Fill in the drop down options, add a blank option at 0 and set value to that
        playahs.Insert(0, "");
        DelDropDown.options.Clear();
        DelDropDown.AddOptions(playahs);
        DelDropDown.value = 0;
    }

    // This is called when the player selects a name from the player drop down list to delete
    public void DelValueChanged()
    {
        Dropdown DelDropDown = DelPlayerPanel.transform.FindChild("Player List DropDown").GetComponent<Dropdown>();
        if (DelDropDown.value > 0)
        {
            string temp = DelDropDown.transform.FindChild("Label").GetComponent<Text>().text;
            string message = "Please confirm deletion of player:\n" + temp;

            DelDropDown.value = 0;
            DelPlayerPanel.SetActive(false);

            DeleteConfirmation.SetActive(true);
            DeleteConfirmation.transform.FindChild("Are you sure?").GetComponent<Text>().text = message;

            nameHolder = temp;
        }    
    
    // NOTE ON DROPDOWN LISTS - YOu have to destroy the automatically instantiated "Dropdown List" between uses, OR 
    // another "Dropdown List" is instatiated under that, and that one is shown (which is not filled out)
        Destroy(DelDropDown.transform.FindChild("Dropdown List").gameObject);
    }

    public void DeleteConfirmed()
    {        
        GlobalControl.control.DeletePlayer(nameHolder);
        DeleteConfirmation.SetActive(false);
    }


    public void DeleteCancelled()
    {
        DeleteConfirmation.SetActive(false);
        RefreshTitle();
    }

    public void PlayPushed()
    {
        //If there is at least one player
        List<string> temp = GlobalControl.control.GetPlayerNames();
        if (temp.Count > 0)
        {
            //If there is no default player
            if (GlobalControl.control.GetCurrentPlayer() == null) SendNotice(Color.black, "Please select one of the Player names first");
            else this.transform.GetComponentInParent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.SelectionScreen);
        }
        else  EnterNewPlayer(Color.black, "You must enter a player name to play");
    }

    public void SettingPushed()
    {
        //If there is at least one player
        List<string> temp = GlobalControl.control.GetPlayerNames();
        if (temp.Count > 0)
        {
            //If there is no default player
            if (GlobalControl.control.GetCurrentPlayer() == null) SendNotice(Color.black, "Please select one of the Player names first");
            else this.transform.GetComponentInParent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.SettingScreen);
        }
        else EnterNewPlayer(Color.black, "You must enter a player name to change settings");        
    }

    public void ShopPushed()
    {
        //If there is at least one player
        List<string> temp = GlobalControl.control.GetPlayerNames();
        if (temp.Count > 0)
        {
            //If there is no default player
            if (GlobalControl.control.GetCurrentPlayer() == null) SendNotice(Color.black, "Please select one of the Player names first");
            // Go to the Game Selection Screen
            else this.transform.GetComponentInParent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.ShopScreen);
        }
        else EnterNewPlayer(Color.black, "You must enter a player name to Shop!"); 
    }

    public void VaultPushed()
    {
        if (GlobalControl.control.GetCurrentPlayer() != null) SceneManager.LoadScene("PlayerVault");
    }

}
