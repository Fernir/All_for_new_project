﻿#pragma checksum "..\..\..\..\Controls\OptionsUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8EF68C3B858817873D6595EED179F108"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Alex.WoWRelogger.Controls {
    
    
    /// <summary>
    /// OptionsUserControl
    /// </summary>
    public partial class OptionsUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ImportCommands;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReloadUI;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReloadAccounts;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateAccount;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AllowTrialsCheckbox;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label AllowTrialsLabel;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AutoCreateAccounts;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\Controls\OptionsUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WmrBalance;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WoWRelogger;component/controls/optionsusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\OptionsUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ImportCommands = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.ImportCommands.Click += new System.Windows.RoutedEventHandler(this.ReloadCommandsButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ReloadUI = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.ReloadUI.Click += new System.Windows.RoutedEventHandler(this.ReloadUIButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ReloadAccounts = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.ReloadAccounts.Click += new System.Windows.RoutedEventHandler(this.ReloadAccountsButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.button = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.button.Click += new System.Windows.RoutedEventHandler(this.button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.CreateAccount = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.CreateAccount.Click += new System.Windows.RoutedEventHandler(this.CreateAccountButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.AllowTrialsCheckbox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 51 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.AllowTrialsCheckbox.Click += new System.Windows.RoutedEventHandler(this.AllowTrialsCheckbox_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AllowTrialsLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.AutoCreateAccounts = ((System.Windows.Controls.CheckBox)(target));
            
            #line 53 "..\..\..\..\Controls\OptionsUserControl.xaml"
            this.AutoCreateAccounts.Click += new System.Windows.RoutedEventHandler(this.AutoCreateAccounts_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.WmrBalance = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

