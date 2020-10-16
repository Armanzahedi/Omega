using AutoMapper;
using Newtonsoft.Json.Linq;
using Omega.Core.Models;
using Omega.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Infrastructure.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Mapping service json object to service class
            CreateMap<JObject, Service>().ForMember(d => d.ServiceId, opt => {
                opt.MapFrom(src => src["serviceId"]);
            }).ForMember(d => d.Name, opt => {
                opt.MapFrom(src => src["name"]);
            }).ForMember(d => d.Code, opt => {
                opt.MapFrom(src => src["code"]);
            }).ForMember(d => d.Price, opt => {
                opt.MapFrom(src => src["price"]);
            }).ForMember(d => d.IsDefault, opt => {
                opt.MapFrom(src => src["isDefault"]);
            }).ForMember(d => d.Qty, opt => {
                opt.MapFrom(src => src["qty"]);
            }).ForMember(d => d.MinQty, opt => {
                opt.MapFrom(src => src["minQty"]);
            }).ForMember(d => d.MaxQty, opt => {
                opt.MapFrom(src => src["maxQty"]);
            }).ForMember(d => d.Description, opt => {
                opt.MapFrom(src => src["description"]);
            }).ForMember(d => d.ContractorSharePercent, opt => {
                opt.MapFrom(src => src["contractorSharePercent"]);
            }).ForMember(d => d.ContractorId, opt => {
                opt.MapFrom(src => src["contractorId"]);
            }).ForMember(d => d.ContractorName, opt => {
                opt.MapFrom(src => src["contractorName"]);
            }).ForMember(d => d.UnitMeasureId, opt => {
                opt.MapFrom(src => src["unitMeasureId"]);
            }).ForMember(d => d.UnitMeasureName, opt => {
                opt.MapFrom(src => src["unitMeasureName"]);
            });
            #endregion

            CreateMap<Service, ServicesForListDto>();
            CreateMap<Service, ServicesForDetailDto>();
            CreateMap<Service, ServicesForReportDto>();
        }
    }
}
