using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Alex.WoWRelogger.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Alex.WoWRelogger.Properties.Settings defaultInstance;

		public static Alex.WoWRelogger.Properties.Settings Default
		{
			get
			{
				return Alex.WoWRelogger.Properties.Settings.defaultInstance;
			}
		}

		static Settings()
		{
			Alex.WoWRelogger.Properties.Settings.defaultInstance = (Alex.WoWRelogger.Properties.Settings)SettingsBase.Synchronized(new Alex.WoWRelogger.Properties.Settings());
		}

		public Settings()
		{
		}
	}
}