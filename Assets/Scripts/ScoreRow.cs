using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRow : MonoBehaviour
{
    public RawImage flag;
    public TMP_Text position, names, scores;

    public void Setup(string pos, string nam, string sco)
    {
        position.text = pos;
        names.text = nam;
        scores.text = sco;
    }

    public static long GetNum(int challenges, int wins, int losses)
    {
        return 1000000 * challenges + 10000 * wins + losses;
    }
    
    public static ScoreDetails GetDetails(long num)
    {
        var challenges = num / 1000000;
        var wins = (num - challenges * 1000000) / 10000;
        var losses = num - challenges * 1000000 - wins * 10000;
        return new ScoreDetails((int)challenges, (int)wins, (int)losses);
    }
}

public class ScoreDetails
{
    public readonly int wins;
    public readonly int losses;
    public readonly int challenges;

    public ScoreDetails(int c, int w, int l)
    {
        challenges = c;
        wins = w;
        losses = l;
    }
}