namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    using MS.GTA.Common.Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment SKU.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter))]
    public enum EnvironmentSku
    {
        /// <summary>
        /// The environment SKU is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The Standard SKU
        /// </summary>
        Standard,

        /// <summary>
        /// The Premium SKU
        /// </summary>
        Premium,

        /// <summary>
        /// The Developer SKU
        /// </summary>
        Developer,

        /// <summary>
        /// The Basic SKU
        /// </summary>
        Basic,

        /// <summary>
        /// The CDS2.0 Production sku
        /// </summary>
        Production,

        /// <summary>
        /// The CDS2.0 Sandbox sku
        /// </summary>
        Sandbox,

        /// <summary>
        /// The CDS2.0 Trial sku
        /// </summary>
        Trial,

        /// <summary>
        /// The CDS2.0 Default sku
        /// </summary>
        Default,
    }
}
