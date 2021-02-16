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
        var mirrored = dudes[0].body.position.x > dudes[1].body.position.x;

        if (mirrored == isMirrored) return;
        
        isMirrored = mirrored;
        dudes[0].bonusMenu.Mirror();
        dudes[1].bonusMenu.Mirror();
    }
}
