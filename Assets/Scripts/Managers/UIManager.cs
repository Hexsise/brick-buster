using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Target;
    public Text ScoreText;
    public Text LivesText;
    public Text LevelText;

    public int Score { get; set; }

    private void Awake()
    {
        
       Brick.OnBrickDestruction += OnBrickDestruction;
       BricksManager.OnLevelLoaded += OnLevelLoaded;
       GameManager.OnLivesLost += OnLivesLost; 
       
    }
 
    private void Start()
    {
        OnLivesLost(GameManager.Instance.AvailableLives);
    }

    private void OnLivesLost(int remainingLives)
    {
        LivesText.text = $"LIVES: ";

        for (int i = 0; i < remainingLives; i++)
        {
            LivesText.text += "❤️";
        }
    }

    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        LevelText.text = $"LEVEL: " + (BricksManager.Instance.CurrentLevel + 1);
    }

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE:" + "\n" +
        (scoreString);
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }

    private void UpdateRemainingBricksText()
    {
        Target.text = $"TARGET:" + "\n" +
        (BricksManager.Instance.RemainingBricks.Count) + "/" + (BricksManager.Instance.initialBrickCount);
    }

    private void OnDisable()
    {
      Brick.OnBrickDestruction -= OnBrickDestruction;
      BricksManager.OnLevelLoaded -= OnLevelLoaded;
      GameManager.OnLivesLost -= OnLivesLost;
    }
}
