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

    public void Activate()
    {
        _useBoosterButton.interactable = true;
    }

    public void Deactivate()
    {
        _useBoosterButton.interactable = false;
    }
    
    private void OnUseBoosterButtonButtonClicked()
    {
        UseBoosterButtonClicked?.Invoke();
    }
}
