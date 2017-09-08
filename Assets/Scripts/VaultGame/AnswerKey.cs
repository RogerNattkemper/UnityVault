using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnswerKey : MonoBehaviour
{
    public int position; //Where in the grid it is
    public bool placed; // Has this answer been fixed?
    public List<Color> colors; // What color the KEY is (Not the Lock)
    public bool solved; // Whether this Lock has been successfully openned


    public AnswerKey(int pos, bool place, List<Color> ky, bool slvd)
    {
        position = pos;
        placed = place;
        colors = ky;
        solved = slvd;
    }

    public AnswerKey()
    {

    }

    // This will return an allowed Color, if whiteLast is set
    // Then as long as there is another color to choose, it will
    // return that.
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

    //


    public Color GetColor()
    {
        return (colors[0]);
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

    // This returns the color value of the color
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

    //This is to cover the circumstance in which the color you need to swap to has already been removed
    // This will reinject the color in question (This will allow th "remove all Color
    public void InjectColor(Color color)
    {
        if (!colors.Contains(color)) colors.Insert(0, color);
    }

    public void FixAnswer(Color color)
    {
        placed = true;
        RemoveAllColorsExcept(color);
    }

    public bool IsColorSet(Color color)
    {
        if ((placed) && (colors[0] == color)) return true;
        return false;
    }

    string ColorTextulizer(Color colour)
    {
        if (colour == GameCreator03.Color1) return "Color1";
        if (colour == GameCreator03.Color2) return "Color2";
        if (colour == GameCreator03.Color3) return "Color3";
        if (colour == GameCreator03.Color4) return "Color4";
        if (colour == GameCreator03.Color5) return "Color5";
        return "Wat?!";
    }
}
