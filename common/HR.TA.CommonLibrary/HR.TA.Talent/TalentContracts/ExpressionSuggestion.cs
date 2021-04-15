//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The expression suggestion contract.
    /// </summary>
    [DataContract]
    public class ExpressionSuggestion
    {
        /// <summary>Gets or sets the Id.</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "improperExpression", EmitDefaultValue = false, IsRequired = false)]
        public string ImproperExpression { get; set; }
        
        [DataMember(Name = "suggestedExpression", EmitDefaultValue = false, IsRequired = false)]
        public string SuggestedExpression { get; set; }
    }
}
