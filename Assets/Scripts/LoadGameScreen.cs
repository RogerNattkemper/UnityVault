using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameScreen : MonoBehaviour
{
    public void PlayGame()
    {
        GameObject.Find("Canvas").GetComponent<FrontEndControl>().PlayGame();
    }
}

