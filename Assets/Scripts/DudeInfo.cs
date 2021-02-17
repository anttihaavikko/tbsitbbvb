using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DudeInfo : MonoBehaviour
{
    public Dude dude;
    public TMP_Text nameText, titleText;
    
    void Start()
    {
        var n = "Ugh";
        var c = ColorUtility.ToHtmlStringRGB(dude.GetColor());
        nameText.text = "<color=#" + c + ">" + n + "</color>";
        titleText.text = dude.GetTitle();
    }
}
