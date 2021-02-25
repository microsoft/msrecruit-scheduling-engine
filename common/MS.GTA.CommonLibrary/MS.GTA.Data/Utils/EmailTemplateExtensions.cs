//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Data.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MS.GTA.Common.Email.GraphContracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.EnumSetModel.SchedulingService;
    using MS.GTA.TalentEntities.Enum;
    
    using CommonEmail = MS.GTA.Common.Email;

    public static class EmailTemplateExtensions
    {
        public const string HiringManager = "Hiring manager";

        public static List<GraphEmailAddress> parseEmailAddressesByRole(List<string> recipients, IDictionary<string, string> templateParams)
        {
            List<GraphEmailAddress> parsedRecipients = new List<GraphEmailAddress>();
            if (recipients != null)
            {
                foreach (string recipient in recipients)
                {
                    if (templateParams.Keys.Contains(recipient))
                    {
                        var emailValue = templateParams[recipient];
                        if (!string.IsNullOrWhiteSpace(emailValue))
                        {
                            var emailRecipients = emailValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var email in emailRecipients)
                            {
                                parsedRecipients.Add(new GraphEmailAddress() { emailAddress = new EmailAddressProperty() { Address = email } });
                            }
                        }
                    }
                    else
                    {
                        parsedRecipients.Add(new GraphEmailAddress() { emailAddress = new EmailAddressProperty() { Address = recipient } });
                    }
                }
            }
            return parsedRecipients;
        }


        public static EmailTemplateLegacy ConvertToSchedulingTemplate(this CommonEmail.Contracts.EmailTemplate template, string companyName)
        {
            if (template == null)
            {
                return null;
            }

            if (!Enum.TryParse(template.TemplateType, out TemplateType templateType))
            {
                throw new CommonEmail.Exceptions.EmailTemplateInvalidOperationException($"Retrieved template has type {template.TemplateType} which is invalid for this service");
            }

            List<EmailContent> contents = new List<EmailContent>();
            AddEmailContentsIfNeeded(contents, EmailContentType.Subject, template.Subject);
            AddEmailContentsIfNeeded(contents, EmailContentType.Header, template.Header);
            AddEmailContentsIfNeeded(contents, EmailContentType.EmailBody, template.Body);
            AddEmailContentsIfNeeded(contents, EmailContentType.Closing, template.Closing);
            AddEmailContentsIfNeeded(contents, EmailContentType.Footer, template.Footer);

            var (ccRoles, ccAddresses) = ExtractJobParticipantRoles(template.Cc);
            var (bccRoles, bccAddresses) = ExtractJobParticipantRoles(template.Bcc);

            return new EmailTemplateLegacy
            {
                Id = template.Id,
                TemplateName = template.TemplateName,
                TemplateType = templateType,
                IsDefault = template.IsDefault,
                IsAutosent = template.isAutosent,
                EmailContent = contents,
                CcEmailAddressRoles = ccRoles,
                CcEmailAddressList = ccAddresses,
                BccEmailAddressRoles = bccRoles,
                BccEmailAddressList = bccAddresses,
            };
        }

        private static void AddEmailContentsIfNeeded(List<EmailContent> contents, EmailContentType contentType, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                contents.Add(new EmailContent
                {
                    EmailContentType = contentType,
                    Order = 0,
                    Content = value
                });
            }
        }

        private static (List<JobParticipantRole> roles, List<string> addresses) ExtractJobParticipantRoles(List<string> recipients)
        {
            var participantRoles = new List<JobParticipantRole>();
            var addresses = new List<string>();
            if (recipients != null)
            {
                foreach (var recipient in recipients.Where(r => r != null))
                {
                    if (Enum.TryParse(recipient.Trim(), out JobParticipantRole role))
                    {
                        participantRoles.Add(role);
                    }
                    else if (recipient.Trim() == HiringManager)
                    {
                        participantRoles.Add(JobParticipantRole.HiringManager);
                    }
                    else
                    {
                        addresses.Add(recipient);
                    }
                }
            }

            return (participantRoles, addresses);
        }
    }
}
