//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA..ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Holds skype interview
    /// </summary>
    public class SkypeInterview
    {
        /// <summary>
        /// Gets or sets the Skype urls
        /// </summary>
        public List<Url> Urls { get; set; }

        /// <summary>
        /// Gets or sets the tokens
        /// </summary>
        public List<Token> Tokens { get; set; }

        /// <summary>
        /// Gets or sets the code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the passcode
        /// </summary>
        public object Passcode { get; set; }

        /// <summary>
        /// Gets or sets the participants
        /// </summary>
        public List<object> Participants { get; set; }

        /// <summary>
        /// Gets or sets the schedulings
        /// </summary>
        public object Scheduling { get; set; }

        /// <summary>
        /// Gets or sets the stage
        /// </summary>
        public Stage Stage { get; set; }

        /// <summary>
        /// Gets or sets the coding config
        /// </summary>
        public CodingConfig CodingConfig { get; set; }

        /// <summary>
        /// Gets or sets the skype config
        /// </summary>
        public SkypeConfig SkypeConfig { get; set; }

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        public object Position { get; set; }

        /// <summary>
        /// Gets or sets the capabilities
        /// </summary>
        public Capabilities Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the tasks
        /// </summary>
        public List<object> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the snapshots
        /// </summary>
        public List<object> Snapshots { get; set; }

        /// <summary>
        /// Gets or sets feedbacks
        /// </summary>
        public List<object> Feedbacks { get; set; }

        /// <summary>
        /// Gets or sets the notes
        /// </summary>
        public List<object> Notes { get; set; }

        /// <summary>
        /// Gets or sets the notesconfig
        /// </summary>
        public List<AddonsConfig> AddonsConfig { get; set; }
    }

    /// <summary>
    /// Holds url
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class Url
    {
        /// <summary>
        /// Gets or sets url
        /// </summary>
        public string SkypeUrl { get; set; }

        /// <summary>
        /// Gets or sets type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets participant type
        /// </summary>
        public object ParticipantType { get; set; }

        /// <summary>
        /// Gets or sets participant
        /// </summary>
        public string Participant { get; set; }
    }

    /// <summary>
    /// Holds token entity
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class Token
    {
        /// <summary>
        /// Gets or sets token header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets token value
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Holds the stage entity
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class Stage
    {
        /// <summary>
        /// Gets or sets value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the metadata
        /// </summary>
        public string Metadata { get; set; }
    }

    /// <summary>
    /// Holds the codingconfig
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class CodingConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets code execution
        /// </summary>
        public bool CodeExecution { get; set; }

        /// <summary>
        /// Gets or sets coding languages
        /// </summary>
        public List<string> CodingLanguages { get; set; }

        /// <summary>
        /// Gets or sets the default coding language
        /// </summary>
        public string DefaultCodingLanguage { get; set; }
    }

    /// <summary>
    /// Holds skype config entity
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class SkypeConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets call
        /// </summary>
        public bool Call { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets chat
        /// </summary>
        public bool Chat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets lobby
        /// </summary>
        public bool Lobby { get; set; }
    }

    /// <summary>
    /// Holds capabilities entity
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class Capabilities
    {
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets code editor
        /// </summary>
        public bool CodeEditor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets skype
        /// </summary>
        public bool Skype { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets feedbacks
        /// </summary>
        public bool Feedbacks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets notes
        /// </summary>
        public bool Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets emails
        /// </summary>
        public bool Emails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets addons
        /// </summary>
        public bool Addons { get; set; }
    }

    /// <summary>
    /// Holds the settings config
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class Settings
    {
        /// <summary>
        /// Gets or sets the theme
        /// </summary>
        public string Theme { get; set; }
    }

    /// <summary>
    /// Holds the add ons config
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class AddonsConfig
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets the configs
        /// </summary>
        public object Configs { get; set; }
    }
}