using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Action = JPWP.Scripts.Action;


namespace JPWP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly App _mainApp;
    public MainWindow(App mainApp)
    {
        this._mainApp = mainApp;
        InitializeComponent();
    }

    public void PrepareUiForNewLevel(Level? level)
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
        SetColorAndText(Slider1Text, val[0], val[0] + "%", true);
        SetColorAndText(Slider2Text,val[1], val[1]+ "%", false);
        SetColorAndText(Slider3Text, val[2],val[2] + "%", false);
        SetColorAndText(Slider4Text, val[3],val[3].ToString(), false);
        SetColorAndText(Slider5Text, val[4],val[4] + "%", true);
    }
    
    public void RefreshRes(int[]? val)
    {
        if(val == null) return;
        Res1.Text = val[0].ToString();
        Res2.Text = val[1].ToString();
        Res3.Text = val[2].ToString();
        Res4.Text = val[3].ToString();
    }

    public void RefreshIncome(int[]? val)
    {
        List<TextBlock> textBlocks = [PredictedRevenueText1, PredictedRevenueText2, PredictedRevenueText3, PredictedRevenueText4];
        if(val == null) return;
        for (var index = 0; index < textBlocks.Count; index++)
        {
            if (val[index] >= 0)
            {
                textBlocks[index].Text = "+" +val[index].ToString();
                textBlocks[index].Foreground = Brushes.Green;       
            }
            else
            {
                textBlocks[index].Text = val[index].ToString();
                textBlocks[index].Foreground = Brushes.Red;
            }
        }
    }
    public void EnableGameOverScreen()
    {
        DisableAllGameWindows();
        GameOverPanel.Visibility = Visibility.Visible;
    }

    public void EnableGameWinScreen(int time)
    {
        DisableAllGameWindows();
        GameWinScreen.Visibility = Visibility.Visible;
        TimeSpan x = TimeSpan.FromSeconds(time);
        string formattedTime = $"Czas: {x.Minutes:D2}:{x.Seconds:D2}";
        TimeText.Text = formattedTime;
    }

    public void DisableAllGameWindows()
    {
        GameWinScreen.Visibility = Visibility.Hidden;
        GameOverPanel.Visibility = Visibility.Hidden;
        MiniMenu.Visibility = Visibility.Hidden;
        ActionMenu.Visibility = Visibility.Hidden;
    }
    

    private void SetColorAndText(TextBlock textBlock,int val, string text, bool pos)
    {
        if (pos)
        {
            switch (val)
            {
                case <10:
                    textBlock.Foreground = Brushes.Red;
                    break;
                case <90:
                    textBlock.Foreground = Brushes.Orange;
                    break;
                default:
                    textBlock.Foreground = Brushes.Green;
                    break;
            }
        }
        else
        {
            switch (val)
            {
                case <10:
                    textBlock.Foreground = Brushes.Green;
                    break;
                case <90:
                    textBlock.Foreground = Brushes.Orange;
                    break;
                default:
                    textBlock.Foreground = Brushes.Red;
                    break;
            }
        }

        textBlock.Text = text;
    }
    public void CreateActions(List<Action>? actions)
    {
     var gameManager = _mainApp.GetGameManager();
    // Wyczyść poprzednie elementy
    ActionPanelStack.Children.Clear();

    Debug.Assert(actions != null, nameof(actions) + " != null");
    foreach (var action in actions) {
                var actionPanel = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(10),
                    Padding = new Thickness(10),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2)
                };

                var grid = new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };

                // Wiersze
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Nazwa akcji
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Koszt
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Zysk (Efekt)

                // Kolumny
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Koszt / Zysk
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Guzik

                // 1. Nazwa akcji
                var nameTextBlock = new TextBlock
                {
                    Text = action.Name,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(nameTextBlock, 0);
                Grid.SetColumn(nameTextBlock, 0);
                grid.Children.Add(nameTextBlock);

                // 2. Koszt akcji
                var costStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var costLabel = new TextBlock
                {
                    Text = "Koszty: ",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };
                costStackPanel.Children.Add(costLabel);

                for (int i = 0; i < action.Costs.Length; i++)
                {
                    if(action.Costs[i] == 0) continue;
                    var costPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(5)
                    };
                    var imgSrc = "Resources/";
                    switch(i)
                    {
                        case 0:
                            imgSrc += "money.png";
                            break;
                        case 1:
                            imgSrc += "workers.png";
                            break;
                        case 2:
                            imgSrc += "Materials.png";
                            break;
                        case 3:
                            imgSrc += "research.png";
                            break;
                    }
                    var image = new Image
                    {
             
                        Source = new BitmapImage(new Uri(imgSrc, UriKind.Relative)),
                        Width = 32,
                        Height = 32
                    };

                    var costText = new TextBlock
                    {
                        Text = action.Costs[i].ToString(),
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5, 0, 0, 0),
                        FontWeight = FontWeights.Bold
                    };
                    costPanel.Children.Add(image);
                    costPanel.Children.Add(costText);
                    costStackPanel.Children.Add(costPanel);
                }

                Grid.SetRow(costStackPanel, 1);
                Grid.SetColumn(costStackPanel, 0);
                grid.Children.Add(costStackPanel);

                // 3. Zysk akcji (Efekty)
                var revenueStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Tekst "Efekt:"
                var effectLabel = new TextBlock
                {
                    Text = "Efekt: ",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };
                revenueStackPanel.Children.Add(effectLabel);

                for (int i = 0; i < action.Revenues.Length; i++)
                {
                    if(action.Revenues[i] == 0) continue;
                    var revenuePanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(5)
                    };
                    var imgSrc = "Resources/";
                    switch(i)
                    {
                        case 0:
                            imgSrc += "money.png";
                            break;
                        case 1:
                            imgSrc += "workers.png";
                            break;
                        case 2:
                            imgSrc += "Materials.png";
                            break;
                        case 3:
                            imgSrc += "research.png";
                            break;
                        case 4:
                            imgSrc += "Happiness.png";
                            break;    
                        case 5:
                            imgSrc += "airPollution.png";
                            break;   
                        case 6:
                            imgSrc += "WaterPollution.png";
                            break;   
                        case 7:
                            imgSrc += "temperature.png";
                            break;   
                        case 8:
                            imgSrc += "biodiversity.png";
                            break;   
                    }
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(imgSrc, UriKind.Relative)),
                        Width = 32,
                        Height = 32
                    };

                    var revenueText = new TextBlock
                    {
                        Text = action.Revenues[i].ToString(),
                       
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5, 0, 0, 0)
                    };
                    if (i is < 4 or 4 or 8 )
                    {
                        if (action.Revenues[i] >= 0)
                        {
                            revenueText.Text = "+" + revenueText.Text;
                            revenueText.Foreground = Brushes.Green;
                        }
                        else
                        {
                            revenueText.Foreground = Brushes.Red;
                        } 
                    }
                    else
                    {
                        if (action.Revenues[i] >= 0)
                        {
                            revenueText.Text = "+" + revenueText.Text;
                            revenueText.Foreground = Brushes.Red;
                        }
                        else
                        {
                            revenueText.Foreground = Brushes.Green;
                        } 
                    }
                    revenuePanel.Children.Add(image);
                    revenuePanel.Children.Add(revenueText);
                    revenueStackPanel.Children.Add(revenuePanel);
                }
                
                Grid.SetRow(revenueStackPanel, 2);
                Grid.SetColumn(revenueStackPanel, 0);
                grid.Children.Add(revenueStackPanel);

                // 4. Guzik "Kup"
                if (gameManager.CheckIfPlayerCanBuy(action))
                {
                    var buyButton = new Button
                    {
                        Content = "Kup",
                        Background = new SolidColorBrush(Color.FromRgb(50, 200, 50)),
                        Foreground = Brushes.White,
                        Width = 75,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    
                    };
                    buyButton.Click += (_, _) => _mainApp.GetGameManager().BuyAction(action);
                    buyButton.Click += (_, _) => CreateActions(gameManager.GetActions());
                    Grid.SetRow(buyButton, 1);
                    Grid.SetColumn(buyButton, 1);
                    grid.Children.Add(buyButton);
                }
                else
                {
                    var buyButton = new Button
                    {
                        Content = "Nie stać cię!",
                        Background = new SolidColorBrush(Color.FromRgb(210, 4, 45)),
                        Foreground = Brushes.Black,
                        IsEnabled=false,
                        Width = 75,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        
                    
                    };
                    Grid.SetRow(buyButton, 1);
                    Grid.SetColumn(buyButton, 1);
                    grid.Children.Add(buyButton);
                }
                

                // Dodaj panel do głównego kontenera
                actionPanel.Child = grid;
                if (gameManager.CheckIfPlayerCanBuy(action))
                {
                    ActionPanelStack.Children.Insert(0, actionPanel);
                }
                else
                {
                    ActionPanelStack.Children.Add(actionPanel);
                }
    }
        
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
    private void NextLevelEvent(object sender, RoutedEventArgs e)
    {
        _mainApp.LoadGame(_mainApp.GetGameManager().GetCurrentLevelId()+1);
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
    private void ActionMenuEventClick(object sender, RoutedEventArgs e)
    {
        if (_mainApp.GetGameManager().GetGameState()) return;
        DisableAllGameWindows();
        ActionMenu.Visibility = Visibility.Visible;
        var levels = _mainApp.GetGameManager().GetActions();
        CreateActions(levels);
    }
    private void ActionMenuExitEventClick(object sender, RoutedEventArgs e)
    {
        ActionMenu.Visibility = Visibility.Hidden;
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