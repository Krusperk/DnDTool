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
        public List<Playlist> playlists = new List<Playlist>();
        protected Random rand = new Random();
        internal string audioDirectoryPath = Directory.GetCurrentDirectory() + @"\audio";

        internal void FillPlaylists(List<Playlist> _playlists, string path) 
        {
            foreach (string directoryNameFull in Directory.GetDirectories(path))
            {
                List<string> listOfTracks = new List<string>();
                foreach (string fileNameFull in Directory.GetFiles(directoryNameFull))
                {
                    listOfTracks.Add(fileNameFull);
                }

                _playlists.Add(new Playlist(directoryNameFull.Split('\\').Last(), listOfTracks));
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

        internal Playlist ReloadActualPlaylist(string actualPlaylistName, List<Playlist> _playlists)
        {
            // Coppy actual plylist
            var reloadedPlaylist = _playlists.Where(x => x.Name == actualPlaylistName).First().Coppy();

            this.LogActualPlaylist(reloadedPlaylist);

            // ... and also random order
            reloadedPlaylist.TrackList = reloadedPlaylist.TrackList
                                                            .OrderBy(x => rand.Next())
                                                            .ToList();
            return reloadedPlaylist;
        }

        internal Playlist RandomPlaylistFromPlaylists(List<Playlist> _playlists)
        {
            // Just coppy of playlist...
            var pickedPlaylist = _playlists.ElementAt(rand.Next(0, _playlists.Count)).Coppy();

            this.LogActualPlaylist(pickedPlaylist);

            // ... and also random order
            pickedPlaylist.TrackList = pickedPlaylist.TrackList
                                                        .OrderBy(x => rand.Next())
                                                        .ToList();
            return pickedPlaylist;
        }

        internal string RandomTrackFromPlaylist(Playlist playlist)
        {
            return playlist.TrackList[rand.Next(0, playlist.TrackList.Count)];
        }

        protected void LogActualPlaylist(Playlist playlist)
        {
            StringBuilder sb = new StringBuilder();
            playlist.TrackList.ForEach(x => sb.Append(x + '\n'));
            Logger.Log($"Actual playlist (random): {playlist.Name} \n{sb}");
        }
    }
}