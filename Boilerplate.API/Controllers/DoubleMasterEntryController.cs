using Boilerplate.Entities.DBModels;
using Boilerplate.Service.Interfaces;
using Boilerplate.Service.Message;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        //[HttpPost(nameof(GetAll))]
        //public IActionResult GetAll([FromBody] MasterEntryModel item)
        //{
        //    var dt = _masterEntryService.GetAll(item);

        //    if (dt != null)
        //    {
        //        var data = JsonConvert.SerializeObject(dt);
        //        return Ok(MessageType.DataFound(data));
        //    }

        //    return Ok(MessageType.NotFound(null));
        //}

        //[HttpPost(nameof(GetByColumns))]
        //public IActionResult GetByColumns([FromBody] MasterEntryModel item)
        //{
        //    var dt = _masterEntryService.GetByColumns(item);

        //    if (dt != null)
        //    {
        //        var data = JsonConvert.SerializeObject(dt);
        //        return Ok(MessageType.DataFound(data));
        //    }

        //    return Ok(MessageType.NotFound(null));
        //}

        [HttpPost(nameof(Insert))]
        public async Task<IActionResult> Insert([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.SaveData(item, AuthUserName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost(nameof(InsertListData))]
        public async Task<IActionResult> InsertListData([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.SaveListData(item, AuthUserName));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.UpdateData(item, AuthUserName));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost(nameof(Delete))]
        public async Task<IActionResult> Delete([FromBody] DoubleMasterEntryModel item)
        {
            try
            {
                return Ok(await _masterEntryService.DeleteData(item));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
