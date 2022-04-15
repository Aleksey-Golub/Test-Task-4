using System;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : BaseUIElement
{
    [SerializeField] private Button _restartButton;

    public event Action RestartButtonClicked;

    public void Init()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        RestartButtonClicked?.Invoke();
    }
}
