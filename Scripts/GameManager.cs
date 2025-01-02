using System.Diagnostics;
using System.Windows.Threading;

namespace JPWP;

public class GameManager(App mainApp)
{
    private bool _gameFrozen;
    private int _currentLevel;
    private int _timeLeft;
    
    private DispatcherTimer? _timer;
    
    private int[]? _currentParameters;
    private int[]? _currentResources;
    private int[]? _currentIncome;
    private void InitTimer()
    {
        _timeLeft = mainApp.Levels![_currentLevel].Time;
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1) 
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
        mainApp.UpdateCountdownText(_timeLeft);
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        if(_gameFrozen) return;
        
        if (_timeLeft > 0)
        {
            CheckParameters();
            mainApp.RefreshParameters(_currentParameters);
            _timeLeft--;
            mainApp.UpdateCountdownText(_timeLeft);

            if (_timeLeft % 10 == 0)
            {
                AddNewResources();
            }
            RefreshData();
        }
        else
        {
            GameLost();
        }
    }

    private void GameLost()
    {
        _gameFrozen = true;
        StopTimer();
        _timeLeft = 0;
        mainApp.PlayerLose();
    }
    
    private void StopTimer()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
        }

        _timer = null;
    }

    private void AddNewResources()
    {
        Debug.Assert(_currentResources != null, nameof(_currentResources) + " != null");
        for (var index = 0; index < _currentResources.Length; index++)
        {
            Debug.Assert(_currentIncome != null, nameof(_currentIncome) + " != null");
            _currentResources[index] += _currentIncome[index];
        }
    }
    private void SetUpParameters()
    {
        _currentParameters = mainApp.Levels![_currentLevel].Parameters;
        mainApp.RefreshParameters(_currentParameters);
    }
    private void SetUpResources()
    {
        _currentResources = mainApp.Levels![_currentLevel].Resources;
        mainApp.RefreshResources(_currentResources);
    }
    private void SetUpIncome()
    {
        _currentIncome = mainApp.Levels![_currentLevel].StartIncome;
        mainApp.RefreshIncome(_currentIncome);
    }
    private void CheckParameters()
    {
        Debug.Assert(_currentParameters != null, nameof(_currentParameters) + " != null");
        foreach (var parameter in _currentParameters)
        {
            if(parameter>=100)
                GameLost();
        }
    }
    private void RefreshData()
    {
        mainApp.RefreshResources(_currentResources);
        mainApp.RefreshIncome(_currentIncome);  
    }
    public void StartGame(int levelId)
    {
        StopTimer();
        _gameFrozen = false;
        _currentLevel = levelId;
        SetUpParameters();
        SetUpResources();
        SetUpIncome();
        InitTimer();
    }
    public void ResetGame()
    {
        StartGame(_currentLevel);
    }

    public void SetGameState(bool val)
    {
        _gameFrozen = val;
    }
}