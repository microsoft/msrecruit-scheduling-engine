//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TalentTagAssociationExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class TalentTagAssociationExtensions
    {
        public static ApplicantTag ToViewModel(this TalentTagAssociation talentTagAssociation) => talentTagAssociation == null ? null : new ApplicantTag()
        {
            Id = talentTagAssociation.RecId.ToString(),
            Name = talentTagAssociation.TalentTag?.Tag
        };
    }
}
