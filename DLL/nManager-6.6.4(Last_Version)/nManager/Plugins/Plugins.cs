namespace nManager.Plugins
{
    using nManager;
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class Plugins
    {
        public static List<Plugin> ListLoadedPlugins = new List<Plugin>();

        public static void DisposePlugins()
        {
            try
            {
                foreach (Plugin plugin in ListLoadedPlugins)
                {
                    if (plugin != null)
                    {
                        plugin.DisposePlugin();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposePlugins(): " + exception, true);
            }
            ListLoadedPlugins.Clear();
        }

        public static void ReLoadPlugins()
        {
            try
            {
                DisposePlugins();
                foreach (string str in nManagerSetting.CurrentSetting.ActivatedPluginsList)
                {
                    string str2 = Application.StartupPath + @"\Plugins\" + str;
                    Plugin item = new Plugin {
                        PathToPluginFile = str2
                    };
                    item.LoadPlugin(false, false, false);
                    ListLoadedPlugins.Add(item);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadPlugins(): " + exception, true);
            }
        }
    }
}

