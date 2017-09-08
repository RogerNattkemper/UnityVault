using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{

    GameObject SelectionContainer;
    Slider MusicSlider;
    Slider SoundSlider;
    Slider HandedSlider;

    void Start()
    {
        SelectionContainer = this.transform.FindChild("UI Panel/Scroll View/Viewport/Content").gameObject;
        MusicSlider = SelectionContainer.transform.FindChild("Music Volume/Slider").GetComponent<Slider>();
        SoundSlider = SelectionContainer.transform.FindChild("Sound Volume/Slider").GetComponent<Slider>();
        HandedSlider = SelectionContainer.transform.FindChild("Handedness/Slider").GetComponent<Slider>();


        // Load the Settings from the Player onto the controls
        if (GlobalControl.control.MusicOn) MusicSlider.value = GlobalControl.control.MusicVol;
        else MusicSlider.value = 0;

        if (GlobalControl.control.SoundOn) SoundSlider.value = GlobalControl.control.SoundVol;
        else SoundSlider.value = 0;

        if (GlobalControl.control.isRighty) HandedSlider.value = 1;
        else HandedSlider.value = 0;

    }

    public void MusicSliderChanged()
    {
        float volume = MusicSlider.value;
        GlobalControl.control.GetCurrentPlayer().SetMusicVol(volume);
        GlobalControl.control.MusicVol = volume;
        if (volume > 0) GlobalControl.control.MusicOn = true;
        else GlobalControl.control.MusicOn = false;
        GlobalControl.control.GetCurrentPlayer().SetMusic(GlobalControl.control.MusicOn);
        GlobalControl.control.SavePlayerData();      
    }

    public void SoundSliderChanged()
    {
        float volume = SoundSlider.value;
        GlobalControl.control.GetCurrentPlayer().SetSoundVol(volume);
        GlobalControl.control.SoundVol = volume;
        if (volume > 0) GlobalControl.control.SoundOn = true;
        else GlobalControl.control.SoundOn = false;
        GlobalControl.control.GetCurrentPlayer().SetSound(GlobalControl.control.SoundOn);
        GlobalControl.control.SavePlayerData();
    }

    public void HandedSliderChanged()
    {
        bool isright = (HandedSlider.value == 1) ? true : false;
        GlobalControl.control.GetCurrentPlayer().SetHanded(isright);
        GlobalControl.control.isRighty = isright;
        GlobalControl.control.SavePlayerData();
    }

    public void RefreshSettings()
    {
        SelectionContainer = this.transform.FindChild("UI Panel/Scroll View/Viewport/Content").gameObject;
        MusicSlider = SelectionContainer.transform.FindChild("Music Volume/Slider").GetComponent<Slider>();
        SoundSlider = SelectionContainer.transform.FindChild("Sound Volume/Slider").GetComponent<Slider>();
        HandedSlider = SelectionContainer.transform.FindChild("Handedness/Slider").GetComponent<Slider>();
        // Load the Settings from the Player onto the controls
        if (GlobalControl.control.MusicOn) MusicSlider.value = GlobalControl.control.MusicVol;
        else MusicSlider.value = 0;

        if (GlobalControl.control.SoundOn) SoundSlider.value = GlobalControl.control.SoundVol;
        else SoundSlider.value = 0;

        HandedSlider.value = (GlobalControl.control.isRighty) ? 1 : 0;
    }

    public void BackPushed()
    {
        this.transform.GetComponentInParent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.TitleScreen);
    }
}
