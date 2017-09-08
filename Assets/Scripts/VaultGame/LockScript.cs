using UnityEngine;
using System.Collections;

public class LockScript : MonoBehaviour 
{

    Animator animator;
    public AudioSource[] sounds;
    public AudioSource explode;
    public AudioSource ding;
    
    public bool isLocked = true;
    public bool isFlagged = false;
    public bool isRevealed = false;
    public int LOCK_ID;
    public bool isAnswer;
    public Color AnswerKeyColor; // The Key that will unlock this lock, (if it is an answer)

    public GameDisplay gamedisplay;

    const int STATE_EMERGE_IDLE = 0;
    const int STATE_LOCKED_IDLE = 1;
    const int STATE_HOVER = 2;
    const int STATE_OPEN_IDLE = 3;
    const int STATE_FLAGGED = 4;
    const int STATE_BURNING = 5;
    const int STATE_REPAIRING = 6;
    const int STATE_DEFUSED = 7;
    const int STATE_REVEALED = 8;
   

    int currentState = STATE_EMERGE_IDLE;

    float ShimmerTime = 30.0f;
    float TimeLeft;
    //bool Shimmer = false;

    GameUIControl GUC;

	// Use this for initialization
	void Awake () 
    {        
        TimeLeft = ShimmerTime;
        changeState(STATE_EMERGE_IDLE); 
        animator = this.GetComponent<Animator>();
        gamedisplay = GameObject.FindGameObjectWithTag("Game Display").GetComponent<GameDisplay>();
        GUC = GameObject.Find("Canvas/Game UI").GetComponent<GameUIControl>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        TimeLeft -= Time.deltaTime;
        if(TimeLeft < 0)
        {
            animator.SetTrigger("Shimmer");
            TimeLeft = ShimmerTime;
        }           
    }

    public void KeyOver() // If the mouse is a key and is over the lock
    {
        if ((currentState == STATE_LOCKED_IDLE) || (currentState == STATE_REVEALED))
        {
            changeState(STATE_HOVER);
            currentState = STATE_HOVER;
        }                   
    }

    public void KeyExit()
    {
        if (currentState == STATE_HOVER)
        {
            if (!isRevealed)
            {
                changeState(STATE_LOCKED_IDLE);
                currentState = STATE_LOCKED_IDLE;
            }
            else
            {
                changeState(STATE_REVEALED);
                currentState = STATE_REVEALED;
            }
        }
    }

    public void KeyTry(Color KeyColor) // If the mouse is a key, and the player presses the left button
    {
        if (currentState == STATE_HOVER)
        {
            if ((isAnswer) && (KeyColor == AnswerKeyColor))
                {
                    gamedisplay.AnswerCheckIn(LOCK_ID);
                    changeState(STATE_OPEN_IDLE);
                    currentState = STATE_OPEN_IDLE;
                }
            else
                {
                if ((GUC.defusesLeft > 0) && (!isAnswer))
                    {
                        GUC.UseDefuse();
                        changeState(STATE_DEFUSED);
                        currentState = STATE_DEFUSED;
                    }
                else
                    {
                        gamedisplay.WrongAnswer(LOCK_ID);
                        changeState(STATE_BURNING);
                        currentState = STATE_BURNING;
                    }
                }
        }
        
    }

    public void FlagToggle() // If Player presses the right mouse button over the lock
    {
        if (currentState == STATE_HOVER)
        {
            print("Toggling Flag");
            changeState(STATE_FLAGGED);
            currentState = STATE_FLAGGED;
        }

        else if (currentState == STATE_FLAGGED)
        {
            changeState(STATE_HOVER);
            currentState = STATE_HOVER;    
        }
    }

    public void RevealAnswerLock()
    {
        isRevealed = true;
        if (currentState == STATE_LOCKED_IDLE)
        {
            changeState(STATE_REVEALED);
            animator.SetBool("isRevealed", true);
        }
    }

    public void RepairLock()
    {
        if(currentState == STATE_BURNING)
        {
            changeState(STATE_OPEN_IDLE);
            currentState = STATE_OPEN_IDLE;
        }
    }
    

     void changeState(int state)
     {
         if (currentState == state) return;

         switch (state)
         {
             case STATE_EMERGE_IDLE:
                 animator.SetInteger("State", STATE_EMERGE_IDLE);
                 break;
             case STATE_LOCKED_IDLE:
                 animator.SetInteger("State", STATE_LOCKED_IDLE);
                 break;
             case STATE_HOVER:
                 animator.SetInteger("State", STATE_HOVER);
                 break;
             case STATE_OPEN_IDLE:
                 animator.SetInteger("State", STATE_OPEN_IDLE);
                 break;
             case STATE_FLAGGED:
                 animator.SetInteger("State", STATE_FLAGGED);
                 break;
             case STATE_BURNING:
                 animator.SetInteger("State", STATE_BURNING);
                 break;
            case STATE_DEFUSED:
                animator.SetInteger("State", STATE_DEFUSED);
                break;   
         }
     }

     
     public void SetLock(int id, bool answer, Color ac)
     { 
       LOCK_ID = id;
       isAnswer = answer;
       AnswerKeyColor = ac;
     }

     public void Go()
     {
         changeState(STATE_LOCKED_IDLE);
         currentState = STATE_LOCKED_IDLE;
     }

   /*
        if (Input.GetMouseButtonDown(0)) //If the left mouse was pressed
        {
            if (currentState == STATE_HOVER)
            {
                isLocked = false;
                // If this lock is an Answer, and the mouse is the right key
                if ((isAnswer) && (KeyNum == MouseControl.KeyNumber))
                {
                    gamedisplay.AnswerCheckIn(LOCK_ID);
                    changeState(STATE_OPEN_IDLE);
                    currentState = STATE_OPEN_IDLE;
                }
                // Or the mouse is a key 
                else if ((KeyNum >= 1) && (KeyNum <= 5))
             
            }

            // If the player clicks on the burning lock with a repair icon
            else if ((currentState == STATE_BURNING) && (MouseControl.RepairNum == MouseControl.KeyNumber))
            {
                changeState(STATE_LOCKED_IDLE);
                currentState = STATE_LOCKED_IDLE;
            }
        }

        //If the right mouse was pressed the lock will be flagged
        if (Input.GetMouseButtonDown(1)) 
        {
            if ((currentState == STATE_HOVER) || (currentState == STATE_FLAGGED))
            {
                if (currentState == STATE_HOVER)
                {
                    changeState(STATE_FLAGGED);
                    currentState = STATE_FLAGGED;
                    isFlagged = true;
                }
                else
                {
                    changeState(STATE_HOVER);
                    currentState = STATE_HOVER;
                    isFlagged = false;
                }
            }
        }          
     
     }

    void OnMouseEnter()
    {
        // If the mouse is not the PanViewIcon
        if (MouseControl.KeyNumber != MouseControl.PanViewIcon)
        {
        //WHen mouse goes over the lock and has not been unlocked or flagged already
        if (currentState == STATE_LOCKED_IDLE)
            {
                changeState(STATE_HOVER);
                currentState = STATE_HOVER;
            }
        }
    }
    
    void OnMouseExit()
    {
        if ((isLocked) && (!isFlagged))
        {
            changeState(STATE_LOCKED_IDLE);
            currentState = STATE_LOCKED_IDLE;
        }
    }
     */

}

   

