using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _orderSpriteRenderers;
    [SerializeField] private GameObject _order;

    private MealFactory _mealFactory;

    public void Init(IReadOnlyList<MealType> order, MealFactory mealFactory)
    {
        _mealFactory = mealFactory;
        UpdateOrder(order);
        HideOrder();
    }

    public void ShowOrder()
    {
        _order.SetActive(true);
    }

    public void HideOrder()
    {
        _order.SetActive(false);
    }

    public void UpdateOrder(IReadOnlyList<MealType> order)
    {
        for (int i = 0; i < _orderSpriteRenderers.Length; i++)
        {
            if (i < order.Count)
            {
                _orderSpriteRenderers[i].enabled = true;
                _orderSpriteRenderers[i].sprite = _mealFactory.GetSprite(order[i]);
            }
            else
            {
                _orderSpriteRenderers[i].enabled = false;
            }
        }
    }
}
