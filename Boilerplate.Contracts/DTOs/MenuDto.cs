using Boilerplate.Contracts.DTOs;

namespace Boilerplate.Contracts;

public class MenuDto
{
    public string ParentMenuName { get; set; }
    public string ParentMenuLogo { get; set; }
    public Nullable<bool> ParentIsActive { get; set; }
    public string ParentUiLink { get; set; }
    public List<MenusDto> ChildMenus { get; set; }

    public MenuDto()
    {

    }

    public MenuDto(string parentMenuName, string parentMenuLogo, bool parentIsActive, string parentUiLink)
    {
        ParentMenuName = parentMenuName;
        ParentMenuLogo = parentMenuLogo;
        ParentIsActive = parentIsActive;
        ParentUiLink = string.IsNullOrEmpty(parentUiLink) ? "#" : parentUiLink;
    }

    public MenuDto(string parentMenuName, string parentMenuLogo, bool parentIsActive, string parentUiLink, List<MenusDto> childMenus)
    {
        ParentMenuName = parentMenuName;
        ParentMenuLogo = parentMenuLogo;
        ParentIsActive = parentIsActive;
        ChildMenus = childMenus;
        ParentUiLink = string.IsNullOrEmpty(parentUiLink) ? "#" : parentUiLink;
    }
}
