using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public List<DudeInfo> infos;
    private bool starting;

    private void Start()
    {
        var rand = new System.Random();
        infos.Select(di => di.dude).Where(d => string.IsNullOrEmpty(d.GetName())).ToList()
            .ForEach(d => d.SetName(Namer.GenerateName(rand)));

        infos.ForEach(i => i.SetNames());
    }

    private void Update()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        StartGame();
    }

    private void StartGame()
    {
        if (!Input.anyKeyDown || starting) return;
        starting = true;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        SceneChanger.Instance.ChangeScene("Main");
    }
}
