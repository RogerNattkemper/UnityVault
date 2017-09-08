using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldPLayerName : MonoBehaviour {

    public void InputPlayerName(string pn)
    {
        GlobalControl.control.PlayerName = pn;
    }
}
