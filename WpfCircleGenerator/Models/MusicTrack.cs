using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCircleGenerator.Models
{
    public class MusicTrack
    {
        public static List<MusicTrack> AllMusic = new List<MusicTrack>(); //List that holds all registered music tracks.

        //-BACKING FIELDS-
        private int trackLenght;

        //-PROPERTIES-
        public string TrackGUID { get; private set; }
        public string TrackArtistNameString { get; set; }
        public string TrackName { get; set; }
        public int TrackLenght { 
            get
            {
                return trackLenght;
            }
            set
            {
                if (value >= 5)
                {
                    trackLenght = value;
                }
                else
                {
                    throw new Exception("Music tracks must have at least 5 seconds of lenght.");
                }
            }
        }
        public DateTime TrackReleaseDate { get; set; }

        //-CONSTRUCTOR-
        public MusicTrack(string TrackName, int TrackLenght, DateTime TrackReleaseDate)
        {
            TrackGUID = Guid.NewGuid().ToString();
            //this.TrackArtistString = TrackArtist;
            this.TrackName = TrackName;
            this.TrackLenght = TrackLenght;
            this.TrackReleaseDate = TrackReleaseDate;
            AllMusic.Add(this);

            //AllArtists.Add(TrackArtist);
        }
    }
}
