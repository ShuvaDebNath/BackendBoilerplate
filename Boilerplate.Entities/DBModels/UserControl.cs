using System;

namespace Boilerplate.Entities.DBModels
{
    public class UserControl
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserTypeId { get; set; }
        public string MenuId { get; set; }
        public string MakeBy { get; set; }
        public Nullable<System.DateTime> MakeDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<bool> DashboardPreview { get; set; }
        public string UserRoleID { get; set; }
        public string CountryType { get; set; }
    }
}
