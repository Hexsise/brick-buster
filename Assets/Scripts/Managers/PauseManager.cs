﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    public GameObject ResumeButton;
    public GameObject QuitButton;
    public GameObject Paddle;

    private bool isPaused = false;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // freeze game time
        ResumeButton.SetActive(true);
        QuitButton.SetActive(true);
        Paddle.SetActive(false);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        ResumeButton.SetActive(false);
        QuitButton.SetActive(false);
        Paddle.SetActive(true);
        isPaused = false;
    }

    public void quitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }
}
