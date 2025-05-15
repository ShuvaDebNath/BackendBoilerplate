
namespace Boilerplate.Contracts.DTOs;

public class MenusDto
{
    public int MenuId { get; set; }
    public string MenuName { get; set; }
    public string SubMenuName { get; set; }
    public string UiLink { get; set; }
    public Nullable<bool> isActive { get; set; }
    public Nullable<bool> ysnParent { get; set; }
    public Nullable<int> OrderBy { get; set; }
    public Nullable<System.DateTime> MakeDate { get; set; }
    public string MenuLogo { get; set; }
}
