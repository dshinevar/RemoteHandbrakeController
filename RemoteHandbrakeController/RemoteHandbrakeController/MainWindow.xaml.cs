﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Timers;
using System.IO;
using System.Windows;


namespace RemoteHandbrakeController
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<FileInfo> lstFilesToBeEncoded = new List<FileInfo>();
		XMLConfig xmlConfig;
		Timer timerStatus;

		ConfigurationPage pageConfig;

		/// <summary> Constructor</summary>
		public MainWindow()
		{
			xmlConfig = Globals.LoadConfig(Globals.CONFIG_NAME);
			if (xmlConfig == null)
			{
				xmlConfig = new XMLConfig();
				Globals.SaveConfig(Globals.CONFIG_NAME, xmlConfig);
			}

			timerStatus = new Timer();
			timerStatus.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			timerStatus.Interval = 1500;
			timerStatus.Start();

			MediaSelectionPage mediaSelectionPage = new MediaSelectionPage(xmlConfig);
			InitializeComponent();
			MainFrame.Navigate(mediaSelectionPage);
		}

		#region MENU_CLICKS
		private void mnuConfig_Click(object sender, RoutedEventArgs e)
		{
			pageConfig = new ConfigurationPage(xmlConfig);
			MainFrame.Navigate(pageConfig);
		}

		private void mnuAbout_Click(object sender, RoutedEventArgs e)
		{
			About wndAbout = new About();
			wndAbout.Show();
		}

		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{
			lstFilesToBeEncoded = null;
			Application.Current.Shutdown();
			return;
		}
		#endregion

		/// <summary>
		/// Status Timer Event
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			if (Globals.currentFileBeingEncoded != String.Empty) Dispatcher.Invoke(() => txtCurrentFile.Text = $"CURRENTLY ENCODING { Globals.currentFileBeingEncoded}");
			else Dispatcher.Invoke(() => txtCurrentFile.Text = String.Empty);

			if (xmlConfig.PingTestMode) Dispatcher.Invoke(() => txtTestMode.Text = "TEST MODE");
			else Dispatcher.Invoke(() => txtTestMode.Text = "ENCODE MODE");
		}

		/// <summary> Events when window is closing </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			timerStatus.Stop();
			timerStatus.Close();
		}
					
	}
}
