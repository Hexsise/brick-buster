using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip[] backgroundMusic;
    private AudioSource audioSource;
    private int currentLevel = 0;
    private int lastMusicChangeLevel = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        PlayBackgroundMusic(currentLevel);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    void OnLevelLoaded(Scene sscene, LoadSceneMode mode)
    {
        currentLevel++;
        if (currentLevel - lastMusicChangeLevel > 2)
        {
            // Change the background music every 2 levels
            PlayBackgroundMusic(currentLevel);
            lastMusicChangeLevel = currentLevel;
        }
    }

    private void PlayBackgroundMusic(int currentLevel)
    {
        if (currentLevel < backgroundMusic.Length)
        {
            audioSource.clip = backgroundMusic[currentLevel];
            audioSource.Play();
        }
    }
}
