using System.Windows.Threading;

namespace JPWP;

public class GameManager(App mainApp)
{
    private int _currentLevel;
    private int _timeLeft;
    
    private DispatcherTimer? _timer;

    public void StartGame(int levelID)
    {
        _currentLevel = levelID;
        InitTimer();
    }

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
        if (_timeLeft > 0)
        {
            _timeLeft--;
            mainApp.UpdateCountdownText(_timeLeft);
        }
        else
        {
            _timer?.Stop();
            // Time out!
        }
    }
}