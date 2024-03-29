﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using HR.TA.CommonDataService.Common.Internal;

namespace HR.TA.ServicePlatform.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SettingsSectionAttribute : Attribute
    {
        private readonly string sectionName;

        public SettingsSectionAttribute(string sectionName)
        {
            Contract.CheckNonEmpty(sectionName, nameof(sectionName));

            this.sectionName = sectionName;
        }

        public string SectionName
        {
            get { return sectionName; }
        }
    }
}
