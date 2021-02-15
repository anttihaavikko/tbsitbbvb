using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusTile : MonoBehaviour
{
    public TMPro.TMP_Text upperText, lowerText;
    public List<Image> colors;
    public GameObject dimmer;

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

    private static string GetSigned(int amount)
    {
        return (amount >= 0 ? "+" : "") + amount;
    }

    public Bonus GetBonus()
    {
        return bonus;
    }
}