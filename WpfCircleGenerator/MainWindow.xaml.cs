using System;
using System.Collections.Generic;
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
using WpfCircleGenerator.Models;

namespace WpfCircleGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            allMusicDataGrid.ItemsSource = MusicTrack.AllMusic; //Sets the allMusicDataGrid's ItemSource property to the AllMusic list inside the MusicTrack class.
            assignEventHandlers();
        }

        private void AddMusicWindow(object sender, RoutedEventArgs e)
        {
            AddWindow Window = new AddWindow(this); //Creates a new AddWindow object, and passing a MainWindow as a parameter using its constructor.
            Window.Show();
        }

        private void PlayMusic(object sender, RoutedEventArgs e)
        {
            if (MusicTrack.AllMusic.Count() > 0)
            {
                MusicTrack SelectedMusicTrack = (MusicTrack)allMusicDataGrid.SelectedItem;
                MessageBox.Show($"Now playing {SelectedMusicTrack.TrackName}, from {SelectedMusicTrack.TrackArtistNameString}.");
            }
            else
            {
                MessageBox.Show($"No tracks added. Add a track first.");
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            allMusicDataGrid.ItemsSource = null;
            allMusicDataGrid.ItemsSource = MusicTrack.AllMusic;
        }
        
        private void assignEventHandlers()
        {
            addMusicButton.Click += AddMusicWindow;
            playMusicButton.Click += PlayMusic;
        }

    }
}
