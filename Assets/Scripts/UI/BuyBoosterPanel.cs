using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyBoosterPanel : BaseUIElement
{
    [SerializeField] private Button _buyBoosterButton;

    public event Action BuyBoosterButtonClicked;

    public void Init()
    {
        _buyBoosterButton.onClick.AddListener(OnBuyBoosterButtonClicked);
    }

    private void OnBuyBoosterButtonClicked()
    {
        BuyBoosterButtonClicked?.Invoke();
    }
}
