using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace Alex.WoWRelogger.Controls
{
	public partial class OptionsUserControl : UserControl
	{
		public OptionsUserControl()
		{
			this.InitializeComponent();
			if (HbRelogManager.Settings.AllowTrials)
			{
				this.AllowTrialsCheckbox.IsChecked = new bool?(true);
			}
			if (HbRelogManager.Settings.AutoCreateAccounts)
			{
				this.CreateAccount.Visibility = System.Windows.Visibility.Hidden;
				this.AutoCreateAccounts.IsChecked = new bool?(true);
			}
		}

		private void AllowTrialsCheckbox_Click(object sender, RoutedEventArgs e)
		{
			if (this.AllowTrialsCheckbox.IsChecked.Value)
			{
				Alex.WoWRelogger.Utility.Log.Write("Trial Account are allowed now.", new object[0]);
				HbRelogManager.Settings.AllowTrials = true;
				return;
			}
			Alex.WoWRelogger.Utility.Log.Write("Trial Accounts are not allowed now !", new object[0]);
			HbRelogManager.Settings.AllowTrials = false;
		}

		private void AutoCreateAccounts_Click(object sender, RoutedEventArgs e)
		{
			if (this.AutoCreateAccounts.IsChecked.Value)
			{
				this.CreateAccount.Visibility = System.Windows.Visibility.Hidden;
				HbRelogManager.Settings.AutoCreateAccounts = true;
				return;
			}
			this.CreateAccount.Visibility = System.Windows.Visibility.Visible;
			HbRelogManager.Settings.AutoCreateAccounts = false;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			WowSettings.PrintParams();
		}

		private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
		{
			AccountCreator.CreateAndPay(null, null, null, null);
		}

		private void DarkStyleCheckCheckedChanged(object sender, RoutedEventArgs e)
		{
			MainWindow.Instance.LoadStyle();
		}

		private void ExportSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog()
			{
				Filter = ".xml|*.xml",
				DefaultExt = ".xml",
				Title = "Export settings to file"
			};
			bool? nullable = saveFileDialog.ShowDialog();
			if ((nullable.GetValueOrDefault() ? nullable.HasValue : false))
			{
				HbRelogManager.Settings.Export(saveFileDialog.FileName).Save();
			}
		}

		private void ImportSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Filter = ".xml|*.xml",
				DefaultExt = ".xml",
				Title = "Browse to and select your exported settings file"
			};
			bool? nullable = openFileDialog.ShowDialog();
			if ((nullable.GetValueOrDefault() ? nullable.HasValue : false))
			{
				HbRelogManager.Settings = GlobalSettings.Import(openFileDialog.FileName);
				MainWindow.Instance.DataContext = HbRelogManager.Settings.CharacterProfiles;
				((OptionsUserControl)MainWindow.Instance.FindName("HbrelogOptions")).DataContext = HbRelogManager.Settings;
				ListBox listBox = (ListBox)((AccountConfigUserControl)MainWindow.Instance.FindName("AccountConfig")).FindName("ProfileTaskList");
				HbRelogManager.Settings.QueueSave();
			}
		}

		private void ReloadAccountsButton_Click(object sender, RoutedEventArgs e)
		{
			AccountManager.Reload();
			Alex.WoWRelogger.Utility.Log.Write("Accounts reloaded!", new object[0]);
		}

		private void ReloadCommandsButton_Click(object sender, object args)
		{
			HbRelogManager.ReloadCommands();
		}

		private void ReloadUIButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (CharacterProfile charactersById in HbRelogManager.CharactersById)
			{
				charactersById.WowManager.NeedReload = true;
			}
		}
	}
}