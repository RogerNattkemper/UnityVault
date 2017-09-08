using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIManager : MonoBehaviour
{
    GameObject Col1Key;
    GameObject Col2Key;
    GameObject Col3Key;
    GameObject Col4Key;
    GameObject Col5Key;

    void Start()
    {
        Col1Key = this.transform.FindChild("Color1Key").gameObject;
        Col2Key = this.transform.FindChild("Color2Key").gameObject;
        Col3Key = this.transform.FindChild("Color3Key").gameObject;
        Col4Key = this.transform.FindChild("Color4Key").gameObject;
        Col5Key = this.transform.FindChild("Color5Key").gameObject;
    }

    public void InitializeKeys()
    {
        SetUpKey(Col1Key, GlobalControl.control.Color1Key, GameCreator03.Color1);
        SetUpKey(Col2Key, GlobalControl.control.Color2Key, GameCreator03.Color2);
        SetUpKey(Col3Key, GlobalControl.control.Color3Key, GameCreator03.Color3);
        SetUpKey(Col4Key, GlobalControl.control.Color4Key, GameCreator03.Color4);
        SetUpKey(Col5Key, GlobalControl.control.Color5Key, GameCreator03.Color5);
    }


    void SetUpKey(GameObject key, bool isUsed, Color colour)
    {
        if (isUsed)
        {
            key.SetActive(true);

            Sprite sprite = null;
            if (colour == Color.black) sprite = Resources.Load<Sprite>("Black Key");
            if (colour == Color.blue) sprite = Resources.Load<Sprite>("Blue Key");
            if (colour == Color.green) sprite = Resources.Load<Sprite>("Green Key");
            if (colour == Color.red) sprite = Resources.Load<Sprite>("Red Key");
            if (colour == Color.yellow) sprite = Resources.Load<Sprite>("Yellow Key");
            if (sprite == null) print("Didn't find Key image for " + colour);

            key.GetComponent<Image>().sprite = sprite;
        }

        else key.SetActive(false);
    }

    public void KeyPressed(GameObject GO)
    {
        // Do something
    }

}
