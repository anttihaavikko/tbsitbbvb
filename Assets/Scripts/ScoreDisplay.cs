using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMPro.TMP_Text score;
    public Match match;

    private int playerScore, opponentScore;

    public bool UpdateScores(int player, int opponent)
    {
        playerScore += player;
        opponentScore += opponent;

        score.text = playerScore + "-" + opponentScore;
        
        match.End();

        return true;
    }
}
