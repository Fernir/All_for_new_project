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
        private static string _anamehuosKanadalap = "";
        private static string _geipeIn = "";
        private static ICustomProfile _ihuohod;
        private static Assembly _jataveipujumuUleivo;
        private static Thread _moaviPoaweam;
        private static BigInteger _uleugoetoxuo = 0x3b9aca00;
        private static object _ximuvGox;

        public static void DisposeCustomProfile()
        {
            try
            {
                if (_ihuohod != null)
                {
                    _ihuohod.Dispose();
                }
                if ((_moaviPoaweam != null) && _moaviPoaweam.IsAlive)
                {
                    _moaviPoaweam.Abort();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeCustomProfile(): " + exception, true);
            }
            finally
            {
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
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
                _geipeIn = pathToCustomProfileFile;
                if (_ihuohod != null)
                {
                    _ihuohod.Dispose();
                }
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
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
                    _jataveipujumuUleivo = results.CompiledAssembly;
                    _ximuvGox = _jataveipujumuUleivo.CreateInstance("Main", true);
                    _anamehuosKanadalap = "CustomProfile CS";
                }
                else
                {
                    _jataveipujumuUleivo = Assembly.LoadFrom(_geipeIn);
                    _ximuvGox = _jataveipujumuUleivo.CreateInstance("Main", false);
                    _anamehuosKanadalap = "CustomProfile DLL";
                }
                if ((_ximuvGox != null) && (_jataveipujumuUleivo != null))
                {
                    _ihuohod = _ximuvGox as ICustomProfile;
                    if (_ihuohod != null)
                    {
                        if (settingOnly)
                        {
                            if (resetSettings)
                            {
                                _ihuohod.ResetConfiguration();
                            }
                            else
                            {
                                _ihuohod.ShowConfiguration();
                            }
                            _ihuohod.Dispose();
                        }
                        else
                        {
                            Thread thread = new Thread(new ThreadStart(_ihuohod.Initialize)) {
                                IsBackground = true,
                                Name = _anamehuosKanadalap
                            };
                            _moaviPoaweam = thread;
                            _moaviPoaweam.Start();
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
                    if (_geipeIn.Substring(_geipeIn.Length - 3) == "dll")
                    {
                        LoadCustomProfile(_geipeIn, false, false, false);
                    }
                    else
                    {
                        LoadCustomProfile(_geipeIn, false, false, true);
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
                return ((_ihuohod != null) && _ihuohod.DontStartFights);
            }
            set
            {
                if (_ihuohod != null)
                {
                    _ihuohod.DontStartFights = value;
                }
            }
        }

        public static bool GetSetIgnoreFight
        {
            get
            {
                return ((_ihuohod != null) && _ihuohod.IgnoreFight);
            }
            set
            {
                if (_ihuohod != null)
                {
                    _ihuohod.IgnoreFight = value;
                }
            }
        }

        public static bool IsAliveCustomProfile
        {
            get
            {
                try
                {
                    return (_ximuvGox != null);
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

