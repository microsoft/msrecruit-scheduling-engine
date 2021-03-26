//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Talent.TalentContracts.TaskService
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
