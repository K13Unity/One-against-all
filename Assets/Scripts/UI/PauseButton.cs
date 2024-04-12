using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;

    public event Action onButtonDown;

    private void OnEnable()
    {
        _pauseButton.onClick.AddListener(OnButtonClic);
    }

    private void OnButtonClic()
    {
        onButtonDown?.Invoke();
    }

    private void OnDisable()
    {
        _pauseButton.onClick.RemoveListener(OnButtonClic);
    }
}