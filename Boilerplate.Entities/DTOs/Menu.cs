using Boilerplate.Entities.DBModels;
using System;
using System.Collections.Generic;

namespace Boilerplate.Entities.DTOs
{
    public class Menu
    {
        public string ParentMenuName { get; set; }
        public string ParentMenuLogo { get; set; }
        public Nullable<bool> ParentIsActive { get; set; }
        public string ParentUiLink { get; set; }
        public List<Menus> ChildMenus { get; set; }

        public Menu()
        {

        }

        public Menu(string parentMenuName, string parentMenuLogo, bool parentIsActive, string parentUiLink)
        {
            ParentMenuName = parentMenuName;
            ParentMenuLogo = parentMenuLogo;
            ParentIsActive = parentIsActive;
            ParentUiLink = string.IsNullOrEmpty(parentUiLink) ? "#" : parentUiLink;
        }

        public Menu(string parentMenuName, string parentMenuLogo, bool parentIsActive, string parentUiLink, List<Menus> childMenus)
        {
            ParentMenuName = parentMenuName;
            ParentMenuLogo = parentMenuLogo;
            ParentIsActive = parentIsActive;
            ChildMenus = childMenus;
            ParentUiLink = string.IsNullOrEmpty(parentUiLink) ? "#" : parentUiLink;
        }
    }
}
