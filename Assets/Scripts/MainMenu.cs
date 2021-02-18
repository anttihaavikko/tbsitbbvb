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
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        StartGame();

        DebugControls();
    }
    
    private void DebugControls()
    {
        if (!Application.isEditor) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            gameStats.Clear();
        }
    }

    private void StartGame()
    {
        if (Input.GetKeyDown(infos[0].renameKey) || Input.GetKeyDown(infos[1].renameKey)) return;
        if (!Input.anyKeyDown || starting || infos.Any(i => i.IsAsking())) return;
        starting = true;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        SceneChanger.Instance.ChangeScene("Main");
    }
}
