namespace MS.GTA.Talent.TalentContracts.ScheduleService.Conferencing
{
    using Microsoft.Graph;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="AudioInfo"/> class stores the audio conferencing details.
    /// </summary>
    [DataContract]
    public class AudioInfo
    {
        /// <summary>
        /// Gets or sets the toll number.
        /// </summary>
        /// <value>
        /// The toll number.
        /// </value>
        [DataMember(Name = "tollNumber")]
        public string TollNumber { get; set; }

        /// <summary>
        /// Gets or sets the toll free number.
        /// </summary>
        /// <value>
        /// The toll free number.
        /// </value>
        [DataMember(Name = "tollFreeNumber")]
        public string TollFreeNumber { get; set; }

        /// <summary>
        /// Gets or sets the conference identifier.
        /// </summary>
        /// <value>
        /// The conference identifier.
        /// </value>
        [DataMember(Name = "conferenceId")]
        public string ConferenceId { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting dial in URL.
        /// </summary>
        /// <value>
        /// The conference meeting dial in URL.
        /// </value>
        [DataMember(Name = "dialInUrl")]
        public string DialInUrl { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="AudioConferencing" /> to <see cref="MS.GTA.Talent.TalentContracts.ScheduleService.Conferencing.AudioInfo" />.
        /// </summary>
        /// <param name="audioConferencing">The instance of <see cref="AudioConferencing"/>.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator AudioInfo(AudioConferencing audioConferencing)
        {
            AudioInfo audioInfo = null;
            if (audioConferencing != null)
            {
                audioInfo = new AudioInfo
                {
                    TollNumber = audioConferencing.TollNumber,
                    TollFreeNumber = audioConferencing.TollFreeNumber,
                    ConferenceId = audioConferencing.ConferenceId,
                    DialInUrl = audioConferencing.DialinUrl
                };
            }

            return audioInfo;
        }
    }
}
