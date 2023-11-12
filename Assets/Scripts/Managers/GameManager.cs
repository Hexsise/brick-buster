using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject gameOverScreen;

    public GameObject victoryScreen;

    public GameObject highscoreTable;

    public int AvailableLives = 3;
    public int Lives { get; set; }
    public bool IsGameStarted { get; set; }

    public static event Action<int> OnLivesLost;

    private void Start()
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        Cursor.visible = false;
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;  
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1); // build index 1 = Game scene
    }

    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                // show game over
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLivesLost?.Invoke(this.Lives);
                // reset balls
                // stop the game
                // reload the level

                // ctrl + '.' to make method and f12 to go to method
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    public void ShowHighscoreTable()
    {
        highscoreTable.SetActive(true);
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
}

/* LIST OF CURRENT BUGS (12/12)
 * 
 * 1) [FIXED] Victory screen --> Play again --> Missed ball does not properly reset and cannot be launched.
 * 
 * 2) [FIXED] GameOver screen --> Try Again -> Missed ball does not trigger death wall --> MissReferenceException for Destroyed object.
 * 
 * 
 * 
*/


