using System.Windows.Threading;

namespace JPWP;

public class GameManager(App mainApp)
{
    private bool _gameFrozen;
    private int _currentLevel;
    private int _timeLeft;
    
    private DispatcherTimer? _timer;
    
    
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
            _timeLeft--;
            mainApp.UpdateCountdownText(_timeLeft);
        }
        else
        {
            StopTimer();
            mainApp.PlayerLose();
        }
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

    public void StartGame(int levelId)
    {
        StopTimer();
        _gameFrozen = false;
        _currentLevel = levelId;
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