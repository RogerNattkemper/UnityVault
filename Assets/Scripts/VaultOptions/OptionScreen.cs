using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class OptionScreen : MonoBehaviour {

    void BackClicked()
    {
        SceneManager.LoadScene("VaultTitle");
    }
}
