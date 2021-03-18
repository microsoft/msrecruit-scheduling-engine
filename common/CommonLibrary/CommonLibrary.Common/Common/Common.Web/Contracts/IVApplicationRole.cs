//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Web.Contracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum IVApplicationRole
    {
        IVAdmin = 0,
        IVReadOnly = 1,
    }
}
