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

    public void PrepereUIForNewLevel(Level? level)
    {
        Menu.Visibility = Visibility.Hidden;
        Game.Visibility = Visibility.Visible;
    }
    
    public void SetTimerText(string text)
    {
        TimerText.Text = text;
    }

    public void RefreshSliders(int[]? val)
    {
        if(val == null) return;
        Slider1.Value = val[0];
        Slider2.Value = val[1];
        Slider3.Value = val[2];
        Slider4.Value = val[3];
        Slider5.Value = val[4];

        Slider1Text.Text = val[0].ToString() + "%";
        Slider2Text.Text = val[1].ToString() + "%";
        Slider3Text.Text = val[2].ToString() + "%";
        Slider4Text.Text = val[3].ToString();
        Slider5Text.Text = val[4].ToString() + "%";
    }
    public void EnableGameOverScreen()
    {
        GameOverPanel.Visibility = Visibility.Visible;
    }

    public void DisableAllGameWindows()
    {
        GameOverPanel.Visibility = Visibility.Hidden;
        MiniMenu.Visibility = Visibility.Hidden;
    }
    private void ExitToMenuEvent(object sender, RoutedEventArgs e)
    {
        Menu.Visibility = Visibility.Visible;
        Game.Visibility = Visibility.Hidden;
    }
    private void ResetGameEvent(object sender, RoutedEventArgs e)
    {
        _mainApp.ResetCurrentLevel();
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
    
    private void ContinueButtonEvent(object sender, RoutedEventArgs e)
    {
        MiniMenu.Visibility = Visibility.Hidden;
        _mainApp.HandleMiniMenu(false);
    }
    private void OptionsButtonMiniMenuEvent(object sender, RoutedEventArgs e)
    {

    }
   
    private void MiniMenuEvent(object sender, RoutedEventArgs e)
    {
        MiniMenu.Visibility = Visibility.Visible;
        _mainApp.HandleMiniMenu(true);
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
        Menu.Visibility = Visibility.Hidden;
        _mainApp.LoadGame(id);
    }
}