using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridSpace : MonoBehaviour 
{
    public int position; // Where this gridspace is in the gamegrid (needed for debugging)
    public int column; // Column of grid where this is found
    public int row;  // Row of grid where found
    public bool placed;  // Whether or not the lock has been set for this position
    public bool answer;  // Indicates if this is an Answer space or not
    public List <Color> colors;  // The color of the grid space
    public string texture;  // This is the non-hover texture
    public List<int> ruleBreaks; // A list of the rules that this space violates if it is chosen as answer
   
    public GridSpace(int pos, int col, int rowlumn, bool place, bool ans, List<Color> clr, string txtr)
    {
        position = pos;
        column = col;
        row = rowlumn;
        placed = place;
        answer = ans;
        colors = clr;
        texture = txtr;
        ruleBreaks = new List<int>();
    }

    public GridSpace()
    {

    }

    public Color GetRandomAllowedColor(bool whiteLast)
    {
        int i;

        if ((whiteLast) && (colors.Count > 1))
        {
            i = Random.Range(1, colors.Count);
        }
        else i = Random.Range(0, colors.Count);

        return colors[i];
    }

    public Color GetColor()
    { 
        return(colors[0]);
    }

    public int GetColorInt()
    {
        if (colors != null)
        {
            if (colors[0] == Color.white) return 0;
            if (colors[0] == Color.black) return 1;
            if (colors[0] == Color.blue) return 2;
            if (colors[0] == Color.green) return 3;
            if (colors[0] == Color.red) return 4;
            if (colors[0] == Color.yellow) return 5;

            print("Returning a non-recognized color!");
        }
        else print("There are no COLORS in the Color LIST!");
        return -1;
    }

    // This returns the RandomCOlor number of the color
    public int GetMixColorInt()
    {
        if (colors != null)
        {
            if (colors[0] == GameCreator03.white) return 0;
            if (colors[0] == GameCreator03.Color1) return 1;
            if (colors[0] == GameCreator03.Color2) return 2;
            if (colors[0] == GameCreator03.Color3) return 3;
            if (colors[0] == GameCreator03.Color4) return 4;
            if (colors[0] == GameCreator03.Color5) return 5;

            print("Returning a non-recognized number!");
        }
        else print("There are no COLORS in the Color LIST!");
        return -1;
    }

    public void RemoveColor(Color color)
    {
        colors.Remove(color);  
    }

    public void RemoveAllColorsExcept(Color color)
    {
        if (colors.Contains(color))
        {
            if (color != Color.white) colors.Remove(Color.white);
            if (color != Color.black) colors.Remove(Color.black);
            if (color != Color.blue) colors.Remove(Color.blue);
            if (color != Color.green) colors.Remove(Color.green);
            if (color != Color.red) colors.Remove(Color.red);
            if (color != Color.yellow) colors.Remove(Color.yellow);
        }
        else print("RemoveAllColorsExcept Aborted: " + ColorTextulizer(color) + " is not present!");
      
    }

    public void SetAnswer(Color cler)
    {
        placed = true;
        answer = true;
        RemoveAllColorsExcept(cler);
           
        string tmp = "";
        
        if (cler == Color.black) tmp = "Black Lock";
        if (cler == Color.blue) tmp = "Blue Lock";
        if (cler == Color.green) tmp = "Green Lock";
        if (cler == Color.red) tmp = "Red Lock";
        if (cler == Color.yellow) tmp = "Yellow Lock";
        texture = tmp;
    }

    public void SetLock(Color cler)
    {
        placed = true;
        RemoveAllColorsExcept(cler);

        string tmp = "";

        if (cler == Color.black) tmp = "Black Lock";
        if (cler == Color.blue) tmp = "Blue Lock";
        if (cler == Color.green) tmp = "Green Lock";
        if (cler == Color.red) tmp = "Red Lock";
        if (cler == Color.yellow) tmp = "Yellow Lock";
        texture = tmp;    
    }

    public bool IsColorAllowed(Color cler)
    {
        return (colors.Contains(cler));
    }

    public bool IsColorSet(Color cler)
    {
        if ((placed) && (colors[0] == cler)) return true;
        return false;
    }

    string ColorTextulizer(Color colour)
    {
        if (colour == Color.black) return "black";
        if (colour == Color.blue) return "blue";
        if (colour == Color.green) return "green";
        if (colour == Color.red) return "red";
        if (colour == Color.yellow) return "yellow";
        return "white";
    }        
}