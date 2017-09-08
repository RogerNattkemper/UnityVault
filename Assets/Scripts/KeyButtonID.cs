using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButtonID : MonoBehaviour
{
    KeyUIManager KUM;

    void Start()
    {
        KUM = this.transform.parent.GetComponent<KeyUIManager>();
    }

    public void pressed()
    {
        KUM.KeyPressed(this.gameObject);
    }
}
