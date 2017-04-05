namespace nManager.Wow.Helpers
{
    using Microsoft.CSharp;
    using nManager.Helpful;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class CustomProfile
    {
        private static Assembly _assembly;
        private static BigInteger _forceBigInteger = 0x3b9aca00;
        private static ICustomProfile _instanceFromOtherAssembly;
        private static object _obj;
        private static string _pathToCustomProfileFile = "";
        private static string _threadName = "";
        private static Thread _worker;

        public static void DisposeCustomProfile()
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }
                if ((_worker != null) && _worker.IsAlive)
                {
                    _worker.Abort();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeCustomProfile(): " + exception, true);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static void LoadCustomProfile(bool init, string profileTypeScriptName)
        {
            try
            {
                if (profileTypeScriptName != "")
                {
                    string pathToCustomProfileFile = Application.StartupPath + @"\Profiles\Battlegrounder\ProfileType\CSharpProfile\" + profileTypeScriptName;
                    if (pathToCustomProfileFile.Substring(pathToCustomProfileFile.Length - 3) == "dll")
                    {
                        LoadCustomProfile(pathToCustomProfileFile, false, false, false);
                    }
                    else
                    {
                        LoadCustomProfile(pathToCustomProfileFile, false, false, true);
                    }
                }
                else
                {
                    Logging.Write("ProfileType: ProfileTypeScriptName is empty.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomProfile(): " + exception, true);
            }
        }

        public static void LoadCustomProfile(string pathToCustomProfileFile, bool settingOnly = false, bool resetSettings = false, bool cSharpFile = true)
        {
            try
            {
                _pathToCustomProfileFile = pathToCustomProfileFile;
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
                if (cSharpFile)
                {
                    CodeDomProvider provider = new CSharpCodeProvider();
                    CompilerParameters options = new CompilerParameters();
                    IEnumerable<string> source = from a in AppDomain.CurrentDomain.GetAssemblies()
                        where !a.IsDynamic && !a.CodeBase.Contains(Process.GetCurrentProcess().ProcessName + ".exe")
                        select a.Location;
                    options.ReferencedAssemblies.AddRange(source.ToArray<string>());
                    string str = File.OpenText(pathToCustomProfileFile).ReadToEnd();
                    CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { str });
                    if (results.Errors.HasErrors)
                    {
                        string text = results.Errors.Cast<CompilerError>().Aggregate<CompilerError, string>("Compilator Error :\n", (current, err) => current + err + "\n");
                        Logging.WriteError(text, true);
                        MessageBox.Show(text);
                        return;
                    }
                    _assembly = results.CompiledAssembly;
                    _obj = _assembly.CreateInstance("Main", true);
                    _threadName = "CustomProfile CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToCustomProfileFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "CustomProfile DLL";
                }
                if ((_obj != null) && (_assembly != null))
                {
                    _instanceFromOtherAssembly = _obj as ICustomProfile;
                    if (_instanceFromOtherAssembly != null)
                    {
                        if (settingOnly)
                        {
                            if (resetSettings)
                            {
                                _instanceFromOtherAssembly.ResetConfiguration();
                            }
                            else
                            {
                                _instanceFromOtherAssembly.ShowConfiguration();
                            }
                            _instanceFromOtherAssembly.Dispose();
                        }
                        else
                        {
                            Thread thread = new Thread(new ThreadStart(_instanceFromOtherAssembly.Initialize)) {
                                IsBackground = true,
                                Name = _threadName
                            };
                            _worker = thread;
                            _worker.Start();
                        }
                    }
                    else
                    {
                        Logging.WriteError("Custom Profile Loading error.", true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomProfile(string _pathToCustomProfileFile): " + exception, true);
            }
        }

        public static void ResetConfigurationCustomProfile(string filePath)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 3) == "dll")
                {
                    LoadCustomProfile(filePath, true, true, false);
                }
                else
                {
                    LoadCustomProfile(filePath, true, true, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCustomProfile(): " + exception, true);
            }
        }

        public static void ResetCustomProfile()
        {
            try
            {
                if (IsAliveCustomProfile)
                {
                    DisposeCustomProfile();
                    Thread.Sleep(0x3e8);
                    if (_pathToCustomProfileFile.Substring(_pathToCustomProfileFile.Length - 3) == "dll")
                    {
                        LoadCustomProfile(_pathToCustomProfileFile, false, false, false);
                    }
                    else
                    {
                        LoadCustomProfile(_pathToCustomProfileFile, false, false, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetCustomProfile(): " + exception, true);
            }
        }

        public static void ShowConfigurationCustomProfile(string filePath)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 3) == "dll")
                {
                    LoadCustomProfile(filePath, true, false, false);
                }
                else
                {
                    LoadCustomProfile(filePath, true, false, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCustomProfile(): " + exception, true);
            }
        }

        public static bool GetSetDontStartFights
        {
            get
            {
                return ((_instanceFromOtherAssembly != null) && _instanceFromOtherAssembly.DontStartFights);
            }
            set
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.DontStartFights = value;
                }
            }
        }

        public static bool GetSetIgnoreFight
        {
            get
            {
                return ((_instanceFromOtherAssembly != null) && _instanceFromOtherAssembly.IgnoreFight);
            }
            set
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.IgnoreFight = value;
                }
            }
        }

        public static bool IsAliveCustomProfile
        {
            get
            {
                try
                {
                    return (_obj != null);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCustomProfile: " + exception, true);
                    return false;
                }
            }
        }
    }
}

