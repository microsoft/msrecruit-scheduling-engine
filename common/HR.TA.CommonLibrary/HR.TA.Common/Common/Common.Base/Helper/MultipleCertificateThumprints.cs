//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ServicePlatform.Configuration;

    public static class MultipleCertificateThumprints
    {
        /// <summary>
        /// Returns the Multiple Thumbprints
        /// </summary>
        /// <param name="certificateThumbprints">Certificate Thumbprints</param>
        /// <param name="certificateThumbprint">Certificate Thumbprint</param>
        /// <returns>list of strings</returns>
        public static List<string> GetThumbprints(string certificateThumbprints, string certificateThumbprint)
        {
            var thumbprints = new List<string>();

            if (!string.IsNullOrWhiteSpace(certificateThumbprints))
            {
                thumbprints.AddRange(certificateThumbprints.Split(',').Select(t => t.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(certificateThumbprint) && !thumbprints.Contains(certificateThumbprint.Trim()))
            {
                thumbprints.Add(certificateThumbprint.Trim());
            }

            return thumbprints;
        }
    }
}
