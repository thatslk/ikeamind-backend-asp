namespace ikeamind_backend.Core.Models.EFModels.UserModels
{
    public partial class AvatarDescriptionByLocale
    {
        public int AvatarId { get; set; }
        public string LocaleCode { get; set; }
        public string Description { get; set; }

        public virtual Avatar Avatar { get; set; }
    }
}
