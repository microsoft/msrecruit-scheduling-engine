//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using Common.Email.SendGridContracts;
    using Common.Email.GraphContracts;

    /// <summary>
    /// Email properties
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Gets or sets Greeting.
        /// </summary>
        public string Greeting { get; set; }

        /// <summary>
        /// Gets or sets email subject.
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets paragraph one.
        /// </summary>
        public string Paragraph1 { get; set; }

        /// <summary>
        /// Gets or sets paragraph two.
        /// </summary>
        public string Paragraph2 { get; set; }

        /// <summary>
        /// Gets or sets email closing.
        /// </summary>
        public string Closing { get; set; }

        /// <summary>
        /// Gets or sets Talent Engagement Client URL.
        /// </summary>
        public string TalentEngagementClientUrl { get; set; }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        public EmailAddress FromAddress { get; set; }

        /// <summary>
        /// Gets or sets Reply To Address.
        /// </summary>
        public EmailAddress ReplyToAddress { get; set; }

        /// <summary>
        /// Gets or sets To Address.
        /// </summary>
        public EmailAddress ToAddress { get; set; }

        /// <summary>
        /// Gets or sets cc addess
        /// </summary>
        public List<GraphEmailAddress> CcAddress { get; set; }

        /// <summary>
        /// Gets or sets bcc addess
        /// </summary>
        public List<GraphEmailAddress> BccAddress { get; set; }

        /// <summary>
        /// Gets or sets Company Info Footer.
        /// </summary>
        public string CompanyInfoFooter { get; set; }

        /// <summary>
        /// Gets or sets Company Info Footer layout.
        /// </summary>
        public string CompanyFooterLayout { get; set; }

        /// <summary>
        /// Gets or sets Company Name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets Candidate Name.
        /// </summary>
        public string CandidateName { get; set; }

        /// <summary>
        /// Gets or sets Candidate First Name.
        /// </summary>
        public string CandidateFirstName { get; set; }

        /// <summary>
        /// Gets or sets Identity provider.
        /// </summary>
        public string IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets Identity provider user name.
        /// </summary>
        public string IdentityProviderUserName { get; set; }

        /// <summary>
        /// Gets or sets Job Title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets Job Id.
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Gets or sets External Job Id.
        /// </summary>
        public string ExternalJobId { get; set; }

        /// <summary>
        /// Gets or sets PreHeader.
        /// </summary>
        public string PreHeader { get; set; }

        /// <summary>
        /// Gets or sets email header image url.
        /// </summary>
        public string EmailHeaderUrl { get; set; }

        /// <summary>
        /// Gets or sets email header image url.
        /// </summary>
        public string EmailHeader { get; set; }

        /// <summary>
        /// Gets or sets email header image height.
        /// </summary>
        public string EmailHeaderHeight { get; set; }

        /// <summary>
        /// Gets or sets Hiring Manager Name.
        /// </summary>
        public string RequesterName { get; set; }

        /// <summary>
        /// Gets or sets Requester Role.
        /// </summary>
        public string RequesterRole { get; set; }

        /// <summary>
        /// Gets or sets Button Text
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Gets or sets Button Text
        /// </summary>
        public string CandidateButton { get; set; }

        /// <summary>
        /// Gets or sets Location Details
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Hiring Manager Name
        /// </summary>
        public string HiringManagerName { get; set; }

        /// <summary>
        /// Gets or sets Hiring Manager Email
        /// </summary>
        public string HiringManagerEmail { get; set; }

        /// <summary>
        /// Requester Email
        /// </summary>
        public string RequesterEmail { get; set; }

        /// <summary>
        /// First interview Date
        /// </summary>
        public string InterviewDate { get; set; }

        /// <summary>
        /// Gets or sets recruiter Name
        /// </summary>
        public string RecruiterName { get; set; }

        /// <summary>
        /// Gets or sets recruiter Email
        /// </summary>
        public string RecruiterEmail { get; set; }

        /// <summary>
        /// Gets or sets offer expiry date
        /// </summary>
        public string OfferExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets offer URL
        /// </summary>
        public string OfferURL { get; set; }

        /// <summary>
        /// Gets or sets offer approver name
        /// </summary>
        public string OfferApproverName { get; set; }

        /// <summary>
        /// Gets or sets offer approver email id
        /// </summary>
        public string OfferApproverEmail { get; set; }

        /// <summary>
        /// Gets or sets offer rejecter name
        /// </summary>
        public string OfferRejecterName { get; set; }

        /// <summary>
        /// Gets or sets offer rejecter email id
        /// </summary>
        public string OfferRejecterEmail { get; set; }

        /// <summary>
        /// Gets or sets job approver name
        /// </summary>
        public string JobApproverName { get; set; }

        /// <summary>
        /// Gets or sets job approver email
        /// </summary>
        public string JobApproverEmail { get; set; }

        /// <summary>
        /// Gets or sets job requester name
        /// </summary>
        public string JobRequesterName { get; set; }

        /// <summary>
        /// Gets or sets job requester emai
        /// </summary>
        public string JobRequesterEmail { get; set; }
    }
}
