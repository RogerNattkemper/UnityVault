using UnityEngine;
using System.Collections;

public class PlateScript : MonoBehaviour {

    Animator animator;
   
    public int LOCK_ID;
    
    float ShimmerTime = 30.0f;
    float TimeLeft;

    // Use this for initialization
    void Awake()
    {

        TimeLeft = ShimmerTime;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if (TimeLeft < 0)
        {
            animator.SetTrigger("Shimmer");
            TimeLeft = ShimmerTime;
        }
    }
   

    public void NamePlate(int id)
    {
        LOCK_ID = id;
    } 
 


}
