using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum TalentPoolParticipantRole
    {
        [EnumMember(Value = "Owner")]
        Owner = 0,
        [EnumMember(Value = "Contributor")]
        Contributor = 1,
    }
}
