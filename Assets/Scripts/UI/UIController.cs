using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private BuyBoosterPanel _buyBoosterPanel;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _clientCounter;
    [SerializeField] private BoosterButton _boosterButton;

    public event Action RestartButtonClicked;
    public event Action BuyBoosterButtonClicked;
    public event Action UseBoosterButtonClicked;

    public void Init()
    {
        _winPanel.Init();
        _winPanel.RestartButtonClicked += OnRestartButtonClicked;

        _losePanel.Init();
        _losePanel.RestartButtonClicked += OnRestartButtonClicked;

        _buyBoosterPanel.Init();
        _buyBoosterPanel.BuyBoosterButtonClicked += OnBuyBoosterButtonClicked;

        _boosterButton.Init();
        _boosterButton.UseBoosterButtonClicked += OnUseBoosterButtonClicked;
    }

    public void UpdateBoosterButton(bool isUsable)
    {
        if (isUsable)
            _boosterButton.Activate();
        else
            _boosterButton.Deactivate();
    }

    public void UpdateClientCounter(int waitingToEnter, int total)
    {
        _clientCounter.text = $"{waitingToEnter}/{total}";
    }

    public void UpdateTimer(int minutes, int seconds)
    {
        _timer.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowLoseScreen()
    {
        _losePanel.Show();
    }

    public void ShowWinScreen()
    {
        _winPanel.Show();
    }

    public void ShowBuyBoosterPanelScreen()
    {
        _buyBoosterPanel.Show();
    }

    public void HideBuyBoosterPanel()
    {
        _buyBoosterPanel.Hide();
    }

    private void OnRestartButtonClicked()
    {
        RestartButtonClicked?.Invoke();
    }

    private void OnBuyBoosterButtonClicked()
    {
        BuyBoosterButtonClicked?.Invoke();
    }

    private void OnUseBoosterButtonClicked()
    {
        UseBoosterButtonClicked?.Invoke();
    }
}
