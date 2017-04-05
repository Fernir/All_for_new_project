using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Alex.WoWRelogger.Controls
{
	[ContentProperty("FileName")]
	public partial class FileInputBox : UserControl
	{
		public readonly static DependencyProperty FileNameProperty;

		public readonly static DependencyProperty TitleProperty;

		public readonly static DependencyProperty FilterProperty;

		public readonly static DependencyProperty DefaultExtProperty;

		public readonly static RoutedEvent FileNameChangedEvent;

		public string DefaultExt
		{
			get
			{
				return (string)base.GetValue(FileInputBox.DefaultExtProperty);
			}
			set
			{
				base.SetValue(FileInputBox.DefaultExtProperty, value);
			}
		}

		public string FileName
		{
			get
			{
				return (string)base.GetValue(FileInputBox.FileNameProperty);
			}
			set
			{
				base.SetValue(FileInputBox.FileNameProperty, value);
			}
		}

		public string Filter
		{
			get
			{
				return (string)base.GetValue(FileInputBox.FilterProperty);
			}
			set
			{
				base.SetValue(FileInputBox.FilterProperty, value);
			}
		}

		public string Title
		{
			get
			{
				return (string)base.GetValue(FileInputBox.TitleProperty);
			}
			set
			{
				base.SetValue(FileInputBox.TitleProperty, value);
			}
		}

		static FileInputBox()
		{
			FileInputBox.FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FileInputBox), new PropertyMetadata(new PropertyChangedCallback(FileInputBox.FileNameChangedCallback)));
			FileInputBox.TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FileInputBox));
			FileInputBox.FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(FileInputBox));
			FileInputBox.DefaultExtProperty = DependencyProperty.Register("DefaultExt", typeof(string), typeof(FileInputBox));
			FileInputBox.FileNameChangedEvent = EventManager.RegisterRoutedEvent("FileNameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileInputBox));
		}

		public FileInputBox()
		{
			this.InitializeComponent();
			this.theTextBox.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
		}

		private static void FileNameChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FileInputBox)d).theTextBox.Text = (string)e.NewValue;
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			if (oldContent != null)
			{
				throw new InvalidOperationException("You can't change the Content!");
			}
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			this.FileName = this.theTextBox.Text;
			base.RaiseEvent(new RoutedEventArgs(FileInputBox.FileNameChangedEvent));
		}

		private void TheButtonClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Filter = this.Filter,
				DefaultExt = this.DefaultExt,
				Title = this.Title
			};
			bool? nullable = openFileDialog.ShowDialog();
			if ((nullable.GetValueOrDefault() ? nullable.HasValue : false))
			{
				this.FileName = openFileDialog.FileName;
			}
		}

		public event RoutedEventHandler FileNameChanged
		{
			add
			{
				base.AddHandler(FileInputBox.FileNameChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(FileInputBox.FileNameChangedEvent, value);
			}
		}
	}
}