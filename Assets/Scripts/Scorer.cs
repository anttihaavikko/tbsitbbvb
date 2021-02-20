using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Scorer : MonoBehaviour
{
    public List<Dude> dudes;
    public Rigidbody2D ball;
    public ScoreDisplay scoreDisplay;
    public int playerMulti, opponentMulti;
    public Ball theBall;
    public GameStatsManager gameStats;
    public EffectCamera cam;

    private bool triggering;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Check(other);
    }

    private void Check(Component other)
    {
        if (triggering) return;

        if (other.gameObject.CompareTag("Ball"))
        {
            if (playerMulti == 1)
            {
                var hitter = theBall.LastToucher();
                if (dudes.Contains(hitter))
                {
                    gameStats.CompleteChallenge(2);
                }
            }

            triggering = true;
            Invoke(nameof(NextRound), 0.75f);

            var lt = theBall.GetHitter();
            if (dudes.Contains(lt))
            {
                lt.partner.SayNice(Random.Range(0.2f, 0.5f));
            }

            return;
        }

        var d = other.GetComponentInParent<Dude>();
        if (!d) return;
        
        if (dudes.Contains(d))
        {
            if (playerMulti == 1)
            {
                gameStats.CompleteChallenge(5);
            }
            d.ReturnHome();
        }
    }

    private void NextRound()
    {
        var ended = scoreDisplay.UpdateScores(playerMulti, opponentMulti);
        
        AudioManager.Instance.PlayEffectAt(playerMulti == 1 ? 1 : 2, Vector3.zero, 1f);

        if (ended) return;
        
        var starter = dudes.OrderByDescending(d => Mathf.Abs(d.body.position.x)).First().body.position;
        theBall.RespawnOn(new Vector2(starter.x, 5f));
        Invoke(nameof(EnableTrigger), 0.2f);
    }

    private void EnableTrigger()
    {
        triggering = false;
    }
}
