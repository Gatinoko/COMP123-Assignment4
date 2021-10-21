using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfCircleGenerator.Models
{
    public class Artist
    {
        public static List<Artist> AllArtists = new List<Artist>(); //List that holds all registered artists.

        //-PROPERTIES-
        public string ArtistGUID { get; set; }
        public string ArtistName { get; set; }
        public List<MusicTrack> ArtistTracks { get; set; }

        //-CONSTRUCTOR- 
        public Artist(string ArtistName)
        {
            ArtistGUID = Guid.NewGuid().ToString();
            this.ArtistName = ArtistName;
            this.ArtistTracks = new List<MusicTrack>(); //Initializes ArtistTracks as an empty list on each object instance.
            AllArtists.Add(this); //This adds the newly created Artist object ArtistName property to a list with al the names.
        }

        //-METHODS-
        public static void CreateArtist(Artist Artist)
        {
            var Serializer = new XmlSerializer(typeof(Artist));
            using (var Stream = new FileStream($@"Data\Artists\{Artist.ArtistGUID}.xml", FileMode.Create))
            {
                Serializer.Serialize(Stream, Artist);
            }
        }
    }
}
