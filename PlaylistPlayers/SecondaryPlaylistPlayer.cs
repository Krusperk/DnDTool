using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace DnDTool
{
    class SecondaryPlaylistPlayer : PlaylistPlayer
    {
        public event Action turnDownMain;

        public SecondaryPlaylistPlayer() : base()
        {
            audioDirectoryPath += @"\secondary";
            mediaPlayer.MediaFailed += (s, e) => { MessageBox.Show("Something went wrong in secondary MediaPlayer:\n" + e.ToString()); };

            FillPlaylists(playlists, audioDirectoryPath + @"\quotes");
            FillPlaylists(playlists, audioDirectoryPath + @"\sounds");
        }

        public void GeneratedButtonClicked(string buttonName)
        {
            PlaySecond(RandomTrackFromPlaylist(playlists.Where(x => x.Name == buttonName).FirstOrDefault()));
        }

        public void PlaySecond(string path = "")
        {
            turnDownMain();
            Logger.Log($"Actual track (second): {string.Join("/", path.Split(Path.DirectorySeparatorChar).Reverse().Take(4).Reverse())}");
            Play(path);
        }
    }
}