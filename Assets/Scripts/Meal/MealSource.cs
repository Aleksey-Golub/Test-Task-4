using System;
using UnityEngine;

public class MealSource : MonoBehaviour, IPauseHandler
{
    [SerializeField] private MealSourceView _view;
    [SerializeField] private MealType _type;

    public MealType Type => _type;
    public bool IsPaused { get; private set; }
    public event Action<MealSource> Clicked;

    public void Init(MealFactory mealFactory)
    {
        _view.Init(mealFactory);
        _view.View(_type);
    }

    public void SetPause(bool isPaused)
    {
        IsPaused = isPaused;
    }

    private void OnMouseDown()
    {
        if (IsPaused)
            return;

        Clicked?.Invoke(this);
    }

    private void OnValidate()
    {
        gameObject.name = $"{ _type} Source";
    }
}
