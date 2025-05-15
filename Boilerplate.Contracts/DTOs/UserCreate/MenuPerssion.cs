using System;

namespace Boilerplate.Contracts;

public class MenuPerssion
{
    public int MenuId { get; set; }
    public string? SubMenuName { get; set; }
    public bool selected { get; set; }
    public bool insert { get; set; }
    public bool update { get; set; }
    public bool delete { get; set; }
    public bool approve { get; set; }
    public Nullable<bool> ysnParent { get; set; }
    public Nullable<int> OrderBy { get; set; }
}
