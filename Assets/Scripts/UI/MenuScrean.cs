using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScrean : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _clouseButton;
    [SerializeField] private TextMeshProUGUI _score;



    public void Init(int score)
    {
        _score.text = $"Score: {score}";
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(Restart);
        _exitButton.onClick.AddListener(Exit);
        _clouseButton.onClick.AddListener(Close);
    }

    private void Restart()
    {
        GameController.Instance.RestartGame();
    }

    private void Exit()
    {
        GameController.Instance.Exit();
    }

    private void Close()
    {
        GameController.Instance.SetGamePauset(false);
    }
    
}
