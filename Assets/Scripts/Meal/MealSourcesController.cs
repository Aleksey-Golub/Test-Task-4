using System.Collections.Generic;
using UnityEngine;

public class MealSourcesController : MonoBehaviour, IPauseHandler
{
    [SerializeField] private List<MealSource> _sources;

    private MealFactory _mealFactory;
    private ClientsController _clientsController;

    public bool IsPaused { get; private set; }

    public void Init(ClientsController clientsController, MealFactory mealFactory)
    {
        _mealFactory = mealFactory;
        _clientsController = clientsController;
        foreach (var s in _sources)
        {
            s.Init(_mealFactory);
            s.Clicked += OnSourceClicked;
        }
    }

    public void SetPause(bool isPaused)
    {
        IsPaused = isPaused;

        foreach (var s in _sources)
            s.SetPause(IsPaused);
    }

    private void OnSourceClicked(MealSource source)
    {
        bool result = _clientsController.TryReceive(source.Type);
    }
}
