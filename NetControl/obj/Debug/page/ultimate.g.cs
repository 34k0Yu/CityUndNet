﻿#pragma checksum "..\..\..\page\ultimate.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FA02F81F463143E55F45F46DA81133F932561E5F"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.DXBinding;
using NetControl;
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


namespace NetControl {
    
    
    /// <summary>
    /// ultimate
    /// </summary>
    public partial class ultimate : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inpid;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock validate_ID;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inpna;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock validate_Name;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inppa;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inphet;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\page\ultimate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inpsped;
        
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
            System.Uri resourceLocater = new System.Uri("/NetControl;component/page/ultimate.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\page\ultimate.xaml"
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
            this.inpid = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\page\ultimate.xaml"
            this.inpid.LostFocus += new System.Windows.RoutedEventHandler(this.Inpid_LostFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.validate_ID = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.inpna = ((System.Windows.Controls.TextBox)(target));
            
            #line 39 "..\..\..\page\ultimate.xaml"
            this.inpna.LostFocus += new System.Windows.RoutedEventHandler(this.Inpna_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.validate_Name = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.inppa = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.inphet = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.inpsped = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            
            #line 65 "..\..\..\page\ultimate.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

