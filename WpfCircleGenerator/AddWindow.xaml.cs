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
using System.Windows.Shapes;
using WpfCircleGenerator.Models;

namespace WpfCircleGenerator
{
    public partial class AddWindow : Window
    {
        //This object enables access to MainWindow's XAML elements, so we can utilize them inside AddWindow's code behind.
        MainWindow MainWindowForm;
        
        public AddWindow(MainWindow MainWindowForm)
        {
            InitializeComponent();
            assignEventHandlers();

            //Assigns the constructor's MainWindowForm parameter (of type MainWindow) to the MainWindowForm variable of this class.
            this.MainWindowForm = MainWindowForm;
            
            //Initializes DataGrid and ComboBox.
            if (Artist.AllArtists.Count == 0)
            {
                artistComboBox.MaxDropDownHeight = 0;
            }
            else
            {
                artistComboBox.MaxDropDownHeight = 100;
            }
            artistsDataGrid.ItemsSource = Artist.AllArtists;
            artistComboBox.DisplayMemberPath = "ArtistName";
        }

        public void assignEventHandlers() //Assigns all needed event handlers
        {
            artistsDataGrid.SelectionChanged += OnSelectedArtist;
            addTrackButton.Click += AddMusic;
            addArtistButton.Click += AddArtist;
            removeArtistButton.Click += RemoveArtist;
            removeTrackButton.Click += RemoveTrack;
        }

        public void AddMusic(object sender, RoutedEventArgs e) //Event responsible for adding a new track to an artist's music repertoire.
        {
            //Checks if all the necessary fields for adding a new music track have the information needed.
            if (artistComboBox.SelectedItem != null && trackName.Text != "" && trackLenght.Text != "" && Convert.ToInt32(trackLenght.Text) >= 5 && releaseDate.SelectedDate != null)
            {
                //Creates an Artist type object with the values of artistComboBox.SelectedItem.
                Artist ComboBoxSelectedArtist = (Artist)artistComboBox.SelectedItem;

                //This checks if the artist has another song with the same name that's trying to be added.
                if (!ComboBoxSelectedArtist.ArtistTracks.Any(t => t.TrackName == trackName.Text))
                {
                    //Instantiation of MusicTrack type object in order to add the desired music to the artist's ArtistTracks list.
                    string TrackName = trackName.Text;
                    string TrackLenght = trackLenght.Text;
                    DateTime TrackReleaseDate = releaseDate.SelectedDate.Value;
                    MusicTrack NewMusicTrack = new MusicTrack(TrackName, Convert.ToInt32(TrackLenght), TrackReleaseDate);
                    
                    //Sets the TrackArtistNameString attribute of the newly created NewMusicTrack object to the ArtistName of ComboBoxSelectedArtist.
                    NewMusicTrack.TrackArtistNameString = ComboBoxSelectedArtist.ArtistName;

                    //Adds the NewMusicTrack object to the selected artist's ArtistTracks list.
                    ComboBoxSelectedArtist.ArtistTracks.Add(NewMusicTrack);

                    //Changes the label element containing the text on top of the datagrid.
                    XArtistMusicList.Content = $"{ComboBoxSelectedArtist.ArtistName}'s added tracks";

                    //Clears all the fields.
                    ClearAddNewTrackFields();
                    MessageBox.Show("Track added!");
                }
                else
                {
                    MessageBox.Show("This artist already has a track with this name! Choose another!");
                }

                //Refresh artistsMusicDataGrid to include the respective artist added tracks.
                artistsMusicDataGrid.ItemsSource = null;
                artistsMusicDataGrid.ItemsSource = ComboBoxSelectedArtist.ArtistTracks;
            }
            else
            {
                MessageBox.Show("Please fill all the required information before trying to add a new music track.");
            }

            //Refreshes the DataGrid in order to show the newly added music track.
            RefreshAllMusicDataGrid();
        }

        private void AddArtist(object sender, RoutedEventArgs e)
        {
            //Creates a disposable empty list in order to "clear" the artistMusicDataGrid datagrid.
            List<MusicTrack> DisposableEmptyList = new List<MusicTrack>();
            string ArtistName = artistName.Text;

            //Checks if the artist is already added to the database.
            if (!Artist.AllArtists.Any(a => a.ArtistName == ArtistName) && ArtistName != "")
            {
                Artist NewArtist = new Artist(ArtistName);

                //Refreshes the DataGrid in order to show the newly added artist.
                RefreshArtistsDataGrid();

                //Refreshes the ComboBox in order to show the newly added music track in the menu.
                RefreshArtistComboBox();

                //Changes the label element containing the text on top of the datagrid, and artistsMusicDataGrid to the DisposableEmptyList.
                XArtistMusicList.Content = $"Respective artist's added tracks";
                artistsMusicDataGrid.ItemsSource = DisposableEmptyList;
                artistComboBox.MaxDropDownHeight = 105;
                ClearAddArtistFields();
                MessageBox.Show("Artist added.");
            } 
            else if (ArtistName == "")
            {
                MessageBox.Show("You must first type a name before trying to add an artist.");
            }
            else
            {
                MessageBox.Show("This artist is already registered into the database.");
            }
        } //Event responsible for adding a new artist.

        private void OnSelectedArtist(object sender, RoutedEventArgs e) //Event responsible for displaying the selected artist's entire music repertoire.
        {
            Artist SelectedArtist = (Artist)artistsDataGrid.SelectedItem;
            if (SelectedArtist != null)
            {
                //Changes the artistsMusicDataGrid to the SelectedArtist's object ArtistTracks list in order to display the selected artist's tracks.
                artistsMusicDataGrid.ItemsSource = SelectedArtist.ArtistTracks;

                //Changes the label element containing the text on top of the datagrid.
                XArtistMusicList.Content = $"{SelectedArtist.ArtistName}'s added tracks";
            }
        }

        private void RemoveArtist(object sender, RoutedEventArgs e)
        {
            //Creates a disposable empty list in order to "clear" the datagrids.
            List<MusicTrack> DisposableEmptyList = new List<MusicTrack>();

            Artist SelectedArtist = (Artist)artistsDataGrid.SelectedItem;
            if (SelectedArtist != null)
            {
                MusicTrack.AllMusic.RemoveAll(t => t.TrackArtistNameString == SelectedArtist.ArtistName);
                Artist.AllArtists.Remove(SelectedArtist);
                MessageBox.Show($"Artist {SelectedArtist.ArtistName} removed.");

                //Changes the label element containing the text on top of the datagrid.
                XArtistMusicList.Content = $"Respective artist's added tracks";
            }
            else
            {
                MessageBox.Show("Please select an artist before trying to remove it.");
            }

            //This "turns off" the comboBox dropdown menu if there are no artists added.
            if (Artist.AllArtists.Count == 0)
            {
                artistComboBox.MaxDropDownHeight = 0;
            }
            RefreshAllMusicDataGrid();
            
            //Refreshes ComboBox object.
            RefreshArtistComboBox();

            //Refreshes the updated datagrids.
            RefreshArtistsDataGrid();
            artistsMusicDataGrid.ItemsSource = DisposableEmptyList;
        } //Event responsible for removing an artist/track.

        private void RemoveTrack(object sender, RoutedEventArgs e)
        {
            //Creates a disposable empty list in order to "clear" the datagrids.
            List<MusicTrack> DisposableEmptyList = new List<MusicTrack>();

            MusicTrack SelectedMusic = (MusicTrack)artistsMusicDataGrid.SelectedItem;
            if (SelectedMusic != null)
            {
                MusicTrack.AllMusic.Remove(SelectedMusic);
                Artist.AllArtists.Find(a => a.ArtistName == SelectedMusic.TrackArtistNameString).ArtistTracks.Remove(SelectedMusic);
                MessageBox.Show($"Track {SelectedMusic.TrackName}, from {SelectedMusic.TrackArtistNameString} removed.");
            }
            else
            {
                MessageBox.Show("Please select a track before trying to remove it.");
            }
            RefreshAllMusicDataGrid();

            //Refreshes the updated datagrids.
            RefreshArtistsDataGrid();

            artistsMusicDataGrid.ItemsSource = null;
            if (Artist.AllArtists.Count >= 1)
            {
                artistsMusicDataGrid.ItemsSource = Artist.AllArtists.Find(a => a.ArtistName == SelectedMusic.TrackArtistNameString).ArtistTracks;
            }
        } //Event responsible for removing an artist/track.

        private void RefreshAllMusicDataGrid()
        {
            MainWindowForm.allMusicDataGrid.ItemsSource = null;
            MainWindowForm.allMusicDataGrid.ItemsSource = MusicTrack.AllMusic;
        }

        private void RefreshArtistsDataGrid()
        {
            artistsDataGrid.ItemsSource = null;
            artistsDataGrid.ItemsSource = Artist.AllArtists;
        }

        private void RefreshArtistComboBox()
        {
            artistComboBox.ItemsSource = null;
            artistComboBox.ItemsSource = Artist.AllArtists;
        }

        private void ClearAddNewTrackFields()
        {
            trackName.Text = "";
            trackLenght.Text = "";
            releaseDate.SelectedDate = null;
            artistComboBox.Text = "Select an artist";
        }

        private void ClearAddArtistFields()
        {
            artistName.Text = "";
        }
    }
}
