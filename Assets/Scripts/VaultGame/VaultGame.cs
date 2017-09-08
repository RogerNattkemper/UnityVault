using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VaultGame : MonoBehaviour 
{
    
    public void BackClicked()
    {
        SceneManager.LoadScene("PC Title Scene");
    }
}
