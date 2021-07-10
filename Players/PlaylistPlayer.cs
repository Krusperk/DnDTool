using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace DnDTool
{
    abstract class PlaylistPlayer
    {
        internal MediaPlayer mediaPlayer = new MediaPlayer();
        public Dictionary<string, List<string>> playlists = new Dictionary<string, List<string>>();
        protected Random rand = new Random();
        internal string audioDirectoryPath = Directory.GetCurrentDirectory() + @"\audio";

        internal void FillPlaylists(Dictionary<string, List<string>> _playlists, string path) 
        {
            foreach (string directoryNameFull in Directory.GetDirectories(path))
            {
                List<string> listOfTracks = new List<string>();
                foreach (string fileNameFull in Directory.GetFiles(directoryNameFull))
                {
                    listOfTracks.Add(fileNameFull);
                }

                _playlists.Add(directoryNameFull.Split('\\').Last(), listOfTracks);
            }
        }

        internal virtual void Play(string path = "")
        {
            if (path != "")
            {
                mediaPlayer.Open(new Uri(path));
            }
            mediaPlayer.Play();
        }

        internal List<string> RandomPlaylistFromPlaylists(Dictionary<string, List<string>> _playlists)
        {
            // Just coppy of playlist and also random ordered
            return _playlists.ElementAt(rand.Next(0, _playlists.Count)).Value
                .OrderBy(x => rand.Next())
                .ToList();
        }

        internal string RandomTrackFromPlaylist(List<string> playlist)
        {
            return playlist[rand.Next(0, playlist.Count)];
        }
    }
}