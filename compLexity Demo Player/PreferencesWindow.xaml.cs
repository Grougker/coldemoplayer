﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics; // ProcessPriorityClass

namespace compLexity_Demo_Player
{
    /// <summary>
    /// Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public PreferencesWindow()
        {
            InitializeComponent();
        }

        private void UpdateSteamAccountNames()
        {
            uiSteamAccountNameComboBox.Items.Clear();
            uiSteamAccountNameComboBox.Text = "";

            // make sure Steam.exe is set
            if (!File.Exists(uiSteamExeTextBox.Text))
            {
                return;
            }

            // make sure SteamApps folder exists
            String steamAppsPath = System.IO.Path.GetDirectoryName(uiSteamExeTextBox.Text) + "\\SteamApps\\";

            if (!Directory.Exists(steamAppsPath))
            {
                return;
            }

            DirectoryInfo steamAppsDirInfo = new DirectoryInfo(steamAppsPath);

            foreach (DirectoryInfo dirInfo in steamAppsDirInfo.GetDirectories())
            {
                uiSteamAccountNameComboBox.Items.Add(dirInfo.Name);
            }

            // select the steam account in the config, or the first item
            if (uiSteamAccountNameComboBox.Items.Count > 0)
            {
                Int32 index = uiSteamAccountNameComboBox.Items.IndexOf(Config.Settings.SteamAccountFolder);

                if (index == -1)
                {
                    index = 0;
                }

                uiSteamAccountNameComboBox.SelectedIndex = index;
            }
        }

        private String BrowseForFile(String fileName, String initialFolder)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Title = "Browse for \"" + fileName + "\"...";
            dialog.InitialDirectory = initialFolder;
            dialog.Filter = fileName + "|" + fileName;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.FileName;
            }

            return null;
        }

        #region Event Handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            uiSteamExeTextBox.Text = Config.Settings.SteamExeFullPath;
            uiSteamHlExeTextBox.Text = Config.Settings.SteamHlExeFullPath;
            uiSteamAdditionalLaunchParametersTextBox.Text = Config.Settings.SteamAdditionalLaunchParameters;
            uiHalfLifeExeTextBox.Text = Config.Settings.HlExeFullPath;
            uiHalfLifeAdditionalLaunchParametersTextBox.Text = Config.Settings.HlAdditionalLaunchParameters;
            uiCounterStrikeExeTextBox.Text = Config.Settings.CstrikeExeFullPath;
            uiCounterStrikeAdditionalLaunchParametersTextBox.Text = Config.Settings.CstrikeAdditionalLaunchParameters;
            uiHlaeExeTextBox.Text = Config.Settings.HlaeExeFullPath;
            uiDemAssociateCheckBox.IsChecked = Config.Settings.AssociateWithDemFiles;
            uiAutoUpdateCheckBox.IsChecked = Config.Settings.AutoUpdate;
            uiMinimizeToTray.IsChecked = Config.Settings.MinimizeToTray;

            foreach (ProcessPriority pp in uiGameProcessPriorityComboBox.Items)
            {
                if (pp.Value == Config.Settings.GameProcessPriority)
                {
                    uiGameProcessPriorityComboBox.SelectedItem = pp;
                }
            }

            UpdateSteamAccountNames();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Config.Settings.SteamExeFullPath = uiSteamExeTextBox.Text;
            Config.Settings.SteamHlExeFullPath = uiSteamHlExeTextBox.Text;
            Config.Settings.SteamAdditionalLaunchParameters = uiSteamAdditionalLaunchParametersTextBox.Text;
            Config.Settings.HlExeFullPath = uiHalfLifeExeTextBox.Text;
            Config.Settings.HlAdditionalLaunchParameters = uiHalfLifeAdditionalLaunchParametersTextBox.Text;
            Config.Settings.CstrikeExeFullPath = uiCounterStrikeExeTextBox.Text;
            Config.Settings.CstrikeAdditionalLaunchParameters = uiCounterStrikeAdditionalLaunchParametersTextBox.Text;

            String selectedSteamAccountName = (String)uiSteamAccountNameComboBox.SelectedItem;

            if (selectedSteamAccountName == null)
            {
                Config.Settings.SteamAccountFolder = "";
            }
            else
            {
                Config.Settings.SteamAccountFolder = selectedSteamAccountName;
            }

            Config.Settings.HlaeExeFullPath = uiHlaeExeTextBox.Text;

            // misc.
            Config.Settings.AutoUpdate = (Boolean)uiAutoUpdateCheckBox.IsChecked;
            Config.Settings.MinimizeToTray = (Boolean)uiMinimizeToTray.IsChecked;
            Config.Settings.GameProcessPriority = ((ProcessPriority)(uiGameProcessPriorityComboBox.SelectedItem)).Value;
        }

        private void uiCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void uiSteamExeBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            String initialFolder = "";

            if (File.Exists(uiSteamExeTextBox.Text))
            {
                initialFolder = System.IO.Path.GetDirectoryName(uiSteamExeTextBox.Text);
            }

            String s = BrowseForFile("Steam.exe", initialFolder);

            if (s != null)
            {
                uiSteamExeTextBox.Text = s;
                UpdateSteamAccountNames();
            }
        }

        private void uiSteamHlExeBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            String initialFolder = "";

            if (File.Exists(uiSteamHlExeTextBox.Text))
            {
                initialFolder = System.IO.Path.GetDirectoryName(uiSteamHlExeTextBox.Text);
            }

            String s = BrowseForFile("hl.exe", initialFolder);

            if (s != null)
            {
                uiSteamHlExeTextBox.Text = s;
            }
        }

        private void uiHalfLifeExeBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            String initialFolder = "";

            if (File.Exists(uiHalfLifeExeTextBox.Text))
            {
                initialFolder = System.IO.Path.GetDirectoryName(uiHalfLifeExeTextBox.Text);
            }

            String s = BrowseForFile("hl.exe", initialFolder);

            if (s != null)
            {
                uiHalfLifeExeTextBox.Text = s;
            }
        }

        private void uiCounterStrikeExeBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            String initialFolder = "";

            if (File.Exists(uiCounterStrikeExeTextBox.Text))
            {
                initialFolder = System.IO.Path.GetDirectoryName(uiCounterStrikeExeTextBox.Text);
            }

            String s = BrowseForFile("cstrike.exe", initialFolder);

            if (s != null)
            {
                uiCounterStrikeExeTextBox.Text = s;
            }
        }

        private void uiHlaeExeBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            String initialFolder = "";

            if (File.Exists(uiHlaeExeTextBox.Text))
            {
                initialFolder = System.IO.Path.GetDirectoryName(uiHlaeExeTextBox.Text);
            }

            String s = BrowseForFile("hlae.exe", initialFolder);

            if (s != null)
            {
                uiHlaeExeTextBox.Text = s;
            }
        }

        private void uiDemAssociateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // handle *.dem file association
            try
            {
                Config.AssociateWithDemFiles();
            }
            catch (UnauthorizedAccessException)
            {
                Common.Message(this, "Can't associate with *.dem files, your Windows user account is not authorized to write registry keys. Try running this program as an administrator.");
                uiDemAssociateCheckBox.IsChecked = false;
            }

            Config.Settings.AssociateWithDemFiles = (Boolean)uiDemAssociateCheckBox.IsChecked;
        }

        private void uiDemAssociateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.Settings.AssociateWithDemFiles = false;
        }
        #endregion
    }

    public class ProcessPriority
    {
        public String Name { get; set; }
        public ProcessPriorityClass Value { get; set; }
    }
}
