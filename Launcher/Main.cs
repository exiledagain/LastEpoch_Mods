using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Launcher
{
    public partial class Main : Form
    {
        private static string game_folder_path = Application.StartupPath + @"\";
        private static string game_filename = @"Last Epoch.exe";
        private static string version_filename = @"version.dll";
        private static string version_back_filename = @"version.dll.back";

        public Main()
        {
            if (File.Exists(game_folder_path + game_filename)) { InitializeComponent(); }
            else { Application.Exit(); }
        }

        private void StartGame(bool online)
        {
            if (online)
            {
                if (File.Exists(game_folder_path + version_filename))
                {
                    if (File.Exists(game_folder_path + version_back_filename)) { File.Delete(game_folder_path + version_filename); }
                    else { File.Move(game_folder_path + version_filename, game_folder_path + version_back_filename); }
                }
                Process.Start(new ProcessStartInfo { FileName = $"steam://rungameid/{899770}", UseShellExecute = true });
            }
            else
            {
                if (!File.Exists(game_folder_path + version_filename))
                {
                    if (File.Exists(game_folder_path + version_back_filename))
                    {
                        File.Move(game_folder_path + version_back_filename, game_folder_path + version_filename);
                    }
                }
                else if (File.Exists(game_folder_path + version_back_filename)) { File.Delete(game_folder_path + version_back_filename); }
                Process.Start(game_folder_path + game_filename, "--offline");
            }
            Application.Exit();
        }

        private void btn_online_Click(object sender, EventArgs e)
        {
            StartGame(true);
        }

        private void btn_offline_Click(object sender, EventArgs e)
        {
            StartGame(false);
        }
    }
}
