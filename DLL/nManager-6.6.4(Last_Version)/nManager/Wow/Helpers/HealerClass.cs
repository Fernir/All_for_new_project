namespace nManager.Wow.Helpers
{
    using Microsoft.CSharp;
    using nManager;
    using nManager.Annotations;
    using nManager.Helpful;
    using nManager.Wow.ObjectManager;
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

    public class HealerClass
    {
        private static string _anamehuosKanadalap = "";
        private static IHealerClass _ihuohod;
        private static Assembly _jataveipujumuUleivo;
        private static Thread _moaviPoaweam;
        private static Thread _oxajosopeawosuOko;
        [UsedImplicitly]
        private static BigInteger _uleugoetoxuo = 0x3b9aca00;
        private static string _weaboi = "";
        private static object _ximuvGox;
        private static readonly object HealerClassLocker = new object();

        public static void DisposeHealerClass()
        {
            try
            {
                lock (HealerClassLocker)
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
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeHealerClass(): " + exception, true);
            }
            finally
            {
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
            }
        }

        public static bool InCustomRange(WoWUnit unit, float minRange, float maxRange)
        {
            try
            {
                if (!IsAliveHealerClass)
                {
                    return CombatClass.InSpellRange(unit, minRange, maxRange);
                }
                float getDistance = unit.GetDistance;
                float getCombatReach = unit.GetCombatReach;
                return ((getDistance - getCombatReach) <= (maxRange - 0.5));
            }
            catch (Exception exception)
            {
                Logging.WriteError("HealerClass > InCustomRange: " + exception, true);
            }
            return false;
        }

        public static bool InMinRange(WoWUnit unit)
        {
            try
            {
                if (!IsAliveHealerClass)
                {
                    return CombatClass.AboveMinRange(unit);
                }
                float getDistance = unit.GetDistance;
                float getCombatReach = unit.GetCombatReach;
                return (((getDistance - getCombatReach) <= (GetRange - 0.5)) && ((getDistance - getCombatReach) >= -1.5));
            }
            catch (Exception exception)
            {
                Logging.WriteError("HealerClass > InMinRange: " + exception, true);
            }
            return false;
        }

        public static bool InRange(WoWUnit unit)
        {
            try
            {
                if (!IsAliveHealerClass)
                {
                    return CombatClass.InRange(unit);
                }
                float getDistance = unit.GetDistance;
                float getCombatReach = unit.GetCombatReach;
                return ((getDistance - getCombatReach) <= (GetRange - 0.5));
            }
            catch (Exception exception)
            {
                Logging.WriteError("HealerClass > InRange: " + exception, true);
            }
            return false;
        }

        public static void LoadHealerClass()
        {
            lock (HealerClassLocker)
            {
                if (((_moaviPoaweam == null) || !_moaviPoaweam.IsAlive) && ((_oxajosopeawosuOko == null) || !_oxajosopeawosuOko.IsAlive))
                {
                    Thread thread = new Thread(new ThreadStart(HealerClass.LoadHealerClassThread)) {
                        Name = "Load Healer Class"
                    };
                    _oxajosopeawosuOko = thread;
                    _oxajosopeawosuOko.Start();
                }
            }
        }

        public static void LoadHealerClass(string pathToHealerClassFile, bool settingOnly = false, bool resetSettings = false, bool cSharpFile = true)
        {
            try
            {
                _weaboi = pathToHealerClassFile;
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
                    string str = File.OpenText(pathToHealerClassFile).ReadToEnd();
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
                    _anamehuosKanadalap = "HealerClass CS";
                }
                else
                {
                    _jataveipujumuUleivo = Assembly.LoadFrom(_weaboi);
                    _ximuvGox = _jataveipujumuUleivo.CreateInstance("Main", false);
                    _anamehuosKanadalap = "HealerClass DLL";
                }
                if ((_ximuvGox != null) && (_jataveipujumuUleivo != null))
                {
                    _ihuohod = _ximuvGox as IHealerClass;
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
                        Logging.WriteError("Custom Class Loading error.", true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadHealerClass(string _pathToHealerClassFile): " + exception, true);
            }
        }

        public static void LoadHealerClassThread()
        {
            try
            {
                if (nManagerSetting.CurrentSetting.HealerClass != "")
                {
                    string pathToHealerClassFile = Application.StartupPath + @"\HealerClasses\" + nManagerSetting.CurrentSetting.HealerClass;
                    if (pathToHealerClassFile.Substring(pathToHealerClassFile.Length - 3) == "dll")
                    {
                        LoadHealerClass(pathToHealerClassFile, false, false, false);
                    }
                    else
                    {
                        LoadHealerClass(pathToHealerClassFile, false, false, true);
                    }
                }
                else
                {
                    Logging.Write("No custom class selected");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadHealerClass(): " + exception, true);
            }
        }

        public static void ResetConfigurationHealerClass(string filePath)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 3) == "dll")
                {
                    LoadHealerClass(filePath, true, true, false);
                }
                else
                {
                    LoadHealerClass(filePath, true, true, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationHealerClass(): " + exception, true);
            }
        }

        public static void ResetHealerClass()
        {
            try
            {
                if (IsAliveHealerClass)
                {
                    DisposeHealerClass();
                    Thread.Sleep(0x3e8);
                    if (_weaboi.Substring(_weaboi.Length - 3) == "dll")
                    {
                        LoadHealerClass(_weaboi, false, false, false);
                    }
                    else
                    {
                        LoadHealerClass(_weaboi, false, false, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetHealerClass(): " + exception, true);
            }
        }

        public static void ShowConfigurationHealerClass(string filePath)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 3) == "dll")
                {
                    LoadHealerClass(filePath, true, false, false);
                }
                else
                {
                    LoadHealerClass(filePath, true, false, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationHealerClass(): " + exception, true);
            }
        }

        public static float GetRange
        {
            get
            {
                try
                {
                    if (_ihuohod != null)
                    {
                        return ((_ihuohod.Range < 5f) ? 5f : _ihuohod.Range);
                    }
                    return 5f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("HealerClass > GetRange: " + exception, true);
                    return 5f;
                }
            }
        }

        public static bool IsAliveHealerClass
        {
            get
            {
                try
                {
                    return ((_moaviPoaweam != null) && _moaviPoaweam.IsAlive);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveHealerClass: " + exception, true);
                    return false;
                }
            }
        }
    }
}

