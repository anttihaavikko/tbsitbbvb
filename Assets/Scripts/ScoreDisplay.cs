using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMPro.TMP_Text score;

    private int playerScore, opponentScore;

    public void UpdateScores(int player, int opponent)
    {
        playerScore += player;
        opponentScore += opponent;

        score.text = playerScore + "-" + opponentScore;
    }
}
