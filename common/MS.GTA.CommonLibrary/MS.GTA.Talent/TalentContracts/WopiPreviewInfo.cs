namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>The project artifact.</summary>
    [DataContract]
    public class WopiPreviewInfo
    {
        /// <summary>Gets or sets the WOPI preview url.</summary>
        [DataMember(Name = "previewUrl")]
        public string PreviewUrl { get; set; }

        /// <summary>Gets or sets the user's access token.</summary>
        [DataMember(Name = "accessToken")]
        public string AccessToken { get; set; }

        /// <summary>Gets or sets the access token's time to expiry.</summary>
        [DataMember(Name = "accessTokenTtl")]
        public long AccessTokenTtl { get; set; }
    }
}
