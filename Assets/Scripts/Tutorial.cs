using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Ball ball;
    public Transform shelf;
    public List<Transform> points;
    public Appearer prompt;
    public List<Dude> dudes;
    public GameObject startCam;
    public GameObject overArrow, underArrow;
    public List<Appearer> splashTexts;
    public Appearer ending;
    public GameStatsManager gameStats;
    public List<ControlHelp> controlHelps;

    private int step = -1;
    private bool movingDone;
    private bool canStart;

    private readonly string[] messages =
    {
        "NOW HIT THE BALL",
        "AGAIN!",
        "YOU NEED TO JUMP",
        "ONE LAST TIME..."
    };

    private void Start()
    {
        AudioManager.Instance.ToMain();
        startCam.SetActive(false);
        splashTexts.ForEach(st => st.HideWithDelay());
        Invoke(nameof(ShowMoveHelps), 4.5f);
    }

    private void ShowMoveHelps()
    {
        canStart = true;
        controlHelps.ForEach(h => h.move.Show());
    }

    private void Update()
    {
        ShouldQuit();
        
        if (!movingDone && canStart)
        {
            if (dudes.All(d => d.HasMoved()))
            {
                movingDone = true;
                NextSpot();
            }
        }

        if (Application.isEditor && Input.GetKeyDown(KeyCode.R))
        {
            SceneChanger.Instance.ChangeScene("Tutorial");
        }
    }
    
    private void ShouldQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    private void AllDone()
    {
        ending.Show();
        Invoke(nameof(BackToMenu), 2f);
    }

    private void BackToMenu()
    {
        AudioManager.Instance.ToMenu();
        SceneChanger.Instance.ChangeScene("Menu");
    }

    public void NextSpot()
    {
        step++;

        if (step >= points.Count)
        {
            DoEffect();
            shelf.gameObject.SetActive(false);
            prompt.Hide();
            Invoke(nameof(AllDone), 1f);
            gameStats.MarkTutorialDone();
            controlHelps.ForEach(h => h.jump.Hide());
            return;
        }

        if (step == 0)
        {
            controlHelps.ForEach(h => h.ShowSwing());
        }
        
        if (step == 2)
        {
            controlHelps.ForEach(h => h.ShowJump());
        }

        if (step > 1)
        {
            overArrow.SetActive(false);
            underArrow.SetActive(true);
        }
        
        prompt.ShowWithText(messages[step], 0f);
        
        DoEffect();
        
        shelf.gameObject.SetActive(true);
        shelf.position = points[step].position;
        ball.transform.parent.gameObject.SetActive(true);
        ball.RespawnOn(shelf.position);
    }

    private void DoEffect()
    {
        var position = shelf.position;
        EffectManager.Instance.AddEffect(0, position);
        EffectManager.Instance.AddEffect(2, position);
    }
}
