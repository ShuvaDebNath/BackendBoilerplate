
using Boilerplate.Contracts;
using Boilerplate.Contracts.Responses;
using Boilerplate.Contracts.Services;
using Boilerplate.Entities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Boilerplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        protected readonly IUserService _createUserService;
        protected readonly IAuthService _authService;
        public UserController(IUserService createUserService,
            IAuthService authService)
        {
            _createUserService = createUserService;
            _authService = authService;
        }

        #region GetAllUser
        [HttpPost(nameof(GetAllUser))]
        public async Task<IActionResult> GetAllUser([FromBody] DataTableParams dataTableParams)
        {
            var list = await _createUserService.GetAllUser(dataTableParams);

            if (list.data.Count() > 0)
                return Ok(MessageType.DataFound(new { list.data, list.totalCount, list.filterCount }));

            return Ok(MessageType.NotFound(list));
        }
        #endregion

        [HttpPost(nameof(ActiveInactive))]
        public async Task<IActionResult> ActiveInactive(string userId, bool isActive)
        {
            var isActiveOrInactive = await _createUserService.ActiveInactive(userId, isActive);

            if (isActiveOrInactive)
                return Ok(MessageType.ProcessSuccess(userId));

            return Ok(MessageType.ProcessError(null));
        }

        [HttpPost(nameof(CheckPassword))]
        public async Task<IActionResult> CheckPassword([FromBody] SetPassword model)
        {
            var passwordhash = HelperExtention.Hash(model.NewPassword);

            var isExist = await _createUserService.CheckPassword(passwordhash);

            if (isExist)
            {
                return Ok(MessageType.DataFound(isExist));
            }

            return Ok(MessageType.NotFound(isExist));
        }

        [HttpPost(nameof(ResetPassword))]
        public async Task<IActionResult> ResetPassword([FromBody] SetPassword model)
        {
            if (model.Id ==null || model.NewPassword == null || model.ConfirmPassword == null)
            {
                return Ok(MessageType.ProcessError(null));
            }

            var passwordhash = HelperExtention.Hash(model.NewPassword);

            var isReset = await _createUserService.ResetPassword(passwordhash,model.Id);

            if (isReset)
                return Ok(MessageType.ProcessSuccess(model));

            return Ok(MessageType.ProcessError(null));
        }

        [HttpGet(nameof(GetUserBasicData))]
        public async Task<IActionResult> GetUserBasicData()
        {
            DataSet ds = await _createUserService.GetUserBasicData();

            if (ds != null)
            {
                var tables = JsonConvert.SerializeObject(ds);
                return Ok(MessageType.DataFound(tables));
            }

            return Ok(MessageType.NotFound(null));
        }

        [HttpPost(nameof(SaveUser))]
        public async Task<IActionResult> SaveUser([FromBody] UserMenuAssign data)
        {
            
            var isSuccess = await _createUserService.SaveUser(data, AuthEmail);

            if (isSuccess)
            {
                return Ok(MessageType.SaveSuccess(data));
            }

            return Ok(MessageType.ProcessError(null));
        }

        [HttpPost(nameof(EditUser))]
        public async Task<IActionResult> EditUser([FromBody] UserMenuAssign data)
        {

            var isSuccess = await _createUserService.EditUser(data, AuthEmail);

            if (isSuccess)
            {
                return Ok(MessageType.UpdateSuccess(data));
            }

            return Ok(MessageType.UpdateError(null));
        }

        [HttpPost(nameof(DeleteUser))]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var isDeleted = await _createUserService.DeleteUser(userId);

            if (isDeleted)
                return Ok(MessageType.DeleteSuccess(userId));

            return Ok(MessageType.DeleteError(null));
        }

        [HttpGet(nameof(GetUserEditData))]
        public async Task<IActionResult> GetUserEditData(string userId)
        {
            var data = await _createUserService.GetUserEditData(userId);

            if (data.userName != null && data.selectedMenu != null)
            {
                var userInfo = JsonConvert.SerializeObject(
                        new { userName = data.userName, selectedMenu = data.selectedMenu}
                    );

                return Ok(MessageType.DataFound(userInfo));
            }

            return Ok(MessageType.NotFound(null));
        }
    }
}
