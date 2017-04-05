namespace nManager.Plugins
{
    using Microsoft.CSharp;
    using nManager.Helpful;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Plugin
    {
        private string _anamehuosKanadalap = "";
        private string _feneoq;
        private IPlugins _ihuohod;
        private Assembly _jataveipujumuUleivo;
        private Thread _moaviPoaweam;
        private object _ximuvGox;

        public void DisposePlugin()
        {
            try
            {
                if (this._ihuohod != null)
                {
                    this._ihuohod.Dispose();
                }
                if ((this._moaviPoaweam != null) && this._moaviPoaweam.IsAlive)
                {
                    this._moaviPoaweam.Abort();
                }
                Thread.Sleep(0x3e8);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposePlugin(): " + exception, true);
            }
            finally
            {
                this._ihuohod = null;
                this._jataveipujumuUleivo = null;
                this._ximuvGox = null;
            }
        }

        public void LoadPlugin(bool settingOnly = false, bool resetSettings = false, bool onlyCheckVersion = false)
        {
            try
            {
                this.DisposePlugin();
                this._ihuohod = null;
                this._jataveipujumuUleivo = null;
                this._ximuvGox = null;
                if (!this.IsDll)
                {
                    CodeDomProvider provider = new CSharpCodeProvider();
                    CompilerParameters options = new CompilerParameters();
                    IEnumerable<string> source = from a in AppDomain.CurrentDomain.GetAssemblies()
                        where !a.IsDynamic && !a.CodeBase.Contains(Process.GetCurrentProcess().ProcessName + ".exe")
                        select a.Location;
                    options.ReferencedAssemblies.AddRange(source.ToArray<string>());
                    string str = File.OpenText(this.PathToPluginFile).ReadToEnd();
                    CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { str });
                    if (results.Errors.HasErrors)
                    {
                        string text = results.Errors.Cast<CompilerError>().Aggregate<CompilerError, string>("Compilator Error :\n", (current, err) => current + err + "\n");
                        Logging.WriteError(text, true);
                        MessageBox.Show(text);
                    }
                    else
                    {
                        this._jataveipujumuUleivo = results.CompiledAssembly;
                        this._ximuvGox = this._jataveipujumuUleivo.CreateInstance("Main", true);
                        if (this._ximuvGox is IPlugins)
                        {
                            goto Label_01B3;
                        }
                        Logging.WriteError("The plugin doesn't implement IPlugins or have a different namespace than \"Main\". Path: " + this.PathToPluginFile, true);
                    }
                    return;
                }
                this._jataveipujumuUleivo = Assembly.LoadFrom(this.PathToPluginFile);
                this._ximuvGox = this._jataveipujumuUleivo.CreateInstance("Main", false);
                if (!(this._ximuvGox is IPlugins))
                {
                    Logging.WriteError("The plugin doesn't implement IPlugins or have a different namespace than \"Main\". Path: " + this.PathToPluginFile, true);
                    return;
                }
            Label_01B3:
                if (this._jataveipujumuUleivo != null)
                {
                    this._ihuohod = this._ximuvGox as IPlugins;
                    foreach (Plugin plugin in nManager.Plugins.Plugins.ListLoadedPlugins)
                    {
                        if (plugin.Name == this._ihuohod.Name)
                        {
                            Logging.WriteError("A plugin with the same name is already started.", true);
                            return;
                        }
                    }
                    this._anamehuosKanadalap = "Plugin " + this._ihuohod.Name;
                    if (settingOnly && resetSettings)
                    {
                        this._ihuohod.ResetConfiguration();
                    }
                    else if (settingOnly)
                    {
                        this._ihuohod.ShowConfiguration();
                    }
                    else
                    {
                        this._moaviPoaweam = onlyCheckVersion ? new Thread(new ThreadStart(this._ihuohod.CheckFields)) : new Thread(new ThreadStart(this._ihuohod.Initialize));
                        this._moaviPoaweam.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadPlugin(bool settingOnly = false, bool resetSettings = false, bool onlyCheckVersion = false): " + exception, true);
            }
        }

        public void ResetConfigurationPlugin()
        {
            try
            {
                nManager.Plugins.Plugins.DisposePlugins();
                this.LoadPlugin(true, true, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationPlugin(): " + exception, true);
            }
        }

        public void ShowConfigurationPlugin()
        {
            try
            {
                nManager.Plugins.Plugins.DisposePlugins();
                this.LoadPlugin(true, false, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationPlugin(): " + exception, true);
            }
        }

        public bool IsAlive
        {
            get
            {
                return (this._ihuohod != null);
            }
        }

        public bool IsDll
        {
            get
            {
                return (this.PathToPluginFile.Substring(this.PathToPluginFile.Length - 3) == "dll");
            }
        }

        public bool IsExpired
        {
            get
            {
                string[] strArray = this._ihuohod.TargetVersion.Split(new char[] { '.' });
                string[] strArray2 = "6.6.4".Split(new char[] { '.' });
                if ((strArray.Length >= 2) && !(strArray[0] != strArray2[0]))
                {
                    return (strArray[1] != strArray2[1]);
                }
                return true;
            }
        }

        public bool IsStarted
        {
            get
            {
                return ((this._ihuohod != null) && this._ihuohod.IsStarted);
            }
        }

        public string Name
        {
            get
            {
                if (this._ihuohod != null)
                {
                    return this._ihuohod.Name;
                }
                return "";
            }
        }

        public string PathToPluginFile
        {
            get
            {
                return this._feneoq;
            }
            set
            {
                if (value != null)
                {
                    this._feneoq = value;
                    this.LoadPlugin(false, false, true);
                }
            }
        }

        public string Version
        {
            get
            {
                if (this._ihuohod != null)
                {
                    return this._ihuohod.Version;
                }
                return "";
            }
        }
    }
}

