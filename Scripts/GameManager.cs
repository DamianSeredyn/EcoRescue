using System.Diagnostics;
using System.Windows.Threading;
using Action = JPWP.Scripts.Action;

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

    private List<Action>? _availableActions;
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
                Debug.Assert(_availableActions != null, nameof(_availableActions) + " != null");
                mainApp.RefreshUiAction(_availableActions);
            }
            RefreshData();
            if(CheckIfPlayerWin())
                GameWin();
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

    private void GameWin()
    {
        _gameFrozen = true;
        StopTimer();
        
        mainApp.GameWon(mainApp.Levels![_currentLevel].Time-_timeLeft);
    }

    private bool CheckIfPlayerWin()
    {
        Debug.Assert(_currentParameters != null, nameof(_currentParameters) + " != null");
        for (var index = 0; index < _currentParameters.Length; index++)
        {
            var parameter = _currentParameters[index];
            if (index != 0 && index != 4)
            {
                if (parameter != 0)
                    return false;
            }
            else
            {
                if (parameter != 100)
                    return false;   
            }
            
        }

        return true;
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
        _currentParameters = (int[])mainApp.Levels![_currentLevel].Parameters!.Clone();
        mainApp.RefreshParameters(_currentParameters);
    }
    private void SetUpResources()
    {
        _currentResources = (int[])mainApp.Levels![_currentLevel].Resources!.Clone();
        mainApp.RefreshResources(_currentResources);
    }
    private void SetUpIncome()
    {
        _currentIncome = (int[])mainApp.Levels![_currentLevel].StartIncome!.Clone();
        mainApp.RefreshIncome(_currentIncome);
    }

    private void InitAction()
    {
        _availableActions = mainApp.AssignActions(mainApp.Levels![_currentLevel].Actions ?? throw new InvalidOperationException());
    }
    private void CheckParameters()
    {
        Debug.Assert(_currentParameters != null, nameof(_currentParameters) + " != null");
        for (var index = 0; index < _currentParameters.Length; index++)
        {
            var parameter = _currentParameters[index];
            if (index != 0 && index != 4)
            {
                if (parameter >= 100)
                    GameLost();
            }
            else
            {
                if (parameter == 0)
                    GameLost();    
            }
            
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
        InitAction();
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

    public void BuyAction(Action action)
    {
        Debug.Assert(_availableActions != null, nameof(_availableActions) + " != null");
        Debug.Assert(_currentResources != null, nameof(_currentResources) + " != null");
        for (var index = 0; index < _currentResources.Length; index++)
        {
            _currentResources[index] = Math.Max(_currentResources[index]-action.Costs[index], 0);
        }

        Debug.Assert(_currentIncome != null, nameof(_currentIncome) + " != null");
        Debug.Assert(_currentParameters != null, nameof(_currentParameters) + " != null");
        for (var index = 0; index < _currentIncome.Length+ _currentParameters.Length; index++)
        {
            if (index < _currentIncome.Length)
            {
                _currentIncome[index] += action.Revenues[index];
            }
            else
            {
                _currentParameters[index-_currentIncome.Length] += action.Revenues[index];
                _currentParameters[index-_currentIncome.Length] =  Math.Max(0,_currentParameters[index-_currentIncome.Length]);
                _currentParameters[index-_currentIncome.Length] =  Math.Min(100,_currentParameters[index-_currentIncome.Length]);
            }
        }

        RefreshData();
        _availableActions.Remove(action);

    }

    public bool CheckIfPlayerCanBuy(Action action)
    {
        Debug.Assert(_currentResources != null, nameof(_currentResources) + " != null");
        for (var index = 0; index < _currentResources.Length; index++)
        {
            if (_currentResources[index] < action.Costs[index])
            {
                return false;
            }
        }

        return true;
    }

    public List<Action>? GetActions() => _availableActions;
    public bool GetGameState() => _gameFrozen;
    public int GetCurrentLevelId() => _currentLevel;
}