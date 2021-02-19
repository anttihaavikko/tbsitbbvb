using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

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
    private ParticleSystem ballTrail;

    private void Start()
    {
        ballTrail = ball.gameObject.GetComponentInChildren<ParticleSystem>();
    }

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
        EffectManager.Instance.AddEffect(0, ball.position);
        EffectManager.Instance.AddEffect(2, ball.position);
        cam.BaseEffect(0.6f);
        ballTrail.Stop();
        var ended = scoreDisplay.UpdateScores(playerMulti, opponentMulti);

        if (ended) return;

        var starter = dudes.OrderByDescending(d => Mathf.Abs(d.body.position.x)).First().body.position;
        var t = ball.transform;
        t.parent.position = new Vector2(starter.x, 5f);
        t.localPosition = Vector3.zero;
        ball.position = new Vector2(starter.x, 5f);
        ball.velocity = Vector2.zero;
        Invoke(nameof(ReactivateBallTrail), 0.2f);
    }

    private void ReactivateBallTrail()
    {
        ballTrail.Play();
        triggering = false;
    }
}
