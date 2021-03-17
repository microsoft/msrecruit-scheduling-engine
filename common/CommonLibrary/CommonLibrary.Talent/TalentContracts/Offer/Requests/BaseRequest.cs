//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.BusinessLibrary.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommonLibrary.Common.Base.Security;
    using CommonLibrary.ServicePlatform.Context;

    /// <summary>
    /// Base request
    /// </summary>
    public class BaseRequest
    {
        private HCMApplicationPrincipal principal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();

        /// <summary>
        /// Gets or sets prinicpal Object
        /// </summary>
        public HCMApplicationPrincipal Principal
        {
            get
            {
                return this.principal;
            }

            set
            {
                this.principal = value;
            }
        }
    }
}
