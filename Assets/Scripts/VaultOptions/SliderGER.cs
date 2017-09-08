using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderGER : MonoBehaviour {
    Slider slider;

/* G.E.R. 0: No Expansion
 * G.E.R. 1: 1 Column or Row every other level
 * G.E.R. 2: 1 Column or Row every level
 * G.E.R. 3: 1 Column and Row every level
 * G.E.R. 4: 2 Columns and Rows every level
 * G.E.R. 5: 3 Columns and Rows every level
 */
    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = (float)GlobalControl.control.GridExpansionRate;
    }



}
