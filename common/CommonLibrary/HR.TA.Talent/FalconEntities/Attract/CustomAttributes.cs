//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System.Runtime.Serialization;
    using HR.TA..TalentEntities.Enum;

    [DataContract]
    public class CustomAttributes
    {
        [DataMember(Name = "AttributeID", EmitDefaultValue = false, IsRequired = false)]
        public string AttributeID { get; set; }

        [DataMember(Name = "EntityType", EmitDefaultValue = false, IsRequired = false)]
        public EntityType EntityType { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "Value", EmitDefaultValue = false, IsRequired = false)]
        public string Value { get; set; }
    }
}
