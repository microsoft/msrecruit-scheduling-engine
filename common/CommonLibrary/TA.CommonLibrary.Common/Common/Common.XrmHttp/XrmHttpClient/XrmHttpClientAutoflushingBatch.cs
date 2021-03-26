//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System.Threading.Tasks;

    /// <summary>
    /// An OData batch containing actions (possibly inside changesets).
    /// </summary>
    public class XrmHttpClientAutoflushingBatch : XrmHttpClientBatch, IXrmHttpClientBatch
    {
        private readonly int maximumActionsPerBatch;

        public XrmHttpClientAutoflushingBatch(IXrmHttpClient client, int maximumActionsPerBatch)
            : base(client)
        {
            this.maximumActionsPerBatch = maximumActionsPerBatch;
        }

        /// <summary>
        /// Executes the batch.
        /// </summary>
        /// <returns>The task to await.</returns>
        /// <remarks>Executes all callbacks for returned data; users should use that to get access to query results.</remarks>
        public new async Task ExecuteAsync()
        {
            foreach (var realBatch in this.Split(this.maximumActionsPerBatch))
            {
                await realBatch.ExecuteAsync();
            }
        }
    }
}
