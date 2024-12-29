using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseUI;
    public GameObject clearUI;
    public GameObject failUI;
    private void Start()
    {
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        gameUI.SetActive(false);
        pauseUI.SetActive(true);
    }

    public void OnClickResume()
    {
        Time.timeScale = 1;
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void OnClickHome()
    {
        LoadingSceneCon.LoadScene("HomeScene");
    }
    
    public void OnClickRestart()
    {
        LoadingSceneCon.LoadScene("GameScene");
    }

    public void ActiveClearUI()
    {
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        failUI.SetActive(false);
        clearUI.SetActive(true);
    }

    public void ActiveFailUI()
    {
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        clearUI.SetActive(false);
        failUI.SetActive(true);
    }
}
