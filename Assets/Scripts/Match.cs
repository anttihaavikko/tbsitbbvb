using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public List<Dude> dudes;
    public GameObject bonusCam;

    private bool isMirrored;

    public void End()
    {
        bonusCam.SetActive(true);
        dudes.ForEach(d => d.ShowMenu());
    }

    private void Update()
    {
        UpdateMirroring();
        LockMenus();
    }

    private void LockMenus()
    {
        var menu1 = dudes[0].bonusMenu;
        var menu2 = dudes[1].bonusMenu;
        
        if (!menu1.Selected() || !menu2.Selected()) return;
        
        menu1.Lock();
        menu2.Lock();
        
        dudes.ForEach(d => d.SaveStats());
    }

    private void UpdateMirroring()
    {
        var mirrored = dudes[0].body.position.x > dudes[1].body.position.x;

        if (mirrored == isMirrored) return;

        isMirrored = mirrored;
        dudes[0].bonusMenu.Mirror();
        dudes[1].bonusMenu.Mirror();
    }
}
