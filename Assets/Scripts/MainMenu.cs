using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public List<DudeInfo> infos;
    public TMP_Text wins, losses, rate;
    public GameStatsManager gameStats;
    public Transform challengeContainer;
    public ChallengeTile challengePrefab;
    public Appearer todo;

    private bool starting;
    private bool canInteract;

    private void Start()
    {
        var rand = new System.Random();
        infos.Select(di => di.dude).Where(d => string.IsNullOrEmpty(d.GetName())).ToList()
            .ForEach(d => d.SetName(Namer.GenerateName(rand)));

        infos.ForEach(i => i.SetNames());

        var w = gameStats.GetData().wins;
        var l = gameStats.GetData().losses;
        var r = w + l > 0 ? Mathf.RoundToInt(w / ((w + l) * 1f) * 100) : 0;
        wins.text = w.ToString();
        losses.text = l.ToString();
        rate.text = r + "% win rate";

        var data = gameStats.GetData();

        for (var i = 0; i < Challenge.Names.Length; i++)
        {
            var c = Instantiate(challengePrefab, challengeContainer);
            c.SetText(i, data);
        }
        
        if (data.wins + data.losses > 0)
        {
            this.StartCoroutine(() => todo.Show(), 1.5f);
        }

        MarkChallengesDone(data);
        
        Invoke(nameof(EnableStart), 1.7f);
    }

    private void EnableStart()
    {
        canInteract = true;
    }

    private void MarkChallengesDone(GameStats data)
    {
        if (!data.recentlyCompleted.Any()) return;

        data.completed.AddRange(data.recentlyCompleted);
        data.recentlyCompleted.Clear();
        gameStats.Save();
    }

    private void Update()
    {
        if (!canInteract) return;
        
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        StartGame();

        Controls();
        DebugControls();
    }

    private void Controls()
    {
        if (EscOnNonWeb())
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            gameStats.Clear();
            infos.Select(info => info.dude).ToList().ForEach(d => d.ClearSave());
            SceneChanger.Instance.ChangeScene("Title");
        }
    }
    
    private void DebugControls()
    {
        if (!Application.isEditor) return;

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            gameStats.Clear();
            SceneChanger.Instance.ChangeScene("Title");
        }
    }

    private bool EscOnNonWeb()
    {
        return Application.platform != RuntimePlatform.WebGLPlayer && Input.GetKeyDown(KeyCode.Escape);
    }

    private void StartGame()
    {
        if (Input.GetKeyDown(infos[0].renameKey) || Input.GetKeyDown(infos[1].renameKey) || EscOnNonWeb()) return;
        if (!Input.anyKeyDown || starting || infos.Any(i => i.IsAsking())) return;
        starting = true;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        var scene = gameStats.GetData().tutorialDone ? "Main" : "Tutorial";
        SceneChanger.Instance.ChangeScene(scene);
    }
}
