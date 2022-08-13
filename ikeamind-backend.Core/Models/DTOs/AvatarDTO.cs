using System.Collections.Generic;

namespace ikeamind_backend.Core.Models.DTOs
{
    public record AvatarDTO
    {
        public int AvatarId { get; set; }
        public string AvatarName { get; set; }
        public string AvatarImageUrl { get; set; }
        public Dictionary<string, string> AvatarDescriptionByLocales { get; set; }
    }
}
