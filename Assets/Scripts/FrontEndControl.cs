using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontEndControl : MonoBehaviour
{
    GameObject Title;
    GameObject Shop;
    GameObject Settings;
    GameObject Selectiion;
    GameObject Test;
    GameObject VaultDoor;
    FrontState currentState;
    FrontState nextState;

    public enum FrontState  
    {
        TitleScreen,
        ShopScreen,
        SettingScreen,
        SelectionScreen,
        TestControlCenter
    }

    void Awake()
    {
        Title = this.transform.FindChild("Vault Background").gameObject;
        Shop = this.transform.FindChild("Shop Background").gameObject;
        Settings = this.transform.FindChild("Settings Background").gameObject;
        Selectiion = this.transform.FindChild("Game Selection").gameObject;
        Test = this.transform.FindChild("Test Control Center").gameObject;
        VaultDoor = this.transform.FindChild("Vault Door").gameObject;

        currentState = FrontState.ShopScreen;
        nextState = FrontState.TitleScreen;
    }

    void Update()
    {
        if (currentState != nextState)
        {
            ClearAll();
            switch (nextState)
            {
                case FrontState.TitleScreen:
                    {
                        Title.SetActive(true);
                        Title.GetComponent<TitleSetUp>().RefreshTitle();
                        break;
                    }
                case FrontState.SelectionScreen:
                    {
                        Selectiion.SetActive(true);
                        Selectiion.GetComponent<GameSelectionControl>().RefreshSelector();
                        break;
                    }
                case FrontState.SettingScreen:
                    {
                        Settings.SetActive(true);
                        Settings.GetComponent<SettingControl>().RefreshSettings();
                        break;
                    }
                case FrontState.ShopScreen:
                    {
                        Shop.SetActive(true);
                        break;
                    }
                case FrontState.TestControlCenter:
                    {
                        Test.SetActive(true);
                        break;
                    }
                default: break;

            }

            currentState = nextState;
        }
    }

    void ClearAll()
    {
        Title.SetActive(false);
        Shop.SetActive(false);
        Settings.SetActive(false);
        Selectiion.SetActive(false);
        Test.SetActive(false);
        VaultDoor.SetActive(false);
    }

    public void SetNextState(FrontState state)
    {
        nextState = state;
    }

    public void BackPushed()
    {
        SetNextState(FrontState.TitleScreen);        
    }

    public void TestConBackPushed()
    {
        //Save Game and PLayer info
        GlobalControl.control.SaveGameSettings();
        GlobalControl.control.SavePlayerData();

        //Change State
        SetNextState(FrontState.TitleScreen);
    }

    public void PlayGame()
    {
        //Save Game and PLayer info
        GlobalControl.control.SaveGameSettings();
        GlobalControl.control.SavePlayerData();

        // Run Door Closing animation
        SceneManager.LoadSceneAsync("VaultGame");
        //SceneManager.LoadScene("VaultGame");
    }

    public void CloseDoor()
    {
        VaultDoor.SetActive(true);
        Animator anim = VaultDoor.GetComponent<Animator>();
        anim.Play("V4 Close");
    }
}
