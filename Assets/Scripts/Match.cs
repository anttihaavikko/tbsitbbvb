using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Match : MonoBehaviour
{
    public List<Dude> dudes;
    public GameObject bonusCam;
    public IntroSplash splash;
    public GameObject ball, overviewCam;
    public Appearer infoAppearer;
    public TextWithBackground infoText;
    public DudeAI ai1, ai2;
    public GameStatsManager gameStats;

    private bool isMirrored;
    private bool changing;
    private float matchTime;

    private void Start()
    {
        var count = dudes[0].GetLevel();
        dudes[2].AddBonuses(count);
        dudes[3].AddBonuses(count);
        
        ai1.SetLevel(count);
        ai2.SetLevel(count);

        splash.SetPlayerNames(dudes[0], dudes[1]);
        splash.SetOpponentNames(dudes[3].GetColor(), dudes[2].GetColor());
        splash.Show();
        splash.SetHeading(gameStats.GetRoundName());
        
        Invoke(nameof(OnStart), 5f);
    }

    private void OnStart()
    {
        ball.SetActive(true);
        overviewCam.SetActive(false);
    }

    public void End(bool won, int total)
    {
        overviewCam.SetActive(true);
        
        if (won)
        {
            if (total >= 15)
            {
                gameStats.CompleteChallenge(3);
            }

            if (total == 5)
            {
                gameStats.CompleteChallenge(9);
            }
            
            if (matchTime >= 5 * 60f)
            {
                gameStats.CompleteChallenge(4);
            }
            
            if (matchTime < 120f)
            {
                gameStats.CompleteChallenge(8);
            }
            
            gameStats.GetData().wins++;
            ShowInfo("YOU WON!");
            Invoke(nameof(OnEnd), 2f);
            gameStats.CompleteChallenge(0);
        }
        else
        {
            gameStats.GetData().losses++;
            ShowInfo("YOU LOST!");
            Invoke(nameof(BackToMenu), 3f);
        }
        
        Debug.Log("Match lasted for " + matchTime);
        
        gameStats.Save();
    }

    private void OnEnd()
    {
        bonusCam.SetActive(true);
        dudes.ForEach(d => d.ShowMenu());
    }

    private void Update()
    {
        matchTime += Time.deltaTime;
        DebugControls();
        UpdateMirroring();
        LockMenus();
    }

    private void DebugControls()
    {
        if (!Application.isEditor) return;
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            dudes.ForEach(d => d.ClearSave());
            BackToMenu();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowInfo("JUST TESTING!");
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnEnd();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    private void LockMenus()
    {
        var menu1 = dudes[0].bonusMenu;
        var menu2 = dudes[1].bonusMenu;
        
        if (!menu1.Selected() || !menu2.Selected()) return;
        if (changing) return;
        
        changing = true;
        
        menu1.Lock();
        menu2.Lock();
        
        dudes.ForEach(d => d.SaveStats());

        this.StartCoroutine(() =>
        {
            menu1.appearer.Hide();
            menu2.appearer.Hide();
        }, 0.5f);
        
        Invoke(nameof(BackToMenu), 1f);
    }

    private void RestartScene()
    {
        SceneChanger.Instance.ChangeScene("Main");
    }
    
    private void BackToMenu()
    {
        SceneChanger.Instance.ChangeScene("Menu");
    }

    private void UpdateMirroring()
    {
        var mirrored = dudes[0].body.position.x > dudes[1].body.position.x;

        if (mirrored == isMirrored) return;

        isMirrored = mirrored;
        dudes[0].bonusMenu.Mirror();
        dudes[1].bonusMenu.Mirror();
    }

    public void ShowInfo(string message)
    {
        infoText.SetText(message);
        infoAppearer.Show();
        Invoke(nameof(HideInfo), 2f);
    }

    private void HideInfo()
    {
        infoAppearer.Hide();
    }
}
