using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public List<Dude> dudes;
    public GameObject bonusCam;

    public void End()
    {
        bonusCam.SetActive(true);
        dudes.ForEach(d => d.ShowMenu());
    }
}
