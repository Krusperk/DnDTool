using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace DnDTool
{
    public partial class MainWindow : Window
    {
        MainPlaylistPlayer mainPlaylistPlayer = new MainPlaylistPlayer();
        SecondaryPlaylistPlayer secondaryPlaylistPlayer = new SecondaryPlaylistPlayer();
        uint backgroundMod = 0;
        const uint backgroundModsCount = 2;

        private void InitializeIcons()
        {
            BitmapImage play = new BitmapImage();
            play.BeginInit();
            play.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\images\play.png");
            play.EndInit();
            playImg.Source = play;

            BitmapImage pause = new BitmapImage();
            pause.BeginInit();
            pause.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\images\pause.png");
            pause.EndInit();
            pauseImg.Source = pause;

            BitmapImage stop = new BitmapImage();
            stop.BeginInit();
            stop.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\images\stop.png");
            stop.EndInit();
            stopImg.Source = stop;
        }

        public MainWindow()
        {
            InitializeComponent();
            setBackground();
            combatCmb.ItemsSource = mainPlaylistPlayer.playlists.Keys;
            combatCmb.DropDownClosed += (s, e) => { PlaySelected(s); };
            afterCombatCmb.ItemsSource = mainPlaylistPlayer.afterCombatPlaylists.Keys;
            afterCombatCmb.DropDownClosed += (s, e) => { PlaySelected(s); };
            mainPlaylistPlayer.hideMediaButtons += stopBtn_Click;
            secondaryPlaylistPlayer.mediaPlayer.MediaEnded += (s, e) => { mainPlaylistPlayer.TurnUpVolume(s); };
            secondaryPlaylistPlayer.turnDownMain += () => { mainPlaylistPlayer.TurnDownVolume(); };
            GenerateDynamicButtons();
            InitializeIcons();
        }

        private void PlayMain(string command = "")
        {
            playBtn.Visibility = Visibility.Collapsed;
            pauseBtn.Visibility = Visibility.Visible;
            stopBtn.Visibility = Visibility.Visible;

            mainPlaylistPlayer.PlayMain(command);
        }

        private void PlaySelected(object s)
        {
            ComboBox comboBox = (ComboBox)s;
            // Rozkliknutí comba a následné zrušení nesmí nic udělat
            if (comboBox.SelectedIndex != -1)
            {
                PlayMain(comboBox.Name + '|' + (string)comboBox.SelectedItem); ;
                comboBox.SelectedIndex = -1;
            }
        }

        #region Main events
        private void encounter_Click(object sender, RoutedEventArgs e)
        {
            PlayMain("encounter");
        }

        private void combatBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayMain("combat");
        }

        private void afterCombatBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayMain("after combat");
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayMain();
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            pauseBtn.Visibility = Visibility.Collapsed;
            playBtn.Visibility = Visibility.Visible;

            mainPlaylistPlayer.Pause();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            stopBtn.Visibility = Visibility.Collapsed;
            playBtn.Visibility = Visibility.Collapsed;
            pauseBtn.Visibility = Visibility.Collapsed;

            mainPlaylistPlayer.Stop();
        }

        private void repeatCheck_Checked(object sender, RoutedEventArgs e)
        {
            mainPlaylistPlayer.repeatP = true;
        }

        private void repeatCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            mainPlaylistPlayer.repeatP = false;
        }
        #endregion



        private void GenerateDynamicButtons()
        {
            foreach (string directoryNameFull in Directory.GetDirectories(secondaryPlaylistPlayer.audioDirectoryPath))
            {
                foreach (string subDirectoryName in Directory.GetDirectories(directoryNameFull).Select(x => System.IO.Path.GetFileName(x)))
                {
                    Button btn = new Button();
                    btn.Content = subDirectoryName;
                    btn.Style = (Style)FindResource("RoundCorner");
                    btn.Margin = new Thickness(0, 5, 0, 0);
                    btn.Click += generatedBtn_Click;

                    if (directoryNameFull.Contains("quotes"))
                    {
                        quotesButtonsGroup.Children.Add(btn);
                    }
                    else
                    {
                        soundsButtonsGroup.Children.Add(btn);
                    }
                }
            }
        }

        private void generatedBtn_Click(object sender, RoutedEventArgs e)
        {
            secondaryPlaylistPlayer.GeneratedButtonClicked((sender as Button).Content.ToString());
        }

        private void setBackground()
        {
            
            switch (backgroundMod)
            {
                case 0:
                    ImageBrush myBrush = new ImageBrush();
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\background.jpg"));
                    myBrush.ImageSource = image.Source;
                    mainGrid.Background = myBrush;
                    logoPlace.Children.Clear();
                    torchPlace.Children.Clear();
                    break;
                case 1:
                    mainGrid.Background = Brushes.Black;

                    Image logo = new Image();
                    logo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\dnd-logo.jpg"));
                    logo.MaxHeight = 150;
                    logo.MaxWidth = 150;
                    logoPlace.Children.Add(logo);

                    Image torch1 = new Image();
                    torch1.BeginInit();
                    torch1.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\torch1.gif"));
                    torch1.MaxHeight = 150;
                    torch1.MaxWidth = 75;
                    ImageBehavior.SetAnimatedSource(torch1, torch1.Source);
                    torchPlace.Children.Add(torch1);

                    Image torch2 = new Image();
                    torch2.BeginInit();
                    torch2.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\torch2.gif"));
                    torch2.MaxHeight = 150;
                    torch2.MaxWidth = 75;
                    ImageBehavior.SetAnimatedSource(torch2, torch2.Source);
                    torchPlace.Children.Add(torch2);
                    break;
            }
        }

        private void switchBackgroundBtn_Click(object sender, RoutedEventArgs e)
        {
            backgroundMod = ++backgroundMod % backgroundModsCount;
            setBackground();
        }
    }
}