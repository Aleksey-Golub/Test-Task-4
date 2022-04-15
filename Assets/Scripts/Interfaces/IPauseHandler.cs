public interface IPauseHandler
{
    public bool IsPaused { get; }
    void SetPause(bool isPaused);
}
