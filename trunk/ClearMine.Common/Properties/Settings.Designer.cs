﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearMine.Common.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Beginner")]
        public global::ClearMine.Common.Model.Difficulty Difficulty {
            get {
                return ((global::ClearMine.Common.Model.Difficulty)(this["Difficulty"]));
            }
            set {
                this["Difficulty"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9")]
        public int Rows {
            get {
                return ((int)(this["Rows"]));
            }
            set {
                this["Rows"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9")]
        public int Columns {
            get {
                return ((int)(this["Columns"]));
            }
            set {
                this["Columns"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int Mines {
            get {
                return ((int)(this["Mines"]));
            }
            set {
                this["Mines"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
                    <heroList xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                        <heroOnLevel level=""beginner"" played=""0"" won=""0"" longestWinning=""0"" longestLosing=""0"" current=""0"" average=""0"">
                            <records />
                        </heroOnLevel>
                        <heroOnLevel level=""interMediate"" played=""0"" won=""0"" longestWinning=""0"" longestLosing=""0"" current=""0"" average=""0"">
                            <records />
                        </heroOnLevel>
                        <heroOnLevel level=""advanced"" played=""0"" won=""0"" longestWinning=""0"" longestLosing=""0"" current=""0"" average=""0"">
                            <records />
                        </heroOnLevel>
                    </heroList>
                ")]
        public global::ClearMine.Common.Model.HeroHistoryList HeroList {
            get {
                return ((global::ClearMine.Common.Model.HeroHistoryList)(this["HeroList"]));
            }
            set {
                this["HeroList"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n        <map xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"h" +
            "ttp://www.w3.org/2001/XMLSchema\">\r\n        </map>\r\n      ")]
        public global::ClearMine.Common.Utilities.DataMap DataMap {
            get {
                return ((global::ClearMine.Common.Utilities.DataMap)(this["DataMap"]));
            }
            set {
                this["DataMap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PlaySound {
            get {
                return ((bool)(this["PlaySound"]));
            }
            set {
                this["PlaySound"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowQuestionMark {
            get {
                return ((bool)(this["ShowQuestionMark"]));
            }
            set {
                this["ShowQuestionMark"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SaveOnExit {
            get {
                return ((bool)(this["SaveOnExit"]));
            }
            set {
                this["SaveOnExit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoContinueSaved {
            get {
                return ((bool)(this["AutoContinueSaved"]));
            }
            set {
                this["AutoContinueSaved"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PlayAnimation {
            get {
                return ((bool)(this["PlayAnimation"]));
            }
            set {
                this["PlayAnimation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AlwaysNewGame {
            get {
                return ((bool)(this["AlwaysNewGame"]));
            }
            set {
                this["AlwaysNewGame"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SaveGame {
            get {
                return ((bool)(this["SaveGame"]));
            }
            set {
                this["SaveGame"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".")]
        public string GameHistoryFolder {
            get {
                return ((string)(this["GameHistoryFolder"]));
            }
            set {
                this["GameHistoryFolder"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".cmg")]
        public string SavedGameExt {
            get {
                return ((string)(this["SavedGameExt"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SavedGame.cmg")]
        public string UnfinishedGameFileName {
            get {
                return ((string)(this["UnfinishedGameFileName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\Fadefy\\ClearMine\\ScreenShoots\\")]
        public string ScreenShotFolder {
            get {
                return ((string)(this["ScreenShotFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("yy-MM-dd-HH-mm-ss")]
        public string ScreenShotFileTimeFormat {
            get {
                return ((string)(this["ScreenShotFileTimeFormat"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("fadefy@gmail.com")]
        public string FeedBackEmail {
            get {
                return ((string)(this["FeedBackEmail"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool WavingFlag {
            get {
                return ((bool)(this["WavingFlag"]));
            }
            set {
                this["WavingFlag"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ClearMineHelp.chm")]
        public string HelpDocumentName {
            get {
                return ((string)(this["HelpDocumentName"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowTooManyFlagsWarning {
            get {
                return ((bool)(this["ShowTooManyFlagsWarning"]));
            }
            set {
                this["ShowTooManyFlagsWarning"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AccurateTime {
            get {
                return ((bool)(this["AccurateTime"]));
            }
            set {
                this["AccurateTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CurrentLanguage {
            get {
                return ((string)(this["CurrentLanguage"]));
            }
            set {
                this["CurrentLanguage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("luna.normalcolor")]
        public string CurrentTheme {
            get {
                return ((string)(this["CurrentTheme"]));
            }
            set {
                this["CurrentTheme"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CustomLanguageFile {
            get {
                return ((string)(this["CustomLanguageFile"]));
            }
            set {
                this["CustomLanguageFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CustomThemeFile {
            get {
                return ((string)(this["CustomThemeFile"]));
            }
            set {
                this["CustomThemeFile"] = value;
            }
        }
    }
}
