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

    private int step = -1;
    private bool movingDone;

    private readonly string[] messages =
    {
        "NOW HIT THE BALL",
        "AGAIN!",
        "YOU NEED TO JUMP",
        "ONE LAST TIME..."
    };

    private void Start()
    {
        startCam.SetActive(false);
        splashTexts.ForEach(st => st.HideWithDelay());
    }

    private void Update()
    {
        if (!movingDone)
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

    private void AllDone()
    {
        ending.Show();
        Invoke(nameof(BackToMenu), 2f);
    }

    private void BackToMenu()
    {
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
            return;
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
