using System.Configuration;
using System.Data;
using System.Windows;

namespace JPWP;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private List<Level>? _levels;
    
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        _levels = new List<Level>();
        
        MessageBox.Show("Witamy w EcoRescue!");
        
        var mainWindow = new MainWindow(this);
        mainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        MessageBox.Show("Do widzenia!");
    }


    public void LoadGame(int LevelID)
    {
        
    }
}