using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private IBzApiRepository _repo;
        private readonly IMapper _mapper;

        public ServicesController(IBzApiRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllItems")]
        public async Task<object> GetAllItems()
        {
            var response = await _repo.GetServicesList();
            return Ok(response);
        }
    }
}
