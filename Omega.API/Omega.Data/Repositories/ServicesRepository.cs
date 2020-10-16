using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omega.Core.Models;
using Omega.Infrastructure.Dtos;
using Omega.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Infrastructure.Repositories
{
    public interface IServicesRepository
    {
        Task<IEnumerable<Service>> GetServices();
        Task<IEnumerable<string>> GetServiceImages(int Id);
        Task<IEnumerable<ServicesForListDto>> GetServicesListDto();
        Task<bool> ServiceHasImage(int Id);
        Task<ServicesForDetailDto> GetServiceDetails(int id);
        Task<List<ServicesForReportDto>> GetServicesReport(string searchString, DateTime? fromDate, DateTime? toDate);
        Task<bool> UpdateServicesTable();
        bool CompareServices(Service oldService, Service newService);
    }

    public class ServicesRepository : IServicesRepository
    {
        private IBzApiHelper _bzoApiHelper;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;

        public ServicesRepository(IBzApiHelper bzoApiHelper, IMapper mapper,MyDbContext context)
        {
            _bzoApiHelper = bzoApiHelper;
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<Service>> GetServices()
        {
            var services = new List<Service>();
            JObject response = (JObject)await _bzoApiHelper.GetEndpoint("/Service/GetAllItems");
            foreach (var item in response["data"])
            {
                var service = _mapper.Map<Service>(item);
                services.Add(service);
            }
            return services;
        }
        public async Task<IEnumerable<ServicesForListDto>> GetServicesListDto()
        {
            var servicesList = new List<ServicesForListDto>();
            var services = await GetServices();
            foreach (var service in services)
            {
                var serviceForListDto = _mapper.Map<ServicesForListDto>(service);
                serviceForListDto.ImageUrl = $"{_bzoApiHelper.GetBaseUrl()}/Service/item/{service.ServiceId}/image";
                servicesList.Add(serviceForListDto);
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
        public async Task<ServicesForDetailDto> GetServiceDetails(int id)
        {
            dynamic response = await _bzoApiHelper.GetEndpoint($"/Service/find/{id}");
            if (response["success"] == false)
                return null;

            JObject data = response["data"];
            var service = _mapper.Map<Service>(data);
            var serviceDetailDto = _mapper.Map<ServicesForDetailDto>(service);
            serviceDetailDto.ImageUrl = $"{_bzoApiHelper.GetBaseUrl()}/Service/item/{serviceDetailDto.ServiceId}/image";

            return serviceDetailDto;
        }
        public async Task<List<ServicesForReportDto>> GetServicesReport(string searchString,DateTime? fromDate,DateTime? toDate)
        {
            var servicesForReportList = new List<ServicesForReportDto>();
            var services = await _context.Services.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
                services = services.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
            if (fromDate != null)
                services = services.Where(s => s.AddedDate >= fromDate || s.ModifiedDate >= fromDate).ToList();
            if (toDate != null)
                services = services.Where(s => s.AddedDate <= toDate || s.ModifiedDate <= toDate).ToList();

            int number = 0;
            foreach (var service in services)
            {
                number++;
                var serviceReportDto = _mapper.Map<ServicesForReportDto>(service);
                serviceReportDto.Number = number;
                servicesForReportList.Add(serviceReportDto);
            }
            servicesForReportList = servicesForReportList.OrderBy(s => s.Number).ToList();
            return servicesForReportList;
        }
        public async Task<bool> UpdateServicesTable()
        {
            try
            {
                var existingServices = await _context.Services.ToListAsync();
                var recievedServices = await GetServices();

                foreach (var service in recievedServices)
                {
                    if (existingServices.Where(s => s.ServiceId == service.ServiceId || s.Name == service.Name).Any())
                    {
                        var oldService = existingServices.Where(s => s.ServiceId == service.ServiceId || s.Name == service.Name).FirstOrDefault();
                        var serviceHasChanged = CompareServices(oldService, service);
                        if (serviceHasChanged)
                        {
                            service.Id = oldService.Id;
                            service.AddedDate = oldService.AddedDate;
                            service.ModifiedDate = DateTime.Now;
                            service.Deleted = false;
                            _context.Entry(oldService).State = EntityState.Detached;
                            _context.Entry(service).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        existingServices.Remove(oldService);
                    }
                    else
                    {
                        service.AddedDate = DateTime.Now;
                        _context.Services.Add(service);
                        await _context.SaveChangesAsync();
                    }
                }
                if (existingServices.Any())
                {
                    foreach (var item in existingServices)
                    {
                        item.Deleted = true;
                        _context.Entry(item).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool CompareServices(Service oldService,Service newService)
        {
            var difs = ObjectComparer.Compare(oldService, newService);

            // It always has 3 differences (Id, AddedDate, ModifiedDate)
            if (difs.Where(d=>d.Prop != "Id" && d.Prop != "AddedDate" && d.Prop != "ModifiedDate").Any())
                return true;

            return false;
        }
    }
}
