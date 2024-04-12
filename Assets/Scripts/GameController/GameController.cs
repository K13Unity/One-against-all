using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private int _score;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetGamePauset(bool isPause)
    {
        Time.timeScale = isPause ? 0f : 1f;
    }

    public void AddScore()
    {
        _score++;
    }
   
    private void ShowPouseScrean()
    {

    }

   
}
