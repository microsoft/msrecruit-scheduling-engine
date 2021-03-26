//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The expression suggestion collection contract.
    /// </summary>
    [DataContract]
    public class ExpressionSuggestionStatistic
    {
        /// <summary>Gets or sets the Id.</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "improperExpressionCount", EmitDefaultValue = false, IsRequired = false)]
        public int ImproperExpressionCount { get; set; }
        
        [DataMember(Name = "suggestionsTakenCount", EmitDefaultValue = false, IsRequired = false)]
        public int SuggestionsTakenCount { get; set; }
    }
}
