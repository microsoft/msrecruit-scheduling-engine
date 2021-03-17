//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.ServicePlatform.Privacy;

namespace CommonLibrary.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// Generic implementation of <see cref="IPrivateDataContainer"/>.
    /// </summary>
    /// <typeparam name="T">Type of sensitive information.</typeparam>
    public class PrivateDataContainer<T> : IPrivateDataContainer
    {
        /// <summary>
        /// Constructs an instance of the <see cref="PrivateDataContainer{T}"/> class.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="privacyLevel"></param>
        public PrivateDataContainer(T value, PrivacyLevel privacyLevel)
        {
            this.Value = value;
            this.PrivacyLevel = privacyLevel;
        }

        /// <summary>
        /// Privacy level.
        /// </summary>
        public PrivacyLevel PrivacyLevel { get; private set; }

        /// <summary>
        /// Gets the original value with sensitive information.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        /// <returns>Underlying value (including sensitive information).</returns>
        public virtual object GetValue()
        {
            return this.Value;
        }
    }
}
