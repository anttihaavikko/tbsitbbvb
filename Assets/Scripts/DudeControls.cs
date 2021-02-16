using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DudeControls : MonoBehaviour
{
    public Dude dude;
    public KeyCode up, down, left, right;

    private void Update()
    {
        var dir = 0f;

        if (Input.GetKey(left)) dir = -1f;
        if (Input.GetKey(right)) dir = 1f;

        dude.Move(dir);
        
        if(Input.GetKeyDown(up)) dude.Jump();
        
        if(Input.GetKeyDown(down)) dude.Swing();
    }
}
