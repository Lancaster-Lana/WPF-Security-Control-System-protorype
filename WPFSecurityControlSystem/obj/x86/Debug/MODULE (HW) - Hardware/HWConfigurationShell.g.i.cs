﻿#pragma checksum "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "83406DDFD005AA2C272360E354A65CC0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
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
using WPFSecurityControlSystem;
using WPFSecurityControlSystem.Commands;
using WPFSecurityControlSystem.MODULE.HWConfiguration.Controls;
using WPFSecurityControlSystem.MODULE.HWConfiguration.Views;


namespace WPFSecurityControlSystem {
    
    
    /// <summary>
    /// HWConfigurationShell
    /// </summary>
    public partial class HWConfigurationShell : Microsoft.Windows.Controls.Ribbon.RibbonWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition colCenter;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.Ribbon.Ribbon MainRibbonMenu;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.Ribbon.RibbonButton rbtnDownload;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.Ribbon.RibbonButton rbtnSetDefaults;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel pnlLeftTools;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnHideLeftTools;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid pnlLeftTabs;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl NavigationRegion;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl ClientArea;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel pnlRightTools;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnHidRightTools;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer pnlRightAccordionTools;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl ToolsRegion;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MainRegionBorder;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFSecurityControlSystem;component/module%20(hw)%20-%20hardware/hwconfigurations" +
                    "hell.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
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
            
            #line 20 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.CanCreate);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddHardware);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 21 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.CanEdit);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.EditHardware);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 22 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.CanDelete);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.DeleteHardware);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 25 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.GenerateDoorsForController);
            
            #line default
            #line hidden
            return;
            case 5:
            this.colCenter = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 6:
            this.MainRibbonMenu = ((Microsoft.Windows.Controls.Ribbon.Ribbon)(target));
            return;
            case 7:
            
            #line 46 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            ((Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OnClose);
            
            #line default
            #line hidden
            return;
            case 8:
            this.rbtnDownload = ((Microsoft.Windows.Controls.Ribbon.RibbonButton)(target));
            return;
            case 9:
            this.rbtnSetDefaults = ((Microsoft.Windows.Controls.Ribbon.RibbonButton)(target));
            return;
            case 10:
            this.pnlLeftTools = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 11:
            this.btnHideLeftTools = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 88 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            this.btnHideLeftTools.Click += new System.Windows.RoutedEventHandler(this.btnHideLeftTools_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.pnlLeftTabs = ((System.Windows.Controls.Grid)(target));
            return;
            case 13:
            this.NavigationRegion = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 14:
            this.ClientArea = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 15:
            this.pnlRightTools = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 16:
            this.btnHidRightTools = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 101 "..\..\..\..\MODULE (HW) - Hardware\HWConfigurationShell.xaml"
            this.btnHidRightTools.Click += new System.Windows.RoutedEventHandler(this.btnHidRightTools_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.pnlRightAccordionTools = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 18:
            this.ToolsRegion = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 19:
            this.MainRegionBorder = ((System.Windows.Controls.Border)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

