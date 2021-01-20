//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SerializeableToJson.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Talent.TalentContracts.TaskService
{
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>The serializeable to json.</summary>
    [DataContract]
    public class SerializeableToJson
    {
        /// <summary>The to content.</summary>
        /// <returns>The <see cref="StringContent"/>.</returns>
        public HttpContent ToContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
        }
    }
}
