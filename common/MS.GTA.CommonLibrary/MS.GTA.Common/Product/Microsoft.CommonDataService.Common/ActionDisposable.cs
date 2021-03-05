using System;

namespace Microsoft.CommonDataService.Common.Internal
{
    public sealed class ActionDisposable : IDisposable
    {
        public ActionDisposable(Action action) { }

        public void Dispose() { }
    }
}
