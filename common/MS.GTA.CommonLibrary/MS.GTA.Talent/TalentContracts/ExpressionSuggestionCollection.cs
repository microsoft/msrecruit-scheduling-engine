//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExpressionSuggestionCollection.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The expression suggestion collection contract.
    /// </summary>
    [DataContract]
    public class ExpressionSuggestionCollection
    {
        /// <summary>Gets or sets the Id.</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "reason", EmitDefaultValue = false, IsRequired = false)]
        public string Reason { get; set; }
        
        [DataMember(Name = "suggestion", EmitDefaultValue = false, IsRequired = false)]
        public string Suggestion { get; set; }
        
        [DataMember(Name = "languageCode", EmitDefaultValue = false, IsRequired = false)]
        public string LanguageCode { get; set; }
        
        [DataMember(Name = "isEnabled", EmitDefaultValue = false, IsRequired = false)]
        public bool IsEnabled { get; set; }
        
        [DataMember(Name = "hasDefault", EmitDefaultValue = false, IsRequired = false)]
        public bool HasDefault { get; set; }

        [DataMember(Name = "expressionSuggestions", EmitDefaultValue = false, IsRequired = false)]
        public IList<ExpressionSuggestion> ExpressionSuggestions { get; set; }
    }
}
