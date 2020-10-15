using AutoMapper;
using Newtonsoft.Json;
using Omega.Infrastructure.Dtos;
using Omega.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Infrastructure.Repositories
{
    public interface IBzApiRepository
    {
        Task<IEnumerable<ServicesForListDto>> GetServicesList();
    }

    public class BzApiRepository : IBzApiRepository
    {
        private IBzApiHelper _bzoApiHelper;
        private readonly IMapper _mapper;

        public BzApiRepository(IBzApiHelper bzoApiHelper, IMapper mapper)
        {
            _bzoApiHelper = bzoApiHelper;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ServicesForListDto>> GetServicesList()
        {
            var servicesList = new List<ServicesForListDto>();
            dynamic response = await _bzoApiHelper.GetEndpoint("/Service/GetAllItems");
            foreach (var item in response["data"])
            {
                var service = new ServicesForListDto();
                service.ServiceId = item.serviceId;
                service.Name = item.name;
                service.ImageUrl = $"{_bzoApiHelper.GetBaseUrl()}/Service/item/{service.ServiceId}/image";
                servicesList.Add(service);
            }
            return servicesList;
        }
    }
}
