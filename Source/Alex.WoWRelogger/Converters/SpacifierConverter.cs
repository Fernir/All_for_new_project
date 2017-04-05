using System;
using System.Globalization;
using System.Windows.Data;

namespace Alex.WoWRelogger.Converters
{
	internal class SpacifierConverter : IValueConverter
	{
		public SpacifierConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string) || !(targetType == typeof(string)))
			{
				throw new InvalidOperationException("value or target type is not supported");
			}
			return SpacifierConverter.GetSpaciousString((string)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string) || !(targetType == typeof(string)))
			{
				throw new InvalidOperationException("value or target type is not supported");
			}
			return ((string)value).Replace(" ", "");
		}

		public static string GetSpaciousString(string input)
		{
			char chr;
			string str = "";
			for (int i = 0; i < input.Length; i++)
			{
				if (i <= 0 || !char.IsUpper(input[i]) || i == input.Length - 1)
				{
					chr = input[i];
					str = string.Concat(str, chr.ToString());
				}
				else
				{
					chr = input[i];
					str = string.Concat(str, " ", chr.ToString());
				}
			}
			return str;
		}
	}
}