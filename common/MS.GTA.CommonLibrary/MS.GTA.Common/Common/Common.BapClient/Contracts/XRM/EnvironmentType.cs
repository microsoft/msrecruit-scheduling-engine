namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    using MS.GTA.Common.Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment type.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter))]
    public enum EnvironmentType
    {
        /// <summary>
        /// The environment type is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The environment type is Production.
        /// </summary>
        Production,

        /// <summary>
        /// The environment type is Sandbox.
        /// </summary>
        Sandbox,
    }
}
