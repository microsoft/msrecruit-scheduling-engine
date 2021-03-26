//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of the validation on a Ruleset
    /// </summary>
    [DataContract]
    public class RulesetValidationResult
    {
        /// <summary>
        /// Gets or sets a <see cref="RulesetValidationResultCode"/> indicating whether ruleset is valid or not.
        /// </summary>
        [DataMember(Name = "validationCode", IsRequired = true)]
        public RulesetValidationResultCode ValidationCode { get; set; }

        /// <summary>
        /// Gets or sets the details when the ruleset is invalid.
        /// </summary>
        [DataMember(Name = "validationMessage", IsRequired = true)]
        public string ValidationMessage { get; set; }
    }
}
