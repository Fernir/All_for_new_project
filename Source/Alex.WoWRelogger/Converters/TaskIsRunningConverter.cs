using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alex.WoWRelogger.Converters
{
	internal class TaskIsRunningConverter : IValueConverter
	{
		private static FontStyleConverter _styleConverter;

		private static FontWeightConverter _weightConverter;

		static TaskIsRunningConverter()
		{
			TaskIsRunningConverter._styleConverter = new FontStyleConverter();
			TaskIsRunningConverter._weightConverter = new FontWeightConverter();
		}

		public TaskIsRunningConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = (bool)value;
			if (targetType == typeof(TextDecorationCollection))
			{
				if (flag)
				{
					return TextDecorations.Underline;
				}
				return null;
			}
			if (targetType != typeof(FontWeight))
			{
				return null;
			}
			if (!flag)
			{
				return TaskIsRunningConverter._weightConverter.ConvertFrom("Normal");
			}
			return TaskIsRunningConverter._weightConverter.ConvertFrom("Bold");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}