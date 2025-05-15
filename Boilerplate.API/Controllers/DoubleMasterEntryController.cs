using Boilerplate.Contracts;
using Boilerplate.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoubleMasterEntryController : BaseApiController
    {
        public readonly IDoubleMasterEntryService _masterEntryService;
        public DoubleMasterEntryController(IDoubleMasterEntryService masterEntryService)
        {
            _masterEntryService = masterEntryService;
        }

        [HttpPost(nameof(Insert))]
        public async Task<IActionResult> Insert([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.SaveData(item, AuthUserName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost(nameof(InsertListData))]
        public async Task<IActionResult> InsertListData([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.SaveListData(item, AuthUserName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.UpdateData(item, AuthUserName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost(nameof(Delete))]
        public async Task<IActionResult> Delete([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.DeleteData(item));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
