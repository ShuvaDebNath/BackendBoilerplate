namespace Boilerplate.Contracts;

public class User
{
    public int RowIndex { get; set; }
    public string Id { get; set; }
    public string MenuId { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string UserTypeName { get; set; }
    public Nullable<bool> isActive { get; set; }
    public Nullable<bool> DashboardPreview { get; set; }
}
