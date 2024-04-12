using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameScrean : MonoBehaviour
{
    [SerializeField] private PauseButton _pauseButton;

  
    private void OnEnable()
    {
        _pauseButton.onButtonDown += Pause;
    }

    private void Pause()
    {
        GameController.Instance.SetGamePauset(true);
    }

    private void OnDisable()
    {
        _pauseButton.onButtonDown -= Pause;
    }
}
