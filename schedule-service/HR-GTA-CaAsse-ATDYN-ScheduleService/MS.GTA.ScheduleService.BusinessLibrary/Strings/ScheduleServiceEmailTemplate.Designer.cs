//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Strings {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ScheduleServiceEmailTemplate {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ScheduleServiceEmailTemplate() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MS.GTA.ScheduleService.BusinessLibrary.Strings.ScheduleServiceEmailTemplate", typeof(ScheduleServiceEmailTemplate).Assembly);
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
        ///   Looks up a localized string similar to &lt;!doctype html&gt;
        ///&lt;html&gt;
        ///  &lt;head&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width&quot; /&gt;
        ///    &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=UTF-8&quot; /&gt;
        ///    &lt;title&gt;Microsoft GTA&lt;/title&gt;
        ///    &lt;style&gt;
        ///      /* -------------------------------------
        ///          GLOBAL RESETS
        ///      ------------------------------------- */
        ///      img {
        ///        border: none;
        ///        -ms-interpolation-mode: bicubic;
        ///        max-width: 100%; }
        ///
        ///      body {
        ///        background-color: #f6f6f6;
        ///        font-fami [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EmailTemplateWithoutButton {
            get {
                return ResourceManager.GetString("EmailTemplateWithoutButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BEGIN:VCALENDAR
        ///PRODID:-//Compnay Inc//Product Application//EN
        ///VERSION:2.0
        ///METHOD:PUBLISH
        ///BEGIN:VEVENT
        ///DTSTART:[StartDate]
        ///DTEND:[EndDate]
        ///DTSTAMP:[StartDate]
        ///UID:[GUID]
        ///CREATED:[StartDate]
        ///SUMMARY:[MailSubject]
        ///LOCATION:[Location]
        ///X-ALT-DESC;FMTTYPE=text/html:[HTMLBody]
        ///LAST-MODIFIED:[StartDate]
        ///SEQUENCE:0
        ///STATUS:CONFIRMED
        ///TRANSP:OPAQUE
        ///END:VEVENT
        ///END:VCALENDAR.
        /// </summary>
        internal static string icsTemplate {
            get {
                return ResourceManager.GetString("icsTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!doctype html&gt;
        ///&lt;html&gt;
        ///
        ///&lt;head&gt;
        ///    &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=windows-1256&quot;&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width&quot;&gt;
        ///    &lt;title&gt;Microsoft GTA&lt;/title&gt;
        ///    &lt;style&gt;
        ///        /* -------------------------------------
        ///              GLOBAL RESETS
        ///          ------------------------------------- */
        ///        img {
        ///            border: none;
        ///            -ms-interpolation-mode: bicubic;
        ///            max-width: 100%;
        ///        }
        ///
        ///        body {
        ///            bac [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ReponsiveEmailLayout {
            get {
                return ResourceManager.GetString("ReponsiveEmailLayout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!doctype html&gt;
        ///&lt;html&gt;
        ///  &lt;head&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width&quot; /&gt;
        ///    &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=UTF-8&quot; /&gt;
        ///    &lt;title&gt;Microsoft GTA&lt;/title&gt;
        ///    &lt;style&gt;
        ///      /* -------------------------------------
        ///          GLOBAL RESETS
        ///      ------------------------------------- */
        ///      img {
        ///        border: none;
        ///        -ms-interpolation-mode: bicubic;
        ///        max-width: 100%; }
        ///
        ///      body {
        ///        background-color: #f6f6f6;
        ///        font-fami [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ResponsiveEmailTemplate {
            get {
                return ResourceManager.GetString("ResponsiveEmailTemplate", resourceCulture);
            }
        }
    }
}
