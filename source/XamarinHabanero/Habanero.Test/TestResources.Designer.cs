﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Habanero.Test
{
    using System;


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TestResources
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TestResources()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Habanero.Test.TestResources", typeof(TestResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static System.Drawing.Bitmap TestJpeg
        {
            get
            {
                object obj = ResourceManager.GetObject("TestJpeg", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }

        internal static System.Drawing.Bitmap TestJpeg2
        {
            get
            {
                object obj = ResourceManager.GetObject("TestJpeg2", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }

        internal static System.Drawing.Bitmap TestJpeg3
        {
            get
            {
                object obj = ResourceManager.GetObject("TestJpeg3", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }

        internal static System.Drawing.Bitmap TestPhoto
        {
            get
            {
                object obj = ResourceManager.GetObject("TestPhoto", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to sample text.
        /// </summary>
        internal static string TestText
        {
            get
            {
                return ResourceManager.GetString("TestText", resourceCulture);
            }
        }
    }
}
