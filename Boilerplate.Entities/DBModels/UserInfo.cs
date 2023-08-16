using Boilerplate.Entities.Common;

namespace Boilerplate.Entities.DBModels
{
    public class UserInfo : BaseEntity
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ComId { get; set; }
        public string MenuId { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public string UserTypeId { get; set; }
        public string? PasswordPin { get; set; }
    }
}