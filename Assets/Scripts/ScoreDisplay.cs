using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMPro.TMP_Text score;
    public Match match;
    public Appearer scoreDiff;

    private int playerScore, opponentScore;

    public bool UpdateScores(int player, int opponent)
    {
        playerScore += player;
        opponentScore += opponent;

        score.text = playerScore + "-" + opponentScore;

        var enoughScore = playerScore >= 5 || opponentScore >= 5;
        var enoughDiff = Mathf.Abs(playerScore - opponentScore) > 1;

        var ended = enoughScore && enoughDiff;

        if (enoughScore && !enoughDiff)
        {
            scoreDiff.Show();
        }

        if (ended)
        {
            match.End(playerScore > opponentScore, playerScore + opponentScore);
        }

        return ended;
    }
}
