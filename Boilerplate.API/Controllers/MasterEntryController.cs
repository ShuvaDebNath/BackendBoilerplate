using Boilerplate.Contracts;
using Boilerplate.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterEntryController : BaseApiController
    {
        public readonly IMasterEntryService _masterEntryService;

        public MasterEntryController(IMasterEntryService masterEntryService)
        {
            _masterEntryService = masterEntryService;
        }

        [HttpPost(nameof(GetAll))]
        public IActionResult GetAll([FromBody] MasterEntryModel item)
        {
            try
            {
                return Ok(_masterEntryService.GetAll(item));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost(nameof(GetByColumns))]
        public IActionResult GetByColumns([FromBody] MasterEntryModel item)
        {
            try
            {
                return Ok(_masterEntryService.GetByColumns(item));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] MasterEntryModel item)
        {
            try
            {
                return Ok(_masterEntryService.Insert(item, AuthUserName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [HttpPut(nameof(Update))]
        public IActionResult Update([FromBody]MasterEntryModel item)
        {
            try
            {
                return Ok(_masterEntryService.Update(item, AuthUserName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [HttpDelete(nameof(Delete))]
        public IActionResult Delete([FromBody]MasterEntryModel item)
        {
            try
            {
                return Ok(_masterEntryService.Delete(item));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
