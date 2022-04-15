using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterButton : BaseUIElement
{
    [SerializeField] private Button _useBoosterButton;

    public event Action UseBoosterButtonClicked;

    public void Init()
    {
        _useBoosterButton.onClick.AddListener(OnUseBoosterButtonButtonClicked);
    }

    public override void Show()
    {
        _useBoosterButton.interactable = true;
    }

    public override void Hide()
    {
        _useBoosterButton.interactable = false;
    }
    
    private void OnUseBoosterButtonButtonClicked()
    {
        UseBoosterButtonClicked?.Invoke();
    }
}
