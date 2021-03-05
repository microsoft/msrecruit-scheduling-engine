using System;

namespace Microsoft.CommonDataService.Common.Internal
{
    public sealed class VoidDisposable : IDisposable
    {
        public static VoidDisposable Instance { get; }

        public void Dispose() { }
    }
}
