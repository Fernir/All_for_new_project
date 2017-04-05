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

    public class CombatClass
    {
        private static Assembly _assembly;
        private static BigInteger _forceBigInteger = 0x3b9aca00;
        private static ICombatClass _instanceFromOtherAssembly;
        private static object _obj;
        private static string _pathToCombatClassFile = "";
        private static string _threadName = "";
        private static Thread _worker;
        private const float BASE_MELEERANGE_OFFSET = 1.3f;
        private const float MIN_ATTACK_DISTANCE = 5f;

        public static bool AboveMinRange(WoWUnit unit)
        {
            return (unit.GetDistance < 2f);
        }

        private static float CombatDistance(WoWUnit unit, bool MeleeSpell = true)
        {
            float num = unit.GetCombatReach + nManager.Wow.ObjectManager.ObjectManager.Me.GetCombatReach;
            if (MeleeSpell)
            {
                num += 1.3f;
            }
            if (MeleeSpell && (num < 5f))
            {
                num = 5f;
            }
            return num;
        }

        public static void DisposeCombatClass()
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
                Logging.WriteError("DisposeCombatClass(): " + exception, true);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static bool InAggroRange(WoWUnit unit)
        {
            if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
            {
                return HealerClass.InRange(unit);
            }
            float num2 = unit.GetCombatReach + GetAggroRange;
            return (unit.GetDistance < num2);
        }

        public static bool InMeleeRange(WoWUnit unit)
        {
            return (unit.GetDistance < (CombatDistance(unit, true) - 0.2f));
        }

        public static bool InRange(WoWUnit unit)
        {
            if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
            {
                return HealerClass.InRange(unit);
            }
            return (unit.GetDistance < (CombatDistance(unit, true) + (isMeleeClass ? -1f : GetRange)));
        }

        public static bool InSpellRange(WoWUnit unit, float minRange, float maxRange)
        {
            try
            {
                float num2;
                if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
                {
                    return HealerClass.InCustomRange(unit, minRange, maxRange);
                }
                float getDistance = unit.GetDistance;
                if (maxRange <= 5f)
                {
                    num2 = CombatDistance(unit, true);
                }
                else
                {
                    num2 = CombatDistance(unit, false);
                }
                return ((getDistance <= (num2 + maxRange)) && ((minRange == 0f) || (getDistance >= (num2 + minRange))));
            }
            catch (Exception exception)
            {
                Logging.WriteError("CombatClass > InCustomRange: " + exception, true);
                return false;
            }
        }

        public static void LoadCombatClass()
        {
            Thread thread2 = new Thread(new ThreadStart(CombatClass.LoadCombatClassThread)) {
                Name = "Load Combat Class"
            };
            thread2.Start();
        }

        public static void LoadCombatClass(string pathToCombatClassFile, bool settingOnly = false, bool resetSettings = false, bool CSharpFile = true)
        {
            try
            {
                _pathToCombatClassFile = pathToCombatClassFile;
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
                    string str = File.OpenText(pathToCombatClassFile).ReadToEnd();
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
                    _threadName = "CombatClass CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToCombatClassFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "CombatClass DLL";
                }
                if ((_obj != null) && (_assembly != null))
                {
                    _instanceFromOtherAssembly = _obj as ICombatClass;
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
                Logging.WriteError("LoadCombatClass(string _pathToCombatClassFile): " + exception, true);
            }
        }

        public static void LoadCombatClassThread()
        {
            try
            {
                if (nManagerSetting.CurrentSetting.CombatClass != "")
                {
                    string pathToCombatClassFile = Application.StartupPath + @"\CombatClasses\" + nManagerSetting.CurrentSetting.CombatClass;
                    if (pathToCombatClassFile.Substring(pathToCombatClassFile.Length - 3) == "dll")
                    {
                        LoadCombatClass(pathToCombatClassFile, false, false, false);
                    }
                    else
                    {
                        LoadCombatClass(pathToCombatClassFile, false, false, true);
                    }
                }
                else
                {
                    Logging.Write("No custom class selected");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCombatClass(): " + exception, true);
            }
        }

        public static void ResetCombatClass()
        {
            try
            {
                if (IsAliveCombatClass)
                {
                    DisposeCombatClass();
                    Thread.Sleep(0x3e8);
                    if (_pathToCombatClassFile.Substring(_pathToCombatClassFile.Length - 3) == "dll")
                    {
                        LoadCombatClass(_pathToCombatClassFile, false, false, false);
                    }
                    else
                    {
                        LoadCombatClass(_pathToCombatClassFile, false, false, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetCombatClass(): " + exception, true);
            }
        }

        public static void ResetConfigurationCombatClass(string filePath)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 3) == "dll")
                {
                    LoadCombatClass(filePath, true, true, false);
                }
                else
                {
                    LoadCombatClass(filePath, true, true, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCombatClass(): " + exception, true);
            }
        }

        public static void ShowConfigurationCombatClass(string filePath)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 3) == "dll")
                {
                    LoadCombatClass(filePath, true, false, false);
                }
                else
                {
                    LoadCombatClass(filePath, true, false, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCombatClass(): " + exception, true);
            }
        }

        public static float GetAggroRange
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                    {
                        if (_instanceFromOtherAssembly.AggroRange < _instanceFromOtherAssembly.Range)
                        {
                            return _instanceFromOtherAssembly.Range;
                        }
                        return ((_instanceFromOtherAssembly.AggroRange < 5f) ? 5f : _instanceFromOtherAssembly.AggroRange);
                    }
                    return 1.5f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CombatClass > GetAggroRange: " + exception, true);
                    return 1.5f;
                }
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
                        return ((_instanceFromOtherAssembly.Range < 1.5f) ? 1.5f : _instanceFromOtherAssembly.Range);
                    }
                    return 1.5f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CombatClass > GetRange: " + exception, true);
                    return 1.5f;
                }
            }
        }

        public static bool IsAliveCombatClass
        {
            get
            {
                try
                {
                    return (_obj != null);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCombatClass: " + exception, true);
                    return false;
                }
            }
        }

        private static bool isMeleeClass
        {
            get
            {
                return (GetRange <= 5f);
            }
        }
    }
}

