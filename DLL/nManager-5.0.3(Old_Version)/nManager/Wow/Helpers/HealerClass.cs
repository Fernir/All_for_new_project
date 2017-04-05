namespace nManager.Wow.Helpers
{
    using Microsoft.CSharp;
    using nManager;
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
        private static Assembly _assembly;
        private static BigInteger _forceBigInteger = 0x3b9aca00;
        private static IHealerClass _instanceFromOtherAssembly;
        private static object _obj;
        private static string _pathToHealerClassFile = "";
        private static string _threadName = "";
        private static Thread _worker;

        public static void DisposeHealerClass()
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
                Logging.WriteError("DisposeHealerClass(): " + exception, true);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
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
            Thread thread2 = new Thread(new ThreadStart(HealerClass.LoadHealerClassThread)) {
                Name = "Load Healer Class"
            };
            thread2.Start();
        }

        public static void LoadHealerClass(string pathToHealerClassFile, bool settingOnly = false, bool resetSettings = false, bool CSharpFile = true)
        {
            try
            {
                _pathToHealerClassFile = pathToHealerClassFile;
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
                if (CSharpFile)
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
                    _assembly = results.CompiledAssembly;
                    _obj = _assembly.CreateInstance("Main", true);
                    _threadName = "HealerClass CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToHealerClassFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "HealerClass DLL";
                }
                if ((_obj != null) && (_assembly != null))
                {
                    _instanceFromOtherAssembly = _obj as IHealerClass;
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
                    if (_pathToHealerClassFile.Substring(_pathToHealerClassFile.Length - 3) == "dll")
                    {
                        LoadHealerClass(_pathToHealerClassFile, false, false, false);
                    }
                    else
                    {
                        LoadHealerClass(_pathToHealerClassFile, false, false, true);
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
                    if (_instanceFromOtherAssembly != null)
                    {
                        return ((_instanceFromOtherAssembly.Range < 5f) ? 5f : _instanceFromOtherAssembly.Range);
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
                    return (_obj != null);
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

