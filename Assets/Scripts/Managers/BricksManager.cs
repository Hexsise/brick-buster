using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;
    public static BricksManager Instance => _instance;

    public static event Action OnLevelLoaded;
    public static event Action OnLevelsCompleted;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private int maxRows = 16;
    private int maxCols = 12;
    private GameObject bricksContainer;
    private float intialBrickSpawnPositionX = -1.96f;
    private float intialBrickSpawnPositionY = 3.325f;
    private float shiftAmount = 0.365f;

    public Brick brickPrefab;

    public Sprite[] Sprites;

    public Color[] BrickColors;

    public List<Brick> RemainingBricks { get; set; }

    public List<int[,]> levelsData { get; set; }

    public int initialBrickCount { get; set; }

    public int CurrentLevel;

    private void Start()
    {
        
        this.bricksContainer = new GameObject("BricksContainer");
        this.levelsData = this.LoadLevelsData();
        this.GenerateBricks();
       

        // moved OnLevelLoaded to GenerateBricks
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel >= this.levelsData.Count()) {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
            // trigger background music change event every 2 levels
            if (this.CurrentLevel % 2 == 0)
            {
                OnLevelsCompleted?.Invoke();
            }
        }
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.levelsData[this.CurrentLevel];
        float currentSpawnX = intialBrickSpawnPositionX;
        float currentSpawnY = intialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = intialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount;
        }

        this.initialBrickCount = this.RemainingBricks.Count;
        OnLevelLoaded.Invoke();
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();

    }

    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

 

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for(int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if(line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {

                // end of current level
                // add the matrix to the last and continue the loop
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }
        return levelsData;
    }
}
