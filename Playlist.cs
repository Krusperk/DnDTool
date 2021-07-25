using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDTool
{
    public class Playlist
    {
        public string Name;
        public List<string> TrackList;

        public Playlist(string name, List<string> trackList)
        {
            Name = name;
            TrackList = trackList;
        }

        public Playlist() { }

        public Playlist Coppy()
        {
            return new Playlist(Name, new List<string>(TrackList));
        }
    }
}
