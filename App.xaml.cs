using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;
using JPWP.Scripts;
using Action = JPWP.Scripts.Action;

namespace JPWP;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public List<Level>? Levels;
    public List<Action>? AllActions;
    public List<Event>? AllEvents;
    private readonly MainWindow _mainWindow;
    private GameManager _gameManager;


    public App(MainWindow mainWindow, List<Level>? levels, GameManager gameManager)
    {
        _mainWindow = mainWindow;
        Levels = levels;
        _gameManager = gameManager;
    }
    public App()
    {
        _gameManager =  new GameManager(this);
        _mainWindow = new MainWindow(this);;
        Levels = new List<Level>();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        LoadLevels();
        LoadAction();
        LoadEvents();
        _gameManager = new GameManager(this);
        
        _mainWindow .Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {

    }

    private void LoadLevels()
    {
        Levels = new List<Level>();
        string dataPath = "Data/levels.json";
        if (File.Exists(dataPath))
        {
            string jsonContent = File.ReadAllText(dataPath);
            Levels = JsonSerializer.Deserialize<List<Level>>(jsonContent);
        }
        else
        {
            Console.WriteLine("Plik JSON nie został znaleziony.");
        }

    }

    private void LoadAction()
    {
        AllActions = new List<Action>();
        string dataPath = "Data/actions.json";
        if (File.Exists(dataPath))
        {
            string jsonContent = File.ReadAllText(dataPath);
            AllActions = JsonSerializer.Deserialize<List<Action>>(jsonContent);
        }
        else
        {
            Console.WriteLine("Plik JSON nie został znaleziony.");
        }
    }
    private void LoadEvents()
    {
        AllEvents = new List<Event>();
        string dataPath = "Data/Events.json";
        if (File.Exists(dataPath))
        {
            string jsonContent = File.ReadAllText(dataPath);
            AllEvents = JsonSerializer.Deserialize<List<Event>>(jsonContent);
        }
        else
        {
            Console.WriteLine("Plik JSON nie został znaleziony.");
        }
    }
    public void LoadGame(int levelId)
    {
        _mainWindow.DisableAllGameWindows();
        _mainWindow.PrepareUiForNewLevel(Levels?[levelId]);
       _gameManager.StartGame(levelId);
    }
    
    public void ResetCurrentLevel()
    {
        _mainWindow.DisableAllGameWindows();
        _gameManager.ResetGame();
    }

    public void HandleMiniMenu(bool val)
    {
        _gameManager.SetGameState(val);
    }
    public void UpdateCountdownText(int time)
    {
        int minutes = time / 60; 
        int seconds = time % 60; 
        var timeText = $"{minutes:D2}:{seconds:D2}";
        _mainWindow.SetTimerText(timeText);
    }

    public void PlayerLose()
    {
        _mainWindow.DisableAllGameWindows();
        _mainWindow.EnableGameOverScreen();
    }

    public void RefreshParameters(int[]? val)
    {
        _mainWindow.RefreshSliders(val);
    }
    public void RefreshResources(int[]? val)
    {
        _mainWindow.RefreshRes(val);
    } 
    public void RefreshIncome(int[]? val)
    {
        _mainWindow.RefreshIncome(val);
    }

    public List<Action>? AssignActions(int[] actionIDs)
    {
        List<Action>? actions = new List<Action>();
        foreach (var actionId in actionIDs)
        {
            Debug.Assert(AllActions != null, nameof(AllActions) + " != null");
            foreach (var action in AllActions)
            {
                if (action.Id != actionId) continue;
                actions.Add(action);
                break;
            }
        }

        return actions;
    }

    public List<Event> AssignEvents()
    {
        List<Event>? events = new List<Event>();
        Debug.Assert(AllEvents != null, nameof(AllEvents) + " != null");
        foreach (var eventOjbect in AllEvents) 
        {
            events.Add(eventOjbect);
        }
        return events;
    }
    public void RefreshUiAction(List<Action> actions)
    {
        _mainWindow.CreateActions(actions);
    }

    public void GameWon(int time)
    {
        _mainWindow.EnableGameWinScreen(time);
    }

    public void GenerateEvent(int x, int y,Event gameEvent)
    {
        _mainWindow.CreateEventIcon(x,y,gameEvent);
    }
    public GameManager GetGameManager() => _gameManager;
}