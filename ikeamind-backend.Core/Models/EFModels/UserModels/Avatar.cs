using System.Collections.Generic;

namespace ikeamind_backend.Core.Models.EFModels.UserModels
{
    public partial class Avatar
    {
        public Avatar()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
