using ikeamind_backend.Core.Models.DTOs;
using System;
using System.Collections.Generic;

namespace ikeamind_backend.Core.Models.ReturnModels
{
    public record AccountPageRM
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public List<AvatarDTO> Avatars { get; set; }
        public int CurrentAvatarId { get; set; }
        public int TitleFirstBestscore { get; set; }
        public int PictureFirstBestscore { get; set; }
    }
}
