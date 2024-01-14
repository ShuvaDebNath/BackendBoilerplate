namespace Boilerplate.Entities.DBModels
{
    public class PagewiseAction
    {
        public string ActionID { get; set; }
        public string UserId { get; set; }
        public int MenuId { get; set; }
        public string ActionPermission { get; set; }
        public string[] ActionPermissionArrya { get; set; }
        public virtual UserControl tblUserControl { get; set; }
    }
}
