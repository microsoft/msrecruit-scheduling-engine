namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    public class XRMEnvironmentDefinition
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the resource properties.
        /// </summary>
        public EnvironmentProperties Properties { get; set; }
    }
}
