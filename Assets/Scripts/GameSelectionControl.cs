using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSelectionControl : MonoBehaviour
{
    Transform Content;
    GameObject TestControlCenter;
    GameObject PocketChange;
    Text Pocket_Text;
    Slider Pocket_Slider;
    GameObject RealMoney;
    Text Money_Text;
    Slider Money_Slider;
    GameObject Riches;
    Text Rich_Text;
    Slider Rich_Slider;
    GameObject KingsRansom;
    Text King_Text;
    Slider King_Slider;


    void Initialize()
    {
        Content = transform.FindChild("Scroll View/Viewport/Content");
        TestControlCenter = Content.FindChild("Test Control Panel").gameObject;
        PocketChange = Content.FindChild("Pocket Change").gameObject;
        Pocket_Text = PocketChange.transform.FindChild("Description").GetComponent<Text>();
        Pocket_Slider = PocketChange.transform.FindChild("Slider").GetComponent<Slider>();
        RealMoney = Content.FindChild("Real Money").gameObject;
        Money_Text = RealMoney.transform.FindChild("Description").GetComponent<Text>();
        Money_Slider = RealMoney.transform.FindChild("Slider").GetComponent<Slider>();
        Riches = Content.FindChild("Riches").gameObject;
        Rich_Text = Riches.transform.FindChild("Description").GetComponent<Text>();
        Rich_Slider = Riches.transform.FindChild("Slider").GetComponent<Slider>();
        KingsRansom = Content.FindChild("King's Ransom").gameObject;
        King_Text = KingsRansom.transform.FindChild("Description").GetComponent<Text>();
        King_Slider = KingsRansom.transform.FindChild("Slider").GetComponent<Slider>();
    }

    public void TestControlButtonPushed()
    {
        this.transform.parent.GetComponent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.TestControlCenter);
    }

    public void RefreshSelector()
    {
        Initialize();
        ClearChoices();
        // Use this to set the Scroll bar so this selection is displayed.        
        PlayerClass player = GlobalControl.control.GetCurrentPlayer();

        TestControlCenter.SetActive(player.HasTestAccess());
        PocketChange.SetActive(true);
        Pocket_Slider.value = player.PocketDiff();
        Pocket_Text.text = GetGameDescription(PocketChange, player.PocketDiff());

        int access = player.HasRMAccess();
        if (access > 0)
        {
            RealMoney.SetActive(true);
            Money_Slider.value = access;
            Money_Text.text = GetGameDescription(RealMoney, access);
        }

        access = player.HasRichAccess();
        if (access > 0)
        {
            Riches.SetActive(true);
            Rich_Slider.value = access;
            Rich_Text.text = GetGameDescription(Riches, access);
        }

        access = player.HasRansomAccess();
        if (access > 0)
        {
            KingsRansom.SetActive(true);
            King_Slider.value = access;
            King_Text.text = GetGameDescription(KingsRansom, access);
        }
    }       

    public void PocketSlider()
    {    
        int value = (int) Pocket_Slider.value;
        Pocket_Text.text = GetGameDescription(PocketChange, value);
        GlobalControl.control.GetCurrentPlayer().SetPocketDiff(value);
    }

    public void PocketPushed()
    {
        int value = (int)Pocket_Slider.value;
        if (value == 1) GlobalControl.control.SetGame(GlobalControl.VaultPackage.PocketChange_Easy);
        else if (value == 2) GlobalControl.control.SetGame(GlobalControl.VaultPackage.PocketChange_Medium);
        else if (value == 3) GlobalControl.control.SetGame(GlobalControl.VaultPackage.PocketChange_Hard);
        else print("Pocket Difficulty set to something impossible");
        PlayGame();
    }

    public void RealMoneySlider()
    {
        int value = (int) Money_Slider.value;
        Money_Text.text = GetGameDescription(RealMoney, value);
        GlobalControl.control.GetCurrentPlayer().SetRMAccess(value);
    }

    public void RealMoneyPushed()
    {
        int value = (int)RealMoney.transform.FindChild("Slider").GetComponent<Slider>().value;
        if (value == 1) GlobalControl.control.SetGame(GlobalControl.VaultPackage.RealMoney_Easy);
        else if (value == 2) GlobalControl.control.SetGame(GlobalControl.VaultPackage.RealMoney_Medium);
        else if (value == 3) GlobalControl.control.SetGame(GlobalControl.VaultPackage.RealMoney_Hard);
        else print("RealMoney Difficulty set to something impossible");

        PlayGame();
    }

    public void RichesSlider()
    {
        int value = (int)Rich_Slider.value;
        Rich_Text.text = GetGameDescription(Riches, value);
        GlobalControl.control.GetCurrentPlayer().SetRichAccess(value);

    }

    public void RichesPushed()
    {
        int value = (int)Riches.transform.FindChild("Slider").GetComponent<Slider>().value;
        if (value == 1) GlobalControl.control.SetGame(GlobalControl.VaultPackage.Riches_Easy);
        else if (value == 2) GlobalControl.control.SetGame(GlobalControl.VaultPackage.Riches_Medium);
        else if (value == 3) GlobalControl.control.SetGame(GlobalControl.VaultPackage.Riches_Hard);
        else print("Riches Difficulty set to something impossible");

        PlayGame();
    }

    public void RansomSlider()
    {
        int value = (int)King_Slider.value;
        King_Text.text = GetGameDescription(KingsRansom, value);
        GlobalControl.control.GetCurrentPlayer().SetRansomAccess(value);

    }

    public void RansomPushed()
    {
        int value = (int)KingsRansom.transform.FindChild("Slider").GetComponent<Slider>().value;
        if (value == 1) GlobalControl.control.SetGame(GlobalControl.VaultPackage.KingsTreasure_Easy);
        else if (value == 2) GlobalControl.control.SetGame(GlobalControl.VaultPackage.KingsTreasure_Medium);
        else if (value == 3) GlobalControl.control.SetGame(GlobalControl.VaultPackage.KingsTreasure_Hard);
        else print("KingsRansom Difficulty set to something impossible");
        PlayGame();
    }

    string GetGameDescription(GameObject GO, int Difficulty)
    {
        if (GO == PocketChange)
        {
            if (Difficulty == 1) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.PocketChange_Easy).GetDescription();
            if (Difficulty == 2) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.PocketChange_Medium).GetDescription();
            if (Difficulty == 3) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.PocketChange_Hard).GetDescription();
        }
        else if (GO == RealMoney)
        {
            if (Difficulty == 1) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.RealMoney_Easy).GetDescription();
            if (Difficulty == 2) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.RealMoney_Medium).GetDescription();
            if (Difficulty == 3) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.RealMoney_Hard).GetDescription();
        }
        else if (GO == Riches)
        {
            if (Difficulty == 1) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.Riches_Easy).GetDescription();
            if (Difficulty == 2) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.Riches_Medium).GetDescription();
            if (Difficulty == 3) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.Riches_Hard).GetDescription();
        }
        else if (GO == KingsRansom)
        {
            if (Difficulty == 1) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.KingsTreasure_Easy).GetDescription();
            if (Difficulty == 2) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.KingsTreasure_Medium).GetDescription();
            if (Difficulty == 3) return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.KingsTreasure_Hard).GetDescription();
        }

        return GlobalControl.control.GetSetting(GlobalControl.VaultPackage.Test).GetDescription();
    }

    void ClearChoices()
    {
        TestControlCenter.SetActive(false);
        RealMoney.SetActive(false);
        Riches.SetActive(false);
        KingsRansom.SetActive(false);
    }

    public void BackPushed()
    { 
        print("BackPushed!");
        this.transform.GetComponentInParent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.TitleScreen);
    }

    public void PlayGame()
    {
        // Run Door Closing animation
        this.transform.GetComponentInParent<FrontEndControl>().CloseDoor();
    }
}
