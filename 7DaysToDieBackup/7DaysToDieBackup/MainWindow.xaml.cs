using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using _7DaysToDieBackup.DAL;
using Microsoft.WindowsAPICodePack.Dialogs;
using _7DaysToDieBackup.Models;
using System.Timers;
using System.IO.Compression;
using System.Diagnostics;

namespace _7DaysToDieBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer = new Timer();
        public MainWindow()
        {
            InitializeComponent();
            ClearTextBox();
            timer.Interval = 60000;
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Enabled = true;
            timer.Start();
        }
        private void OnTimer(object sender, ElapsedEventArgs args)
        {
            if (CheckTime())
            {
                List<Games> games = GetLocations();
                if (games.Capacity > 0)
                {
                    ZipFiles(games);
                }
            }
        }
        private bool CheckTime()
        {
            if ((DateTime.Now.Minute == 0))
            {
                return true;
            }
            return false;
        }

        private List<Games>GetLocations()
        {
            Database db = new Database();
            List<Games> games = db.GetAllLocations();

            return games;
        }

        private void ZipFiles(List<Games> games)
        {
            foreach (Games game in games)
            {
                ZipGameFolder(game.strGameLocation, game.strBackupLocation);
            }
        }
        private void ZipGameFolder(string strGameLocation, string strBackupLocation)
        {
            ZipFile.CreateFromDirectory(strGameLocation, Path.Combine(strBackupLocation, GenerateZipFileName(strGameLocation)), CompressionLevel.Optimal, true);
        }

        private string GenerateZipFileName(string strGameLocation)
        {
            string filename = new DirectoryInfo(strGameLocation).Name.ToString() + "-" + DateTime.Now.ToString("yy-MM-dd HH-mm") + ".zip";
            return filename;
        }

        private void openFolderButton_Click(object sender, RoutedEventArgs e)
        {
            installTextBox.Text = GetFolderPath();
        }

        private void backupButton_Click(object sender, RoutedEventArgs e)
        {
            backupTextBox.Text = GetFolderPath();
        }

        private string GetFolderPath()
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "7DaysToDie");
                CommonFileDialogResult result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }
            else
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        return dialog.SelectedPath;
                    }
                }
            }

            return "";
        }

        private string GetFilePath()
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "7DaysToDie");
                CommonFileDialogResult result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }
            else
            {
                using (var dialog = new System.Windows.Forms.OpenFileDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        return dialog.FileName;
                    }
                }
            }

            return "";
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.AddLocation(installTextBox.Text, backupTextBox.Text);
            ClearTextBox();
        }

        private void ClearTextBox()
        {
            installTextBox.Text = "";
            backupTextBox.Text = "";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListLocations loc = new ListLocations();

            loc.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            About ab = new About();
            ab.Show();
        }

        private void launchGameButton_Click(object sender, RoutedEventArgs e)
        {
            string strGameExe;
            if (string.IsNullOrEmpty(Properties.Settings.Default.exelocation))
            {
                strGameExe = GetFilePath();
                Properties.Settings.Default.exelocation = strGameExe;
                Properties.Settings.Default.Save();
            }
            else
            {
                strGameExe = Properties.Settings.Default.exelocation;
            }
            if (!string.IsNullOrEmpty(strGameExe))
            {
                LaunchGame(strGameExe);
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            string strGameExe = GetFilePath();
            Properties.Settings.Default.exelocation = strGameExe;
            Properties.Settings.Default.Save();
        }

        private void LaunchGame(string strGame)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = strGame;
            int exitCode;

            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                exitCode = proc.ExitCode;
            }
        }
    }
}
