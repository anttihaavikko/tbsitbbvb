using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool starting;

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
