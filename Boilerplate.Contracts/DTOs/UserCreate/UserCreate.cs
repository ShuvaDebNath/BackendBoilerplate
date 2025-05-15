namespace Boilerplate.Contracts;

public class UserCreate
{
    public string? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public string? UserTypeId { get; set; }
    public string? UserRoleID { get; set; }
    public bool DashboardPreview { get; set; }
    public string? CountryType { get; set; }
}
