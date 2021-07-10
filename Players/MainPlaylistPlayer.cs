using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace DnDTool
{
    class MainPlaylistPlayer : PlaylistPlayer
    {
        public Dictionary<string, List<string>> afterCombatPlaylists = new Dictionary<string, List<string>>();
        List<string> actualPlaylist;
        string encounterPath = Directory.GetCurrentDirectory() + @"\audio\main\encounter.mp3";
        public bool repeatP;
        public bool combatP;
        public event Action<object, RoutedEventArgs> hideMediaButtons;

        public MainPlaylistPlayer() : base()
        {
            audioDirectoryPath += @"\main";
            mediaPlayer.MediaFailed += (s, e) => { MessageBox.Show("Something went wrong in main MediaPlayer:\n" + e.ToString()); };
            mediaPlayer.MediaEnded += (s, e) => { PlayNext(); };

            string[] groupsOfPlayLists = { @"\main\combat", @"\main\after combat" };

            FillPlaylists(playlists, audioDirectoryPath + @"\combat");
            FillPlaylists(afterCombatPlaylists, audioDirectoryPath + @"\after combat");
        }

        public void PlayMain(string command = "")
        {
            switch (command)
            {
                case "":
                    PlayContinue();
                    break;

                case "encounter":
                    PlayEncounter();
                    break;

                case "combat":
                    PlayCombat();
                    break;

                case "after combat":
                    PlayAfterCombat();
                    break;

                default:
                    // Zde se predava i ten selectnuty playlist s priznakem, jestli je to combat nebo after combat
                    string[] commandParts = command.Split('|');
                    if (commandParts[0] == "combatCmb")
                    {
                        PlayCombatSelected(commandParts[1]);
                    }
                    else
                    {
                        PlayAfterCombatSelected(commandParts[1]);
                    }
                    break;
            }
        }

        private void PlayNext()
        {
            // Encounter was played or playlist is empty and should repeat
            if (actualPlaylist == null || (actualPlaylist.Count == 0 && repeatP))
            {
                actualPlaylist = RandomPlaylistFromPlaylists(combatP ? playlists : afterCombatPlaylists);
            }
            if (actualPlaylist.Count != 0)
            {
                string actualTrack = actualPlaylist.First();
                actualPlaylist.RemoveAt(0);
                Play(actualTrack);
            }
            else
            {
                hideMediaButtons(null, null);
            }
        }

        public void PlayEncounter()
        {
            combatP = true;
            actualPlaylist = null;  
            Play(encounterPath);
        }

        public void PlayCombat()
        {
            combatP = true;
            actualPlaylist = RandomPlaylistFromPlaylists(playlists);
            PlayNext();
        }

        public void PlayCombatSelected(string playlist)
        {
            combatP = true;
            actualPlaylist = playlists[playlist].OrderBy(x => rand.Next()).ToList();
            PlayNext();
        }

        public void PlayAfterCombat()
        {
            combatP = false;
            actualPlaylist = RandomPlaylistFromPlaylists(afterCombatPlaylists);
            PlayNext();
        }

        public void PlayAfterCombatSelected(string playlist)
        {
            combatP = false;
            actualPlaylist = afterCombatPlaylists[playlist].OrderBy(x => rand.Next()).ToList();
            PlayNext();
        }

        public void PlayContinue()
        {
            Play();
        }

        public void Pause()
        {
            mediaPlayer.Pause();
        }

        public void Stop()
        {
            mediaPlayer.Stop();
        }

        public void TurnUpVolume(object sender)
        {
            mediaPlayer.Volume = 0.5;
        }

        public void TurnDownVolume()
        {
            mediaPlayer.Volume = 0.25;
        }
    }
}
