using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OnClickStart()
    {
        LoadingSceneCon.LoadScene("GameScene");
    }
}
