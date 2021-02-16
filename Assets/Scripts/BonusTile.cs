using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusTile : MonoBehaviour
{
    public TMPro.TMP_Text upperText, lowerText;
    public List<Image> colors;
    public GameObject dimmer;
    public Color red, green;

    private Bonus bonus;

    public void Setup(Bonus b)
    {
        bonus = b;
        upperText.text = Enum.GetName(typeof(Stat), b.firstStat) + " " + GetSigned(b.firstAmount);
        if (b.secondAmount != 0)
        {
            lowerText.text = Enum.GetName(typeof(Stat), b.secondStat) + " " + GetSigned(b.secondAmount);    
        }

        colors[(int)b.colorType].color = b.color;
    }

    private string GetSigned(int amount)
    {
        var color = amount > 0 ? ColorUtility.ToHtmlStringRGB(green) : ColorUtility.ToHtmlStringRGB(red);
        return "<color=#" + color + ">" + (amount >= 0 ? "+" : "") + amount + "</color>";
    }

    public Bonus GetBonus()
    {
        return bonus;
    }
}
