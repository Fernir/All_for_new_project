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
        private Assembly _assembly;
        private IPlugins _instanceFromOtherAssembly;
        private object _obj;
        private string _pathToPluginFile;
        private string _threadName = "";
        private Thread _worker;

        public void DisposePlugin()
        {
            try
            {
                if (this._instanceFromOtherAssembly != null)
                {
                    this._instanceFromOtherAssembly.Dispose();
                }
                if ((this._worker != null) && this._worker.IsAlive)
                {
                    this._worker.Abort();
                }
                Thread.Sleep(0x3e8);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposePlugin(): " + exception, true);
            }
            finally
            {
                this._instanceFromOtherAssembly = null;
                this._assembly = null;
                this._obj = null;
            }
        }

        public void LoadPlugin(bool settingOnly = false, bool resetSettings = false, bool onlyCheckVersion = false)
        {
            try
            {
                this.DisposePlugin();
                this._instanceFromOtherAssembly = null;
                this._assembly = null;
                this._obj = null;
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
                        this._assembly = results.CompiledAssembly;
                        this._obj = this._assembly.CreateInstance("Main", true);
                        if (this._obj is IPlugins)
                        {
                            goto Label_01B3;
                        }
                        Logging.WriteError("The plugin doesn't implement IPlugins or have a different namespace than \"Main\". Path: " + this.PathToPluginFile, true);
                    }
                    return;
                }
                this._assembly = Assembly.LoadFrom(this.PathToPluginFile);
                this._obj = this._assembly.CreateInstance("Main", false);
                if (!(this._obj is IPlugins))
                {
                    Logging.WriteError("The plugin doesn't implement IPlugins or have a different namespace than \"Main\". Path: " + this.PathToPluginFile, true);
                    return;
                }
            Label_01B3:
                if (this._assembly != null)
                {
                    this._instanceFromOtherAssembly = this._obj as IPlugins;
                    foreach (Plugin plugin in nManager.Plugins.Plugins.ListLoadedPlugins)
                    {
                        if (plugin.Name == this._instanceFromOtherAssembly.Name)
                        {
                            Logging.WriteError("A plugin with the same name is already started.", true);
                            return;
                        }
                    }
                    this._threadName = "Plugin " + this._instanceFromOtherAssembly.Name;
                    if (settingOnly && resetSettings)
                    {
                        this._instanceFromOtherAssembly.ResetConfiguration();
                    }
                    else if (settingOnly)
                    {
                        this._instanceFromOtherAssembly.ShowConfiguration();
                    }
                    else
                    {
                        this._worker = onlyCheckVersion ? new Thread(new ThreadStart(this._instanceFromOtherAssembly.CheckFields)) : new Thread(new ThreadStart(this._instanceFromOtherAssembly.Initialize));
                        this._worker.Start();
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
                return (this._instanceFromOtherAssembly != null);
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
                string[] strArray = this._instanceFromOtherAssembly.TargetVersion.Split(new char[] { '.' });
                string[] strArray2 = "5.0.5".Split(new char[] { '.' });
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
                return ((this._instanceFromOtherAssembly != null) && this._instanceFromOtherAssembly.IsStarted);
            }
        }

        public string Name
        {
            get
            {
                if (this._instanceFromOtherAssembly != null)
                {
                    return this._instanceFromOtherAssembly.Name;
                }
                return "";
            }
        }

        public string PathToPluginFile
        {
            get
            {
                return this._pathToPluginFile;
            }
            set
            {
                if (value != null)
                {
                    this._pathToPluginFile = value;
                    this.LoadPlugin(false, false, true);
                }
            }
        }

        public string Version
        {
            get
            {
                if (this._instanceFromOtherAssembly != null)
                {
                    return this._instanceFromOtherAssembly.Version;
                }
                return "";
            }
        }
    }
}

