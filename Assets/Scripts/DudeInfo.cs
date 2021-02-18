using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DudeInfo : MonoBehaviour
{
    public Dude dude;
    public TMP_Text nameText, titleText;
    public NameInput nameInput;
    public KeyCode renameKey;
    public MainMenu menu;

    private bool asking;

    public void SetNames()
    {
        UpdateShownName(dude.GetName());
        titleText.text = dude.GetTitle();
    }

    private void UpdateShownName(string n)
    {
        var c = ColorUtility.ToHtmlStringRGB(dude.GetColor());
        nameText.text = "<color=#" + c + ">" + n + "</color>";
    }

    private void OnNameInputChange(string text)
    {
        UpdateShownName(text);
    }

    private void Update()
    {
        if (menu.infos.Any(info => info.IsAsking())) return;
        if (!Input.GetKeyDown(renameKey) || nameInput.IsAsking()) return;
        asking = true;
        nameInput.Ask(dude.GetName());
        nameInput.onDone += OnNameInputDone;
        nameInput.onUpdate += OnNameInputChange;
    }

    private void OnNameInputDone(string text)
    {
        nameInput.onDone -= OnNameInputDone;
        nameInput.onUpdate -= OnNameInputChange;
        
        dude.SetName(text);
        UpdateShownName(dude.GetName());
        Invoke(nameof(EndAsk), 0.25f);
    }

    private void EndAsk()
    {
        asking = false;
    }

    public bool IsAsking()
    {
        return asking;
    }
}
