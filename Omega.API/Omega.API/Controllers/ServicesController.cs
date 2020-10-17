using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omega.Infrastructure.Helpers;
using Omega.Infrastructure.Repositories;

namespace Omega.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private IServicesRepository _repo;

        public ServicesController(IServicesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<object> GetAllServices()
        {
            var response = await _repo.GetServicesListDto();
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/Images")]
        public async Task<object> GetServiceImages(int id)
        {
            var serviceHasImage = await _repo.ServiceHasImage(id);
            if (serviceHasImage == false)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "گالری مجود نیست" });
            var images = await _repo.GetServiceImages(id);
            return Ok(images);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<object> GetService(int id)
        {
            var service = await _repo.GetServiceDetails(id);
            if (service == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "خدماتی یافته نشد" });

            return Ok(service);
        }

        [HttpGet]
        [Route("UpdateServicesTable")]
        [Authorize]

        public async Task<object> UpdateServicesTable()
        {
            var databaseUpdated = await _repo.UpdateServicesTable();

            if (databaseUpdated == false)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "مشکلی در هنگام گزارش گیری رخ داد" });

            return Ok();
        }
        [HttpGet]
        [Route("GetReport")]
        [Authorize]

        public async Task<object> GetServicesReport(string searchString,string from,string to)
        {

            #region Generating date filters
            DateTime dateValue;
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrEmpty(from))
                if (DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                    fromDate = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(to))
                if (DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                    toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            #endregion


            var services = await _repo.GetServicesReport(searchString,fromDate,toDate);
            if (services == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "خدماتی یافته نشد" });

            return Ok(services);
        }
    }
}
