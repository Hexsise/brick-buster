using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip[] backgroundMusic;
    private AudioSource audioSource;
    private int currentMusic = 0;
    private int lastMusicChangeLevel = 0;

    void Awake()
    {
        BricksManager.OnLevelsCompleted += ChangeBackgroundMusic;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        PlayBackgroundMusic(currentMusic);
    }

    private void ChangeBackgroundMusic()
    {
        currentMusic++;
        PlayBackgroundMusic(currentMusic); // play music at currentMusic index
    }

    private void PlayBackgroundMusic(int currentMusic)
    {
        if (currentMusic < backgroundMusic.Length)
        {
            audioSource.clip = backgroundMusic[currentMusic];
            audioSource.Play();
        }
    }
}
