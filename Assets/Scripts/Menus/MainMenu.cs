using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void highscoreTable()
    {
        SceneManager.LoadScene(2);
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
        // In the Editor, simply stop Play Mode.
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // In standalone builds, quit the application.
        Application.Quit();
        #endif
    }
}
