using System.Configuration;
using System.Data;
using System.Windows;

namespace JPWP;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private List<Level> _levels;

    private MainWindow _mainWindow;

    public App(MainWindow mainWindow, List<Level> levels)
    {
        _mainWindow = mainWindow;
        _levels = levels;
    }
    public App()
    {
        _mainWindow = new MainWindow(this);;
        _levels = new List<Level>();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        _levels = new List<Level>();
        _levels.Add(new Level()); // TEST 
        MessageBox.Show("Witamy w EcoRescue!");
        
        _mainWindow .Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        MessageBox.Show("Do widzenia!");
    }


    public void LoadGame(int levelId)
    {
        _mainWindow.PrepereUIForNewLevel(_levels?[levelId]);
    }
}