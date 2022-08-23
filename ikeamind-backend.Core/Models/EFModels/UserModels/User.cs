using System;

namespace ikeamind_backend.Core.Models.EFModels.UserModels
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int AvatarId { get; set; }
        public int? BestscoreTf { get; set; }
        public int? BestscorePf { get; set; }

        public virtual Avatar Avatar { get; set; }
    }
}
