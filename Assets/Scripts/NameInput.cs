﻿using System;
using UnityEngine;

public class NameInput : MonoBehaviour {
    public Action<string> onUpdate;
    public Action<string> onDone;

    string nameInput = "";
    string playerName = "";
    bool showLine;
    bool asking;

    public void Ask(string oldName)
    {
        LineToggle();
        asking = true;
        playerName = oldName;
    }

    public string MarkDoneAndGetName()
	{
		asking = false;
		onDone?.Invoke(playerName);
		return playerName;
	}
	
	// Update is called once per frame
	void Update () {

        if (!asking)
            return;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
        }

        foreach (char c in Input.inputString)
        {

            if (c == "\b"[0])
            {
                if (playerName.Length != 0)
                {
                    playerName = playerName.Substring(0, playerName.Length - 1);
                    TriggerUpdate();

                    // AudioManager.Instance.PlayEffectAt(19, Vector3.zero, 0.267f);
                    // AudioManager.Instance.PlayEffectAt(20, Vector3.zero, 0.227f);
                    // AudioManager.Instance.PlayEffectAt(23, Vector3.zero, 1.174f);
                }
                else
                {
                    // AudioManager.Instance.PlayEffectAt(18, Vector3.zero, 0.526f);
                    // AudioManager.Instance.PlayEffectAt(17, Vector3.zero, 0.972f);
                }

            }
            else
            {

                if (c == ';' || c == ',' || c == ':' || c == ' ')
                {
                    return;
                }

                if (c == "\n"[0] || c == "\r"[0])
                {

                    if (playerName != "")
                    {
                        // AudioManager.Instance.PlayEffectAt(1, Vector3.zero, 0.478f);
                        // AudioManager.Instance.PlayEffectAt(2, Vector3.zero, 1f);
                        // AudioManager.Instance.PlayEffectAt(15, Vector3.zero, 1.474f);

                        TriggerUpdate();
						asking = false;
                        CancelInvoke(nameof(LineToggle));
                        onDone?.Invoke(playerName);
                    } else {
                        // AudioManager.Instance.PlayEffectAt(18, Vector3.zero, 0.526f);
                        // AudioManager.Instance.PlayEffectAt(17, Vector3.zero, 0.972f);
                    }

                }
                else
                {
                    if (playerName.Length < 8 && char.IsLetterOrDigit(c))
                    {
                        playerName += c.ToString();
                        TriggerUpdate();

                        // AudioManager.Instance.PlayEffectAt(1, Vector3.zero, 0.478f);
                        // AudioManager.Instance.PlayEffectAt(2, Vector3.zero, 1f);
                        // AudioManager.Instance.PlayEffectAt(15, Vector3.zero, 1.474f);

                    }
                }

            }
        }
    }

    void LineToggle()
    {
        showLine = !showLine;
        TriggerUpdate();
        Invoke(nameof(LineToggle), 0.5f);
    }

    void TriggerUpdate()
    {
		if (!asking) return;

        nameInput = playerName + (showLine ? "\u25A1" : "\u25A0");
        onUpdate?.Invoke(nameInput);
    }

    public bool IsAsking()
    {
        return asking;
    }
}
