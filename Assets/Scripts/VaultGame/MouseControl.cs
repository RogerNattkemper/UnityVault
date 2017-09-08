using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MouseControl : MonoBehaviour 
{
    public static int KeyNumber;

    public List<Texture2D> KeyTexList;
    List<Color> KeyColorList;
    /*
    public Texture2D RepairIcon;
    public Texture2D PanView;
    */
    public Texture2D cursorTexture;

    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = Vector2.zero;

    
    public GameObject hovered;
    public string objName;
    public LockScript ls;
    public enum HoverState { HOVER, NONE}
    public HoverState hover_state = HoverState.NONE;
    
    private Vector3 dragOrigin;

    private bool RayCastOn;

    void Start()
    {
        RayCastOn = false;
    }   

    public void SetCursors()
    {
        KeyTexList = new List<Texture2D>();
        KeyColorList = new List<Color>();
        // Get Screen size, and load the correct sized cursor
        string size;
        print(Screen.width);
        if (Screen.width < 500) size = "32";
        else if (Screen.width < 1000) size = "64";
        else size = "128";

        string path;
        Texture2D tex;

        if (GlobalControl.control.Color1Key)
        {
            tex = Resources.Load<Texture2D>(ColorTrans(GameCreator03.Color1) + size);
            KeyTexList.Add(tex);
            KeyColorList.Add(GameCreator03.Color1);
        }
        
        if (GlobalControl.control.Color2Key)
        {
            tex = Resources.Load<Texture2D>(ColorTrans(GameCreator03.Color2) + size);
            KeyTexList.Add(tex);
            KeyColorList.Add(GameCreator03.Color2);
        }
        if (GlobalControl.control.Color3Key)
        {
            tex = Resources.Load<Texture2D>(ColorTrans(GameCreator03.Color3) + size);
            KeyTexList.Add(tex);
            KeyColorList.Add(GameCreator03.Color3);
        }
        if (GlobalControl.control.Color4Key)
        {
            tex = Resources.Load<Texture2D>(ColorTrans(GameCreator03.Color4) + size);
            KeyTexList.Add(tex);
            KeyColorList.Add(GameCreator03.Color4);
        }
        if (GlobalControl.control.Color5Key)
        {
            tex = Resources.Load<Texture2D>(ColorTrans(GameCreator03.Color5) + size);
            KeyTexList.Add(tex);
            KeyColorList.Add(GameCreator03.Color5);
        }
        
        Cursor.visible = true;
        ChangeMouse(0);
    }
    
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousespot = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (RayCastOn)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(mousespot.x, mousespot.y), Vector2.zero, Mathf.Infinity);

            if (hitInfo.collider)
            {
                if (hitInfo.collider.gameObject != hovered)
                {

                    if (hovered) ls.KeyExit();
                    hovered = hitInfo.collider.gameObject;
                    objName = hovered.name;
                    ls = (LockScript)hovered.GetComponent(typeof(LockScript));
                    ls.KeyOver();
                    hover_state = HoverState.HOVER;
                }
            }
            else
            {
                if (hovered) ls.KeyExit();
                hovered = null;
                ls = null;
                objName = "";
                hover_state = HoverState.NONE;
            }

            //if the Mouse is over a lock, with a key showing, AND the player presses the left mouse
            if ((hover_state == HoverState.HOVER) && (Input.GetMouseButtonDown(0)))
            {
                ls.KeyTry(KeyColorList[KeyNumber]);
            }

            //If mouse over lock, and key is showing, AND the player right clicks
            if ((hover_state == HoverState.HOVER) && (Input.GetMouseButtonDown(1)))
            {
                ls.FlagToggle();
            }

            /*

             // If dragging (Because in Panview)
            if (dragging)
            {

                Vector3 MV = Vector3.zero;
                if ((mousespot.x - dragOrigin.x) > 0) MV += Vector3.right;
                else if ((mousespot.x - dragOrigin.x) < 0) MV += Vector3.left;
                if ((mousespot.y - dragOrigin.y) > 0) MV += Vector3.up;
                else if ((mousespot.y - dragOrigin.y) < 0) MV += Vector3.down;

                Vector3 MV = mousespot - dragOrigin; 

                camCon.MoveCam(MV);
                dragOrigin = mousespot;
            }

             //Camera drag with Pan View (KeyNumber 7), send coordinates to Camera Control
             // Camera Controller has MoveCam(Vector3), which will tell the camera to move that vector 
             /*
            if ((Input.GetMouseButtonDown(0)) && (KeyNumber == 7))
            {
                 dragging = true;
                 dragOrigin = mousespot;
            }


            if (Input.GetMouseButtonUp(0)) dragging = false;
            */


        } // if RaycastOn

            //Right now the mouse changes with the scroll wheel, I am thinking I will remove that
            // and change it to number keys, and clicking on the icons.
            bool loadtex = false;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (KeyNumber == (KeyTexList.Count - 1)) KeyNumber = 0;
                else KeyNumber++;
                loadtex = true;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (KeyNumber == 0) KeyNumber = KeyTexList.Count - 1;
                else KeyNumber--;
                loadtex = true;
            }

            if (loadtex) ChangeMouse(KeyNumber);            
    }

    public void SetRaycast(bool set)
    {
        RayCastOn = set;
    }

    public void ChangeMouse(int mousenum)
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Vector2 hotSpot = new Vector2(0, 0);
        Texture2D tex = KeyTexList[mousenum];
        Cursor.SetCursor(tex, hotSpot, mode);
        Cursor.visible = true;
    }

    string ColorTrans(Color colour)
    {
        if (colour == Color.black) return "Black";
        if (colour == Color.blue) return "Blue";
        if (colour == Color.green) return "Green";
        if (colour == Color.red) return "Red";
        return "Yellow";
    }
}

