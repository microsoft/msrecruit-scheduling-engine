//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateAttachmentDocumentTypeExtenstions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Data.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Extensions for enumerations.
    /// </summary>
    public static partial class EnumExtensions
    {
        private static readonly Tuple<CandidateAttachmentDocumentType, string>[] SupportedContentTypes = new Tuple<CandidateAttachmentDocumentType, string>[]
        {
            Tuple.Create(CandidateAttachmentDocumentType.AVI, "video/x-msvideo"),
            Tuple.Create(CandidateAttachmentDocumentType.DOC, "application/msword"),
            Tuple.Create(CandidateAttachmentDocumentType.DOCX, "application/vnd.openxmlformats-officedocument.wordprocessingml.document"),
            Tuple.Create(CandidateAttachmentDocumentType.HTML, "text/html"),
            Tuple.Create(CandidateAttachmentDocumentType.JPG, "image/jpeg"),
            Tuple.Create(CandidateAttachmentDocumentType.MP4, "video/mp4"),
            Tuple.Create(CandidateAttachmentDocumentType.ODT, "application/vnd.oasis.opendocument.text"),
            Tuple.Create(CandidateAttachmentDocumentType.PDF, "application/pdf"),
            Tuple.Create(CandidateAttachmentDocumentType.PPTX, "application/vnd.openxmlformats-officedocument.presentationml.presentation"),
            Tuple.Create(CandidateAttachmentDocumentType.RTF, "application/rtf"),
            Tuple.Create(CandidateAttachmentDocumentType.TXT, "text/plain")
        };

        private static readonly IDictionary<CandidateAttachmentDocumentType, string> ExtensionContentTypeMap = SupportedContentTypes.ToDictionary(s => s.Item1, s => s.Item2);
        private static readonly IDictionary<string, CandidateAttachmentDocumentType> ContentTypeExtensionMap = SupportedContentTypes.ToDictionary(s => s.Item2, s => s.Item1);

        /// <summary>
        /// Converts <see cref="CandidateAttachmentDocumentType"/> to content MIME type.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>Content type of the document.</returns>
        public static string ToContentType(this CandidateAttachmentDocumentType documentType)
        {
            if (ExtensionContentTypeMap.ContainsKey(documentType))
            {
                return ExtensionContentTypeMap[documentType];
            }
            throw new NotSupportedException($"Unknown document type {documentType}");
        }

        /// <summary>
        /// Converts content MIME type to <see cref="CandidateAttachmentDocumentType"/>
        /// </summary>
        /// <param name="contentType">Content type of the document.</param>
        /// <returns><see cref="CandidateAttachmentDocumentType"/> of the document.</returns>
        public static CandidateAttachmentDocumentType ToExtension(this string contentType)
        {
            if (ContentTypeExtensionMap.ContainsKey(contentType))
            {
                return ContentTypeExtensionMap[contentType];
            }
            throw new NotSupportedException($"Unknown content type {contentType}");
        }

        /// <summary>
        /// Gets Entity Enum from View model Enum.
        /// </summary>
        /// <typeparam name="TEnum">Entity Enum Type</typeparam>
        /// <param name="contractEnum">View model Enum</param>
        /// <returns>Enum</returns>
        public static TEnum ToEntityEnum<TEnum>(this Enum contractEnum)
            where TEnum : struct, IConvertible
        {
            if (contractEnum == null)
            {
                return default(TEnum);
            }

            var sourceType = contractEnum.GetType();
            var targetType = typeof(TEnum);

            return (TEnum)Enum.Parse(targetType, contractEnum.ToString(), true);
            throw new ArgumentException("ContractEnum attribute does not match Entity Enum");
        }

        /// <summary>
        /// Gets View Model Enum from Entity Enum
        /// </summary>
        /// <typeparam name="TEnum">Entity Enum Type</typeparam>
        /// <param name="entityEnum">Entity Enum</param>
        /// <param name="assignDefaultIfNotFound">If enum value is not found, assigns default value</param>
        /// <returns>Enum</returns>
        public static TEnum ToContractEnum<TEnum>(this Enum entityEnum, bool assignDefaultIfNotFound = false)
            where TEnum : struct, IConvertible
        {
            if (entityEnum == null)
            {
                return default(TEnum);
            }

            var sourceType = entityEnum.GetType();
            var targetType = typeof(TEnum);

            var fieldInfo = targetType.GetFields();
            foreach (var member in fieldInfo)
            {
                if (string.Equals(member.Name, entityEnum.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return (TEnum)Enum.Parse(targetType, member.Name);
                }
            }

            if (assignDefaultIfNotFound)
            {
                return default(TEnum);
            }

            throw new ArgumentException("ContractEnum attribute does not match Entity Enum");
        }
    }
}
