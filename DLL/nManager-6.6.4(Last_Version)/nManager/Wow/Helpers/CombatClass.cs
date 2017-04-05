namespace nManager.Wow.Helpers
{
    using Microsoft.CSharp;
    using nManager;
    using nManager.Annotations;
    using nManager.Helpful;
    using nManager.Wow.Class;
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
        private static string _anamehuosKanadalap = "";
        private const float _deikaevi = 4f;
        private static ICombatClass _ihuohod;
        private static string _ikuijaofokSa = "";
        private static Assembly _jataveipujumuUleivo;
        private static Thread _moaviPoaweam;
        private static Thread _navoumuDiosoaq;
        [UsedImplicitly]
        private static BigInteger _uleugoetoxuo = 0x3b9aca00;
        private const float _uqiriKak = 1.33f;
        private static object _ximuvGox;
        private static readonly object CombatClassLocker = new object();

        public static bool AboveMinRange(WoWUnit unit)
        {
            return (unit.GetDistance < 2f);
        }

        public static void DisposeCombatClass()
        {
            try
            {
                lock (CombatClassLocker)
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
                Logging.WriteError("DisposeCombatClass(): " + exception, true);
            }
            finally
            {
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
            }
        }

        private static float Exifiovemieloi(WoWUnit voiquni, bool ulexiAfuasokao = true)
        {
            float num = voiquni.GetCombatReach + nManager.Wow.ObjectManager.ObjectManager.Me.GetCombatReach;
            if (ulexiAfuasokao)
            {
                num += 1.33f;
            }
            if (ulexiAfuasokao && (num < 4f))
            {
                num = 4f;
            }
            return num;
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
            return (unit.GetDistance < (Exifiovemieloi(unit, true) - 0.2f));
        }

        public static bool InRange(WoWUnit unit)
        {
            if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
            {
                return HealerClass.InRange(unit);
            }
            return (unit.GetDistance < (Exifiovemieloi(unit, true) + (Ariedicabepan() ? -1f : GetRange)));
        }

        public static bool InSpellRange(WoWUnit unit, float minRange, float maxRange)
        {
            try
            {
                if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
                {
                    return HealerClass.InCustomRange(unit, minRange, maxRange);
                }
                float getDistance = unit.GetDistance;
                float num2 = (maxRange <= 5f) ? Exifiovemieloi(unit, true) : Exifiovemieloi(unit, false);
                return ((getDistance <= (num2 + maxRange)) && ((System.Math.Abs(minRange) < 0.001) || (getDistance >= (num2 + minRange))));
            }
            catch (Exception exception)
            {
                Logging.WriteError("CombatClass > InCustomRange: " + exception, true);
                return false;
            }
        }

        public static void LoadCombatClass()
        {
            lock (CombatClassLocker)
            {
                if (((_moaviPoaweam == null) || !_moaviPoaweam.IsAlive) && ((_navoumuDiosoaq == null) || !_navoumuDiosoaq.IsAlive))
                {
                    Thread thread = new Thread(new ThreadStart(CombatClass.LoadCombatClassThread)) {
                        Name = "Load Combat Class"
                    };
                    _navoumuDiosoaq = thread;
                    _navoumuDiosoaq.Start();
                }
            }
        }

        public static void LoadCombatClass(string pathToCombatClassFile, bool settingOnly = false, bool resetSettings = false, bool cSharpFile = true)
        {
            try
            {
                _ikuijaofokSa = pathToCombatClassFile;
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
                    string str = File.OpenText(pathToCombatClassFile).ReadToEnd();
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
                    _anamehuosKanadalap = "CombatClass CS";
                }
                else
                {
                    _jataveipujumuUleivo = Assembly.LoadFrom(_ikuijaofokSa);
                    _ximuvGox = _jataveipujumuUleivo.CreateInstance("Main", false);
                    _anamehuosKanadalap = "CombatClass DLL";
                }
                if ((_ximuvGox != null) && (_jataveipujumuUleivo != null))
                {
                    _ihuohod = _ximuvGox as ICombatClass;
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
                Logging.WriteError("LoadCombatClass(string _pathToCombatClassFile): " + exception, true);
            }
        }

        public static void LoadCombatClassThread()
        {
            try
            {
                if (nManagerSetting.CurrentSetting.CombatClass != "")
                {
                    string str;
                    if (nManagerSetting.CurrentSetting.CombatClass == "OfficialTnbClassSelector")
                    {
                        str = string.Concat(new object[] { Application.StartupPath, @"\CombatClasses\OfficialTnbClassSelector\Tnb_", nManager.Wow.ObjectManager.ObjectManager.Me.WowClass, "Rotations.dll" });
                    }
                    else
                    {
                        str = Application.StartupPath + @"\CombatClasses\" + nManagerSetting.CurrentSetting.CombatClass;
                    }
                    if (str.Substring(str.Length - 3) == "dll")
                    {
                        LoadCombatClass(str, false, false, false);
                    }
                    else
                    {
                        LoadCombatClass(str, false, false, true);
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
                    if (_ikuijaofokSa.Substring(_ikuijaofokSa.Length - 3) == "dll")
                    {
                        LoadCombatClass(_ikuijaofokSa, false, false, false);
                    }
                    else
                    {
                        LoadCombatClass(_ikuijaofokSa, false, false, true);
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

        private static bool _litao
        {
            get
            {
                return (GetRange <= 5f);
            }
        }

        public static float GetAggroRange
        {
            get
            {
                try
                {
                    if (_ihuohod != null)
                    {
                        if (_ihuohod.AggroRange < _ihuohod.Range)
                        {
                            return _ihuohod.Range;
                        }
                        return ((_ihuohod.AggroRange < 1.5f) ? 1.5f : _ihuohod.AggroRange);
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

        public static Spell GetLightHealingSpell
        {
            get
            {
                if (((_ihuohod != null) && (_ihuohod.LightHealingSpell != null)) && (_ihuohod.LightHealingSpell.Id > 0))
                {
                    return _ihuohod.LightHealingSpell;
                }
                return new Spell(0);
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
                        return ((_ihuohod.Range < 1.5f) ? 1.5f : _ihuohod.Range);
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
                    return ((_moaviPoaweam != null) && _moaviPoaweam.IsAlive);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCombatClass: " + exception, true);
                    return false;
                }
            }
        }
    }
}

