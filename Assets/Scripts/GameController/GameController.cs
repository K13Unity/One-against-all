using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] MenuScrean _menuScrean;

    private int _score;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetGamePauset(bool isPause)
    {
        Time.timeScale = isPause ? 0f : 1f;
        _menuScrean.gameObject.SetActive(isPause);
        if (isPause) _menuScrean.Init(_score);
    }

    public void AddScore()
    {
        _score++;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _menuScrean.gameObject.SetActive(true);
        _menuScrean.Init(_score, true);
    }
}
