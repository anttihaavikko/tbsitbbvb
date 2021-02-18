using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    public GameObject startCam;

    private bool canStart;

    private void Start()
    {
        startCam.SetActive(false);
        Invoke(nameof(EnableStart), 1.5f);
    }

    private void EnableStart()
    {
        canStart = true;
    }

    private void Update()
    {
        if (!canStart) return;

        if (Application.isEditor && Input.GetKeyDown(KeyCode.R))
        {
            SceneChanger.Instance.ChangeScene("Title");
            return;
        }

        if (Input.anyKeyDown)
        {
            SceneChanger.Instance.ChangeScene("Menu");
        }
    }
}
