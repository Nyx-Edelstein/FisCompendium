using System.Runtime.Serialization;

namespace FuzzyTranslator
{
    [DataContract]
    public enum Language
    {
        [EnumMember] Blin,
        [EnumMember] Deku,
        [EnumMember] Fae,
        [EnumMember] Gerudo,
        [EnumMember] Goron,
        [EnumMember] Hylian,
        [EnumMember] Lizalfos,
        [EnumMember] Lynel,
        [EnumMember] Octorok,
        [EnumMember] Wizkin,
        [EnumMember] Zora
    }
}