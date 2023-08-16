using System.Collections.Generic;

namespace Boilerplate.Entities.DTOs.UserCreate
{
    public class UserMenuAssign
    {
        public UserCreate Data { get; set; }
        public List<MenuPerssion> DetailsData { get; set; }
    }
}
