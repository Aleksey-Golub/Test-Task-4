using System.Collections.Generic;

public class PauseManager : IPauseHandler
{
    private List<IPauseHandler> _handlers = new List<IPauseHandler>();

    public bool IsPaused { get; private set; }

    public void Register(IPauseHandler handler)
    {
        _handlers.Add(handler);
    }

    public void Unregister(IPauseHandler handler)
    {
        _handlers.Remove(handler);
    }

    public void SetPause(bool isPaused)
    {
        IsPaused = isPaused;
        foreach (var h in _handlers)
            h.SetPause(IsPaused);
    }
}
