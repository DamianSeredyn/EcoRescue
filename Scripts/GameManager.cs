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

    private void SetUpParameters()
    {
        _currentParameters = mainApp.Levels![_currentLevel].Parameters;
        mainApp.RefreshParameters(_currentParameters);
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
    public void StartGame(int levelId)
    {
        StopTimer();
        _gameFrozen = false;
        _currentLevel = levelId;
        SetUpParameters();
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