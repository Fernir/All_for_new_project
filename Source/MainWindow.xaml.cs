using Alex.WoWRelogger.Controls;
using Alex.WoWRelogger.FiniteStateMachine.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shell;
using System.Windows.Threading;

namespace Alex.WoWRelogger
{
	public partial class MainWindow : Window
	{
		private Timer _autoCloseTimer;

		public static MainWindow Instance
		{
			get;
			private set;
		}

		public MainWindow()
		{
			MainWindow.Instance = this;
			this.LoadStyle();
			Application.LoadComponent(this, new Uri("mainwindow.xaml", UriKind.Relative));
		}

		private void AccountGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.AccountGrid.SelectedItem != null)
			{
				this.EditAccount(((CharacterProfile)this.AccountGrid.SelectedItem).Settings);
			}
		}

		private void AccountGridSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.AccountConfig != null && this.AccountGrid != null && this.AccountConfig.IsEditing && this.AccountGrid.SelectedItem != null)
			{
				this.EditAccount(((CharacterProfile)this.AccountGrid.SelectedItem).Settings);
			}
		}

		private void AcountConfigCloseButtonClick(object sender, RoutedEventArgs e)
		{
			DoubleAnimation doubleAnimation = new DoubleAnimation(0, new Duration(TimeSpan.Parse("0:0:0.4")))
			{
				AccelerationRatio = 0.7
			};
			this.AccountConfigGrid.BeginAnimation(FrameworkElement.WidthProperty, doubleAnimation);
			this.AccountConfig.IsEditing = false;
			HbRelogManager.Settings.Save();
		}

		private void AddAccountButton_Click(object sender, RoutedEventArgs e)
		{
			CharacterProfile characterProfile = new CharacterProfile();
			if (this.AccountGrid.SelectedItem != null)
			{
				characterProfile.Settings = ((CharacterProfile)this.AccountGrid.SelectedItem).Settings.ShadowCopy();
			}
			Thread thread = new Thread(() => HbRelogManager.DoWork(characterProfile))
			{
				IsBackground = true
			};
			thread.Start();
			HbRelogManager.WorkerThreads.Add(characterProfile.Id, thread);
			if (characterProfile.Settings != null)
			{
				HbRelogManager.Settings.CharacterProfiles.Add(characterProfile);
				this.AccountGrid.SelectedItem = characterProfile;
				this.EditAccount(characterProfile.Settings);
			}
		}

		private void BringHbToForegroundMenuItem_Click(object sender, RoutedEventArgs e)
		{
		}

		private void DataGridRowContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)sender;
			WowManager wowManager = ((CharacterProfile)dataGridRow.Item).WowManager;
			MenuItem item = (MenuItem)dataGridRow.ContextMenu.Items[2];
			MenuItem menuItem = (MenuItem)dataGridRow.ContextMenu.Items[3];
		}

		private void DeleteAccountButtonClick(object sender, RoutedEventArgs e)
		{
			if (this.AccountGrid.SelectedIndex >= 0 && MessageBox.Show("Are you sure you want to delete selected profile/s", "Account deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				for (int i = this.AccountGrid.SelectedItems.Count - 1; i >= 0; i--)
				{
					CharacterProfile item = (CharacterProfile)this.AccountGrid.SelectedItems[i];
					HbRelogManager.Settings.CharacterProfiles.Remove(item);
					HbRelogManager.WorkerThreads[item.Id].Abort();
					HbRelogManager.WorkerThreads.Remove(item.Id);
				}
				HbRelogManager.Settings.Save();
			}
		}

		private void EditAccount(ProfileSettings charSettings)
		{
			if (charSettings != null)
			{
				DoubleAnimation doubleAnimation = new DoubleAnimation(this.AccountConfigGridColumn.ActualWidth, new Duration(TimeSpan.Parse("0:0:0.4")))
				{
					DecelerationRatio = 0.7
				};
				this.AccountConfigGrid.BeginAnimation(FrameworkElement.WidthProperty, doubleAnimation);
				this.AccountConfig.EditAccount(charSettings);
			}
		}

		private void EditAccountButtonClick(object sender, RoutedEventArgs e)
		{
			if (this.AccountGrid.SelectedItem != null)
			{
				this.EditAccount(((CharacterProfile)this.AccountGrid.SelectedItem).Settings);
			}
		}

		private void KillHbMenuItem_Click(object sender, RoutedEventArgs e)
		{
		}

		public void LoadStyle()
		{
			Uri uri = new Uri("/styles/ExpressionDark.xaml", UriKind.Relative);
			ResourceDictionary resourceDictionaries = new ResourceDictionary()
			{
				Source = uri
			};
			base.Resources.MergedDictionaries.Clear();
			base.Resources.MergedDictionaries.Add(resourceDictionaries);
		}

		private void MaximizeWowMenuItem_Click(object sender, RoutedEventArgs e)
		{
			WowManager wowManager = ((CharacterProfile)((MenuItem)sender).Tag).WowManager;
			if (wowManager.IsRunning && !wowManager.GameProcess.HasExitedSafe())
			{
				NativeMethods.ShowWindow(wowManager.GameProcess.MainWindowHandle, NativeMethods.ShowWindowCommands.Maximize);
				NativeMethods.SetForegroundWindow(wowManager.GameProcess.MainWindowHandle);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			TimeSpan saveCompleteTimeSpan = HbRelogManager.Settings.SaveCompleteTimeSpan;
			if (HbRelogManager.Settings == null || !(saveCompleteTimeSpan != TimeSpan.FromSeconds(0)))
			{
				base.OnClosing(e);
			}
			else
			{
				e.Cancel = true;
				base.Title = "WoWRelogger : Waiting for settings to save before exiting";
				this._autoCloseTimer = new Timer((object state) => {
					if (this._autoCloseTimer != null)
					{
						this._autoCloseTimer.Dispose();
					}
					this._autoCloseTimer = null;
					base.Dispatcher.Invoke(new Action(this.Close));
				}, null, saveCompleteTimeSpan, TimeSpan.FromMilliseconds(-1));
			}
			HbRelogManager.Shutdown();
		}

		private void PauseButtonClick(object sender, RoutedEventArgs e)
		{
			foreach (CharacterProfile selectedItem in this.AccountGrid.SelectedItems)
			{
				if (!selectedItem.IsRunning)
				{
					continue;
				}
				selectedItem.Pause();
			}
		}

		private void PauseThumbButtonClick(object sender, EventArgs e)
		{
			foreach (CharacterProfile characterProfile in HbRelogManager.Settings.CharacterProfiles)
			{
				if (!characterProfile.Settings.IsEnabled)
				{
					continue;
				}
				characterProfile.Pause();
			}
		}

		private void ProfileEnabledCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			((ProfileSettings)((CheckBox)sender).Tag).IsEnabled = true;
		}

		private void ProfileEnabledCheckBoxUnchecked(object sender, RoutedEventArgs e)
		{
			((ProfileSettings)((CheckBox)sender).Tag).IsEnabled = false;
		}

		private void SelectAllButtonClick(object sender, RoutedEventArgs e)
		{
			this.AccountGrid.SelectAll();
		}

		private void SkipTaskMenuItem_Click(object sender, RoutedEventArgs e)
		{
		}

		private void StartSelButtonClick(object sender, RoutedEventArgs e)
		{
			foreach (object selectedItem in this.AccountGrid.SelectedItems)
			{
				HbRelogManager.StartProfile((CharacterProfile)selectedItem);
			}
		}

		private void StartThumbButtonClick(object sender, EventArgs e)
		{
			foreach (CharacterProfile characterProfile in HbRelogManager.Settings.CharacterProfiles)
			{
				if (characterProfile.IsRunning && !characterProfile.IsPaused || !characterProfile.Settings.IsEnabled)
				{
					continue;
				}
				characterProfile.Start();
			}
		}

		private void StopSelButtonClick(object sender, RoutedEventArgs e)
		{
			foreach (object selectedItem in this.AccountGrid.SelectedItems)
			{
				HbRelogManager.StopProfile((CharacterProfile)selectedItem);
			}
		}

		private void StopThumbButtonClick(object sender, EventArgs e)
		{
			foreach (CharacterProfile characterProfile in HbRelogManager.Settings.CharacterProfiles)
			{
				if (!characterProfile.IsRunning || !characterProfile.Settings.IsEnabled)
				{
					continue;
				}
				characterProfile.Stop();
			}
		}

		private void TabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			TabItem tabItem = (TabItem)sender;
			if (e.OriginalSource != tabItem.Header)
			{
				return;
			}
			double num = 250;
			if (!tabItem.IsSelected)
			{
				this.OptionsAndLogTabCtrl.SelectedItem = tabItem;
			}
			else
			{
				num = 20;
				this.OptionsAndLogTabCtrl.SelectedIndex = -1;
			}
			e.Handled = true;
			DoubleAnimation doubleAnimation = new DoubleAnimation(num, new Duration(TimeSpan.Parse("0:0:0.300")))
			{
				DecelerationRatio = 0.7
			};
			this.OptionsAndLogGrid.BeginAnimation(FrameworkElement.HeightProperty, doubleAnimation);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.OptionsTab.IsSelected = false;
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			version = new Version(version.Major, version.Minor, version.Build);
			Alex.WoWRelogger.Utility.Log.Write("WoWRelogger Version {0}", new object[] { version });
			Alex.WoWRelogger.Utility.Log.Write("******* Settings ******** ", new object[0]);
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Auto AcceptTosEula:", HbRelogManager.Settings.AutoAcceptTosEula });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Allow Trials:", HbRelogManager.Settings.AllowTrials });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Auto Update HB:", HbRelogManager.Settings.AutoUpdateHB });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Check Hb's Responsiveness:", HbRelogManager.Settings.CheckHbResponsiveness });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Check Realm Status:", HbRelogManager.Settings.CheckRealmStatus });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "HB Delay:", HbRelogManager.Settings.HBDelay });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Login Delay:", HbRelogManager.Settings.LoginDelay });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Minimize Hb On Startup:", HbRelogManager.Settings.MinimizeHbOnStart });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Set GameWindow Title:", HbRelogManager.Settings.SetGameWindowTitle });
			Alex.WoWRelogger.Utility.Log.Write("\t{0,-30} {1}", new object[] { "Wow Start Delay:", HbRelogManager.Settings.WowDelay });
			HbRelogManager.UpdateWebmoneyBalance();
		}
	}
}