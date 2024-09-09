using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Minimal_Api.Domain.Enum
{
    [DataContract]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumProfiles
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Edit")]
        Edit
    }
}