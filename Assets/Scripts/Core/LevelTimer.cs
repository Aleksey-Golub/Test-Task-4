using System;

public class LevelTimer : IUpdatable
{
    private float _value;
    private int _seconds;

    public event Action<int, int> Updated;
    public event Action TimeIsOver;

    public LevelTimer(float startValue)
    {
        _value = startValue;
        _seconds = 0;

        CheckNeedUpdate();
    }

    public void CustomUpdate(float deltaTime)
    {
        _value -= deltaTime;

        if (_value <= 0)
        {
            TimeIsOver?.Invoke();
            return;
        }

        CheckNeedUpdate();
    }

    private void CheckNeedUpdate()
    {
        int minutes = (int)_value / 60;
        int seconds = (int)_value % 60;
        if (_seconds != seconds)
        {
            _seconds = seconds;
            Updated?.Invoke(minutes, seconds);
        }
    }
}
