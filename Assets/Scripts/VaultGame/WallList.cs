using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallList : MonoBehaviour
{
    public bool placed;
    public List<Color> colors; // The possible wall colors
    
    public WallList(bool place, List<Color> clers)
    {
        placed = place;
        colors = clers;
    }

    public WallList()
    {

    }

    public Color GetRandomAllowedColor()
    {
        int i = Random.Range(0, colors.Count);
        return colors[i];
    }

    public Color GetColor()
    {
        return (colors[0]);
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

    public void RemoveColor(Color color)
    {
        colors.Remove(color);

        // If you've removed all but one color, then the color is Set
        if (colors.Count == 1) placed = true;
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
        else print("RemoveAllColorsExcept Aborted: Input Color is not present!");
    }

    public void SetColor(Color cler)
    {
        placed = true;
        RemoveAllColorsExcept(cler);
    }

    public bool IsColorSet(Color cler)
    {
        if ((placed) && (colors[0] == cler)) return true;
        return false;
    }

    public bool IsColorAllowed(Color cler)
    {
        return (colors.Contains(cler));
    }

}
