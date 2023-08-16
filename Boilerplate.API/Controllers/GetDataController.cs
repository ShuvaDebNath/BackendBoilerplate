using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Service.Interfaces;
using Boilerplate.API.Message;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Boilerplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataController : BaseApiController
    {
        public readonly IGetDataService _getDataService;

        public GetDataController(IGetDataService getDataService)
        {
            _getDataService = getDataService;
        }

        [HttpPost(nameof(GetAllData))]
        public async Task<IActionResult> GetAllData([FromBody] GetDataModel model)
        {
            try
            {
                return Ok(await _getDataService.GetAllData(model));
            }
            catch (Exception)
            {
               throw;
            }
        }

        [HttpPost(nameof(GetDataById))]
        public async Task<IActionResult> GetDataById([FromBody] GetDataModel model)
        {
            try
            {
                return Ok(await _getDataService.GetDataById(model));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost(nameof(GetInitialData))]
        public async Task<IActionResult> GetInitialData([FromBody] GetDataModel model)
        {
            try
            {
                return Ok(await _getDataService.GetInitialData(model));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
