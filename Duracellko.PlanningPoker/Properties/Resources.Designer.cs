﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Duracellko.PlanningPoker.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Duracellko.PlanningPoker.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IAsyncResult object was not provided by BeginGetMessage method..
        /// </summary>
        internal static string Error_EndGetMessagesAsyncResult {
            get {
                return ResourceManager.GetString("Error_EndGetMessagesAsyncResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Member name cannot be longer than 50 characters..
        /// </summary>
        internal static string Error_MemberNameTooLong {
            get {
                return ResourceManager.GetString("Error_MemberNameTooLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create Scrum Team &quot;{0}&quot;. Team with that name already exists..
        /// </summary>
        internal static string Error_ScrumTeamAlreadyExists {
            get {
                return ResourceManager.GetString("Error_ScrumTeamAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scrum Team &quot;{0}&quot; does not exist..
        /// </summary>
        internal static string Error_ScrumTeamNotExist {
            get {
                return ResourceManager.GetString("Error_ScrumTeamNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scrum Team cannot be opened..
        /// </summary>
        internal static string Error_ScrumTeamTimeout {
            get {
                return ResourceManager.GetString("Error_ScrumTeamTimeout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Team name cannot be longer than 50 characters..
        /// </summary>
        internal static string Error_TeamNameTooLong {
            get {
                return ResourceManager.GetString("Error_TeamNameTooLong", resourceCulture);
            }
        }
    }
}
