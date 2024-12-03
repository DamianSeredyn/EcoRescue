using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JPWP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private App _mainApp;
    public MainWindow(App mainApp)
    {
        this._mainApp = mainApp;
        InitializeComponent();
    }

    public void PrepereUIForNewLevel(Level level)
    {
        Game.Visibility = Visibility.Visible;
    }
    private void NewGameButton_Click(object sender, RoutedEventArgs e)
    {
        SendInfoToLoadLevel(0);
    }

    private void LoadGameButton_Click(object sender, RoutedEventArgs e)
    {
        DisableAllWindows();
        LoadGame.Visibility = Visibility.Visible;
    }

    private void OptionsButton_Click(object sender, RoutedEventArgs e)
    {
        DisableAllWindows();
        Option.Visibility = Visibility.Visible;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void LoadLevelButton_Pressed(object sender, RoutedEventArgs e)
    {
        if (sender is Button button) // Rzutowanie sendera na przycisk
        {
            if (int.TryParse(button.Tag.ToString(), out int argument)) // Konwersja Tag na int
            {
                SendInfoToLoadLevel(argument);
            }
            else
            {
                MessageBox.Show("Nie udało się przekonwertować argumentu na liczbę.");
            }
        }
    }
    private void Return_Pressed(object sender, RoutedEventArgs e)
    {
        DisableAllWindows();
        MainMenu.Visibility = Visibility.Visible;
    }


    private void DisableAllWindows()
    {
        MainMenu.Visibility = Visibility.Hidden;
        LoadGame.Visibility = Visibility.Hidden;
        Option.Visibility = Visibility.Hidden;
    }

    private void SendInfoToLoadLevel(int id)
    {
        MainMenu.Visibility = Visibility.Hidden;
        _mainApp.LoadGame(id);
    }
}