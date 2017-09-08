using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControl : MonoBehaviour
{

    public void BackPushed()
    {
        this.transform.GetComponentInParent<FrontEndControl>().SetNextState(FrontEndControl.FrontState.TitleScreen);
    }


}
