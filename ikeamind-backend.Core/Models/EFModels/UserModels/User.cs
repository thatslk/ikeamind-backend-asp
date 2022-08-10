using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikeamind_backend.Core.Models.EFModels.UserModels
{
    public partial class User
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        public long AvatarId { get; set; }
        [Column("Bescore_PF")]
        public long? BescorePf { get; set; }
        [Column("Bestscore_IF")]
        public long? BestscoreIf { get; set; }

        [ForeignKey("AvatarId")]
        [InverseProperty("Users")]
        public virtual Avatar Avatar { get; set; }
    }
}
