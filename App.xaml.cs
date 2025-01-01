﻿using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

namespace JPWP;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public List<Level>? Levels;

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
        _gameManager = new GameManager(this);
        MessageBox.Show("Witamy w EcoRescue!");
        
        _mainWindow .Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        MessageBox.Show("Do widzenia!");
    }

    private void LoadLevels()
    {
        Levels = new List<Level>();
        string dataPath = "Data/levels.json";
        if (File.Exists(dataPath))
        {
            string jsonContent = File.ReadAllText(dataPath);
            Levels = JsonSerializer.Deserialize<List<Level>>(jsonContent);

            // Wyświetl poziomy w konsoli
            if (Levels != null)
            {
                foreach (var level in Levels)
                {
                    Console.WriteLine($"Name: {level.Name}, Time: {level.Time}, Parameters: {string.Join(", ", level.Parameters)}");
                }
            }
        }
        else
        {
            Console.WriteLine("Plik JSON nie został znaleziony.");
        }

    }
    public void LoadGame(int levelId)
    {
        _mainWindow.PrepereUIForNewLevel(Levels?[levelId]);
       _gameManager.StartGame(levelId);
    }
    public void UpdateCountdownText(int time)
    {
        int minutes = time / 60; 
        int seconds = time % 60; 
        var timeText = $"{minutes:D2}:{seconds:D2}";
        _mainWindow.SetTimerText(timeText);
    }
}