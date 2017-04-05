using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;

namespace Alex.WoWRelogger.Controls
{
	public partial class AccountConfigUserControl : System.Windows.Controls.UserControl
	{
		private readonly static DependencyProperty CharacterSettingProperty;

		private ProfileSettings CharacterSetting
		{
			get
			{
				return (ProfileSettings)base.GetValue(AccountConfigUserControl.CharacterSettingProperty);
			}
			set
			{
				base.SetValue(AccountConfigUserControl.CharacterSettingProperty, value);
			}
		}

		public bool IsEditing
		{
			get;
			set;
		}

		static AccountConfigUserControl()
		{
			AccountConfigUserControl.CharacterSettingProperty = DependencyProperty.Register("CharacterSettings", typeof(ProfileSettings), typeof(AccountConfigUserControl));
		}

		public AccountConfigUserControl()
		{
			this.InitializeComponent();
			this.IsEditing = false;
			this.RegionCombo.ItemsSource = Enum.GetValues(typeof(WowSettings.WowRegion));
			this.FactionCombo.ItemsSource = Enum.GetValues(typeof(WowSettings.WowFaction));
		}

		private void AuthenticatorCodeClicked(object sender, RoutedEventArgs e)
		{
			object selectedItem = MainWindow.Instance.AccountGrid.SelectedItem;
		}

		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			CharacterProfile selectedItem = (CharacterProfile)MainWindow.Instance.AccountGrid.SelectedItem;
		}

		public void EditAccount(ProfileSettings settings)
		{
			this.WoWFileInput.FileName = settings.WowSettings.WowPath;
			this.RegionCombo.SelectedItem = settings.WowSettings.Region;
			this.FactionCombo.SelectedItem = settings.WowSettings.Faction;
			this.IsEditing = true;
		}

		private void FactionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MainWindow.Instance.AccountGrid.SelectedItem != null)
			{
				((CharacterProfile)MainWindow.Instance.AccountGrid.SelectedItem).Settings.WowSettings.Faction = (WowSettings.WowFaction)this.FactionCombo.SelectedItem;
			}
		}

		public static T FindParent<T>(DependencyObject from)
		where T : Visual
		{
			T t = default(T);
			DependencyObject parent = VisualTreeHelper.GetParent(from);
			if (parent is T)
			{
				t = (T)(parent as T);
			}
			else if (parent != null)
			{
				t = AccountConfigUserControl.FindParent<T>(parent);
			}
			return t;
		}

		private void PasswordTextPasswordChanged(object sender, RoutedEventArgs e)
		{
		}

		private void ProfileTaskList_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void RegionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MainWindow.Instance.AccountGrid.SelectedItem != null)
			{
				((CharacterProfile)MainWindow.Instance.AccountGrid.SelectedItem).Settings.WowSettings.Region = (WowSettings.WowRegion)this.RegionCombo.SelectedItem;
			}
		}

		private void WoWFileInputFileNameChanged(object sender, RoutedEventArgs e)
		{
			if (MainWindow.Instance.AccountGrid.SelectedItem != null)
			{
				((CharacterProfile)MainWindow.Instance.AccountGrid.SelectedItem).Settings.WowSettings.WowPath = this.WoWFileInput.FileName;
			}
		}

		private void WowWindowGrabTextClick(object sender, RoutedEventArgs e)
		{
			if (MainWindow.Instance.AccountGrid.SelectedItem != null)
			{
				CharacterProfile selectedItem = (CharacterProfile)MainWindow.Instance.AccountGrid.SelectedItem;
				if (selectedItem.WowManager.GameProcess != null)
				{
					Alex.WoWRelogger.Utility.NativeMethods.Rect windowRect = Helper.GetWindowRect(selectedItem.WowManager.GameProcess.MainWindowHandle);
					selectedItem.Settings.WowSettings.WowWindowX = windowRect.Left;
					selectedItem.Settings.WowSettings.WowWindowY = windowRect.Top;
					selectedItem.Settings.WowSettings.WowWindowWidth = windowRect.Right - windowRect.Left;
					selectedItem.Settings.WowSettings.WowWindowHeight = windowRect.Bottom - windowRect.Top;
				}
			}
		}

		private void WowWindowRatioButtonClick(object sender, RoutedEventArgs e)
		{
			if (MainWindow.Instance.AccountGrid.SelectedItem != null)
			{
				CharacterProfile selectedItem = (CharacterProfile)MainWindow.Instance.AccountGrid.SelectedItem;
				if (selectedItem.WowManager.GameProcess != null)
				{
					Screen screen = Screen.FromHandle(selectedItem.WowManager.GameProcess.MainWindowHandle);
					try
					{
						int num = int.Parse(this.WowWindowRatioText.Text);
						WowSettings wowSettings = selectedItem.Settings.WowSettings;
						Rectangle bounds = screen.Bounds;
						wowSettings.WowWindowWidth = bounds.Width / num;
						WowSettings height = selectedItem.Settings.WowSettings;
						bounds = screen.Bounds;
						height.WowWindowHeight = bounds.Height / num;
						Helper.ResizeAndMoveWindow(selectedItem.WowManager.GameProcess.MainWindowHandle, selectedItem.Settings.WowSettings.WowWindowX, selectedItem.Settings.WowSettings.WowWindowY, selectedItem.Settings.WowSettings.WowWindowWidth, selectedItem.Settings.WowSettings.WowWindowHeight);
					}
					catch
					{
					}
				}
			}
		}
	}
}