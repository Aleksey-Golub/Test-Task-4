using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MealSourcesController _mealSourcesController;
    [SerializeField] private MealFactory _mealFactory;
    [SerializeField] private ClientsController _clientsController;
    [SerializeField] private UIController _uiController;
    [Header("Settings")]
    [Tooltip("Current Level Name. Use for load Level Data")]
    [SerializeField] private string _currentLevelName;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private JsonSaveLoader _saveLoader;
    [SerializeField] private string _filePath = "Assets/LevelData/";

    private LevelTimer _levelTimer;
    private PauseManager _pauseManager;

    private void Start()
    {
        LoadLevelData();

        _pauseManager = new PauseManager();
        _pauseManager.Register(_mealSourcesController);

        _levelTimer = new LevelTimer(_levelData.Timer);
        _levelTimer.Updated += OnLevelTimerNeedUpdate;
        _levelTimer.TimeIsOver += OnTimeIsOver;

        _mealSourcesController.Init(_clientsController, _mealFactory);

        _clientsController.ClientCountChanged += OnClientCountChanged;
        _clientsController.Init(_levelData.Orders, _mealFactory);
        _clientsController.AllCliensAreServed += OnAllCliensAreServed;

        _uiController.Init();
        _uiController.UpdateBoosterButton(_levelData.BoosterCount > 0);
        _uiController.RestartButtonClicked += OnRestartButtonClicked;
        _uiController.BuyBoosterButtonClicked += OnBuyBoosterButtonClicked;
        _uiController.UseBoosterButtonClicked += OnUseBoosterButtonClicked;
    }

    private void Update()
    {
        if (_pauseManager.IsPaused)
            return;

        float deltaTime = Time.deltaTime;

        _clientsController.CustomUpdate(deltaTime);
        _levelTimer.CustomUpdate(deltaTime);
    }

    private void OnAllCliensAreServed()
    {
        _pauseManager.SetPause(true);
        _uiController.ShowWinScreen();
    }

    private void OnClientCountChanged(int waitingToEnter, int total)
    {
        _uiController.UpdateClientCounter(waitingToEnter, total);
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTimeIsOver()
    {
        _pauseManager.SetPause(true);
        _uiController.ShowLoseScreen();
    }

    private void OnLevelTimerNeedUpdate(int minutes, int seconds)
    {
        _uiController.UpdateTimer(minutes, seconds);
    }

    private void OnBuyBoosterButtonClicked()
    {
        _levelData.BoosterCount++;
        _uiController.UpdateBoosterButton(_levelData.BoosterCount > 0);
        _uiController.HideBuyBoosterPanel();
        _pauseManager.SetPause(false);
    }

    private void OnUseBoosterButtonClicked()
    {
        if (_pauseManager.IsPaused)
            return;

        if (_clientsController.CompleteFirstInQueueOrder())
        {
            _levelData.BoosterCount--;
            _uiController.UpdateBoosterButton(_levelData.BoosterCount > 0);
        }

        if (_levelData.BoosterCount == 0)
        {
            _pauseManager.SetPause(true);
            _uiController.ShowBuyBoosterPanelScreen();
        }
    }

    [ContextMenu("SAVE Level Data in Json")]
    private void SaveLevelData()
    {
        string path = $"{_filePath}{_levelData.Name}.json";

        _saveLoader.SaveLevelData(_levelData, path);

        Debug.Log($"Level Data Saved in {path}\n Refresh Tab: Ctrl+R");
    }

    [ContextMenu("LOAD Level Data from Json")]
    private void LoadLevelData()
    {
        string path = $"{_filePath}{_currentLevelName}.json";

        try
        {
            _levelData = _saveLoader.LoadLevelData(path);

            Debug.Log("Level Data Loaded");
        }
        catch
        {
            Debug.LogError($"File with path {path} not found");
        }
    }
}
