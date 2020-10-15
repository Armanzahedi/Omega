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
        Task<IEnumerable<string>> GetServiceImages(int Id);
        Task<IEnumerable<ServicesForListDto>> GetServicesList();
        Task<bool> ServiceHasImage(int Id);
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
        public async Task<bool> ServiceHasImage(int Id)
        {
            dynamic response = await _bzoApiHelper.GetEndpoint($"/Service/item/{Id}/images");

            if(response["success"] == false)
                return false;

            var data = response["data"];
            if (data.Count < 1)
                return false;

            return true;
        }
        public async Task<IEnumerable<string>> GetServiceImages(int Id)
        {
            var imageUrls = new List<string>();
            dynamic response = await _bzoApiHelper.GetEndpoint($"/Service/item/{Id}/images");
            foreach (var item in response["data"])
            {
                var imageUrl = $"{_bzoApiHelper.GetBaseUrl()}/Service/item/{Id}/image/{item.imageId}";
                imageUrls.Add(imageUrl);
            }
            return imageUrls;
        }
    }
}
