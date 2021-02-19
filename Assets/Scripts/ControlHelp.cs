using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHelp : MonoBehaviour
{
    public Appearer move, swing, jump;

    public void ShowSwing()
    {
        move.Hide();
        this.StartCoroutine(() => swing.Show(), 0.3f);
    }
    
    public void ShowJump()
    {
        swing.Hide();
        this.StartCoroutine(() => jump.Show(), 0.3f);
    }
}
