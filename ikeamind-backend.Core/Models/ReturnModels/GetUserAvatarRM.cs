using ikeamind_backend.Core.Models.DTOs;

namespace ikeamind_backend.Core.Models.ReturnModels
{
    public record GetUserAvatarRM
    {
        public AvatarDTO Avatar { get; set; }
    }
}
