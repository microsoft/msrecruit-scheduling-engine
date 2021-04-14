//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Utils
{
    /// <summary>
    /// Constants across Offer Engagement service.
    /// </summary>
    public static class BusinessConstants
    {
        /// <summary>
        /// Company Information footer html for send grid email
        /// </summary>
        public const string CompanyInfoFooter = @"CompanyInfoFooter";

        /// <summary>
        /// Company Info Footer for graph emails
        /// </summary>
        public const string SendGridInfoFooter = @"SendGridFooter";

        /// <summary>
        /// Add Candidate Header Url
        /// </summary>
        public const string AddCandidateHeaderUrl = "https://onboard.talent.dynamics.com/assets/images/Cameron_ApplicationReceived.png";

        /// <summary>
        /// Candidate Schedule Url
        /// </summary>
        public const string CandidateScheduleUrl = "https://onboard.talent.dynamics.com/assets/images/Cameron_Scheduler_Availability.png";

        /// <summary>
        /// Candidate Schedule Url
        /// </summary>
        public const string OfferHeaderUrl = "https://onboard.talent.dynamics.com/assets/images/Cameron_Offer.png";

        /// <summary>
        /// Microsoft Logo Url
        /// </summary>
        public const string MicrosoftLogoUrl = "https://msrefer.microsoft.com/assets/images/microsoft-logo/microsoft-logo.png";

        /// <summary>
        /// Assessment Url
        /// </summary>
        public const string AssessmentHeaderUrl = "https://onboard.talent.dynamics.com/assets/images/Cameron_Assessment.png";

        /// <summary>
        /// Add Ruleset Header Url
        /// </summary>
        public const string AddRulesetHeaderUrl = "https://onboard.talent.dynamics.com/assets/images/Attract_Generic.png";

        /// <summary>
        /// Microsoft Info Footer
        /// </summary>
        public const string MicrosoftInfoFooter = "<p><a href='{Privacy_Policy_Link}'>Microsoft Data Privacy Notice</a>. See our <a href='{Terms_And_Conditions_Link}'>Terms of use</a>.</p>";

        /// <summary>
        /// Microsoft Privacy Policy Url
        /// </summary>
        public const string PrivacyPolicyUrl = "http://go.microsoft.com/fwlink/?LinkId=518021";

        /// <summary>
        /// Microsoft Terms And Conditions Url
        /// </summary>
        public const string TermsAndConditionsUrl = "https://careers.microsoft.com/us/en/legalpolicies";

        /// <summary>
        /// Email header height
        /// </summary>
        public const string EmailHeaderHeight1 = "200";

        /// <summary>
        /// Email Header height
        /// </summary>
        public const string EmailHeaderHeight2 = "128";

        /// <summary>
        /// Email Header height
        /// </summary>
        public const string EmailHeaderHeight3 = "90";

        /// <summary>
        /// Email Header height
        /// </summary>
        public const string EmailHeaderHeight4 = "25";

        /// <summary>
        /// Email Header width
        /// </summary>
        public const string EmailHeaderWidth1 = "615";

        /// <summary>
        /// Email Header width
        /// </summary>
        public const string EmailHeaderWidth2 = "100";

        /// <summary>
        /// Token URL
        /// </summary>
        public const string TokenUrl = "?token={0}";

        /// <summary>
        /// Download Error Log URL
        /// </summary>
        public const string DownloadErrorLogUrl = "/v1/ruleset/{0}/errorlog";

        /// <summary>
        /// Approve Offer Url for Approvers
        /// </summary>
        public const string ApproveOfferUrl = "offer/{0}/approve";

        /// <summary>
        /// Approve Offer Url for Approvers
        /// </summary>
        public const string PrepareOfferUrl = "offer/{0}/prepare";

        /// <summary>
        /// Rulset Type
        /// </summary>
        public const string RulesetType = "Ruleset";

        /// <summary>
        /// Rulset Data Type
        /// </summary>
        public const string RulesetDataType = "RulesetData";

        /// <summary>
        /// Template Rulset Type
        /// </summary>
        public const string TemplateRulesetType = "TemplateRuleset";

        /// <summary>
        /// signed offer documents zip attachment file name
        /// </summary>
        public const string SignedOfferDocumentsZipFileName = "offerDocuments.zip";

        /// <summary>
        /// Test candidate email address
        /// </summary>
        public const string TestCandidateEmailAddress = "d365test2@gmail.com";

        /// <summary>
        /// Test candidate name
        /// </summary>
        public const string TestCandidateName = "D365 OfferTest";

        /// <summary>
        /// Placeholder Style on Templace preview with merging with the value
        /// </summary>
        public const string TemplatePreviewPlaceholderStyle = "<span style=\"color: #666666\">{0}</span>";

        /// <summary>
        /// Default Template Name for Notification
        /// </summary>
        public const string DefaultTemplateName = "ResponsiveEmailTemplate";

        /// <summary>
        /// Template Name for Send to Interviewer Notification
        /// </summary>
        public const string InterviewerTemplateName = "ReponsiveEmailLayout";

        /// <summary>
        /// Email template without any action button
        /// </summary>
        public const string EmailTemplateWithoutButton = "EmailTemplateWithoutButton";

        /// <summary>
        /// The ics template.
        /// </summary>
        public const string ICSTemplate = "icsTemplate";

        /// <summary>
        /// Get All Tokens Url
        /// </summary>
        public const string GetAllTokensUrl = "v1/token";

        /// <summary>
        ///  Job Application status updated to offer Api url
        /// </summary>
        public const string AttractJobApplicationOfferUrl = "v5.0/applications/{0}/offer";

        /// <summary>The length limit for upload file.</summary>
        public const int UploadFileLengthLimit = 25000000;

        /// <summary>The size limit for upload file in MB.</summary>
        public const int UploadFileSizeMBLimit = 25;

        /// <summary>
        ///  1 and 40 alphanumeric characters with special characters '`,-,,'
        /// </summary>
        public const string NameValidation = @"^[\p{L}\p{Zs}\p{Lu}\p{Ll}\'\d\,\-]{1,40}$";

        /// <summary>
        /// Content Type Application json i.e. "application/json"
        /// </summary>
        public const string ContentTypeApplicationJson = "application/json";

        /// <summary>
        /// Environment App ID Header
        /// </summary>
        public const string EnvironmentAppIdHeader = "x-ms-environment-id";

        /// <summary>
        /// Name of the file for offer template artifact
        /// </summary>
        public const string OfferTemplateArtifactName = "OfferTemplateDocument.html";

        /// <summary>
        /// Html Open Span Text
        /// </summary>
        public const string HtmlOpenSpanText = "<span";

        /// <summary>
        /// Html Span Regex
        /// </summary>
        public const string HtmlSpanRegex = "<([/]?)span";

        /// <summary>
        /// Regex to find variables in offer template
        /// </summary>
        public const string ScheduleTemplateVariableRegex = @"<span class=""token-key"">([a-zA-Z0-9-]+)";

        /// <summary>
        /// Template variable class string to find tokens
        /// </summary>
        public const string ScheduleTemplateVariableClass = @"class=""alignment token-identifier""";

        /// <summary>
        /// Invitation Application Data
        /// </summary>
        public const string InvitationApplicationData = "ApplicationData";

        /// <summary>
        /// Length of column headers allowed
        /// </summary>
        public const int MaxColumnHeaderLengthAllowed = 30;

        /// <summary>
        /// Maximum number of columns allowed
        /// </summary>
        public const int MaxNumberOfColumnsAllowedInRulesetFile = 11;

        /// <summary>
        /// CSV file extension
        /// </summary>
        public const string CsvFileExtension = ".csv";

        /// <summary>
        /// MaxMetadataColumnValueLength
        /// </summary>
        public const int MaxMetadataColumnValueLength = 40;

        /// <summary>
        /// MaxLastColumnValueLength
        /// </summary>
        public const int MaxLastColumnValueLength = 4000;

        /// <summary>
        /// Clause token opening tag
        /// </summary>
        public const string ClauseFieldIdentifier = "_Clause";

        /// <summary>
        /// Regex to find clause token in offer, anything of the form $[[ ]]$
        /// </summary>
        public const string OfferClauseTokenRegex = @"\$\[\[([^\[$]+)]]\$";

        /// <summary>The cleanup threshold of Inprogress ruleset- in Days</summary>
        public const int InprogressRulesetCleanupThreshold = 1;

        /// <summary>JobOpening Extended Attribute HireType</summary>
        public const string JobOpeningHireType = "hiretype";

        /// <summary>JobOpening Extended Attribute HireType Value</summary>
        public const string UniversityHireType = "university";
    }
}
