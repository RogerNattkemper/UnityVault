using UnityEngine;
using System.Collections;

public class RulesPanel : MonoBehaviour {

    //Open means the Grid is exposed and the rules can't be seen
    bool IsOpen;
    private Animator anim;
   
	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        IsOpen = false;
	}

    public void TogglePanel()
    {
        print("In Toggle Panel");
   
        anim.enabled = true;
        if (IsOpen) anim.Play("ScrollRectSlideClosed");
        else anim.Play("ScrollRectSlideOpen");
        IsOpen = !IsOpen;
    }
}
