using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikeamind_backend.Core.Models.EFModels.UserModels
{
    public partial class Avatar
    {
        public Avatar()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Column("ImageURL")]
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        [InverseProperty("Avatar")]
        public virtual ICollection<User> Users { get; set; }
    }
}
