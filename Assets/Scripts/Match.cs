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

    private bool isMirrored;
    private bool changing;

    private void Start()
    {
        var count = dudes[0].GetLevel();
        dudes[2].AddBonuses(count);
        dudes[3].AddBonuses(count);
        
        ai1.SetLevel(count);
        ai2.SetLevel(count);

        splash.SetPlayerNames(dudes[0].GetColor(), dudes[1].GetColor());
        splash.SetOpponentNames(dudes[3].GetColor(), dudes[2].GetColor());
        splash.Show();
        
        Invoke(nameof(OnStart), 5f);
    }

    private void OnStart()
    {
        ball.SetActive(true);
        overviewCam.SetActive(false);
    }

    public void End(bool won)
    {
        overviewCam.SetActive(true);
        
        if (won)
        {
            ShowInfo("YOU WON!");
            Invoke(nameof(OnEnd), 2f);    
        }
        else
        {
            ShowInfo("YOU LOST!");
            Invoke(nameof(RestartScene), 3f);
        }
    }

    private void OnEnd()
    {
        bonusCam.SetActive(true);
        dudes.ForEach(d => d.ShowMenu());
    }

    private void Update()
    {
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
            RestartScene();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowInfo("JUST TESTING!");
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnEnd();
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
        
        Invoke(nameof(RestartScene), 1f);
    }

    private void RestartScene()
    {
        SceneChanger.Instance.ChangeScene("Main");
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
