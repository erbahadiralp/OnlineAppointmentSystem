using AutoMapper;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Cache;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class ServiceManager : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private const string CacheKeyPrefix = "Service_";

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<ServiceDTO>> GetAllServicesAsync()
        {
            var cacheKey = $"{CacheKeyPrefix}All";
            var cachedServices = await _cacheService.GetAsync<List<ServiceDTO>>(cacheKey);

            if (cachedServices != null)
                return cachedServices;

            var services = await _unitOfWork.Services.GetAllAsync();
            var serviceDTOs = _mapper.Map<List<ServiceDTO>>(services);

            await _cacheService.SetAsync(cacheKey, serviceDTOs, TimeSpan.FromMinutes(30));

            return serviceDTOs;
        }

        public async Task<ServiceDTO> GetServiceByIdAsync(int id)
        {
            var cacheKey = $"{CacheKeyPrefix}{id}";
            var cachedService = await _cacheService.GetAsync<ServiceDTO>(cacheKey);

            if (cachedService != null)
                return cachedService;

            var service = await _unitOfWork.Services.GetByIdAsync(id);
            var serviceDTO = _mapper.Map<ServiceDTO>(service);

            await _cacheService.SetAsync(cacheKey, serviceDTO, TimeSpan.FromMinutes(30));

            return serviceDTO;
        }

        public async Task<List<ServiceDTO>> GetActiveServicesAsync()
        {
            // Clear the cache first
            await _cacheService.RemoveAsync($"{CacheKeyPrefix}Active");

            var services = await _unitOfWork.Services.GetActiveServicesAsync();
            var serviceDTOs = _mapper.Map<List<ServiceDTO>>(services);

            await _cacheService.SetAsync($"{CacheKeyPrefix}Active", serviceDTOs, TimeSpan.FromMinutes(30));

            return serviceDTOs;
        }

        public async Task<List<ServiceDTO>> GetServicesByEmployeeIdAsync(int employeeId)
        {
            var services = await _unitOfWork.Services.GetServicesByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<ServiceDTO>>(services);
        }

        public async Task<bool> CreateServiceAsync(ServiceDTO serviceDTO)
        {
            try
            {
                var service = _mapper.Map<Service>(serviceDTO);
                service.CreatedDate = DateTime.Now;
                service.IsActive = true;

                await _unitOfWork.Services.AddAsync(service);
                await _unitOfWork.CompleteAsync();

                // Cache'i temizle
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}Active");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateServiceAsync(ServiceDTO serviceDTO)
        {
            try
            {
                var existingService = await _unitOfWork.Services.GetByIdAsync(serviceDTO.ServiceId);
                if (existingService == null)
                    return false;

                _mapper.Map(serviceDTO, existingService);
                existingService.UpdatedDate = DateTime.Now;

                _unitOfWork.Services.Update(existingService);
                await _unitOfWork.CompleteAsync();

                // Cache'i temizle
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}Active");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}{serviceDTO.ServiceId}");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            try
            {
                var service = await _unitOfWork.Services.GetByIdAsync(id);
                if (service == null)
                    return false;

                _unitOfWork.Services.Remove(service);
                await _unitOfWork.CompleteAsync();

                // Cache'i temizle
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}Active");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}{id}");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ActivateServiceAsync(int id)
        {
            try
            {
                var service = await _unitOfWork.Services.GetByIdAsync(id);
                if (service == null)
                    return false;

                service.IsActive = true;
                service.UpdatedDate = DateTime.Now;

                _unitOfWork.Services.Update(service);
                await _unitOfWork.CompleteAsync();

                // Cache'i temizle
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}Active");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}{id}");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeactivateServiceAsync(int id)
        {
            try
            {
                var service = await _unitOfWork.Services.GetByIdAsync(id);
                if (service == null)
                    return false;

                service.IsActive = false;
                service.UpdatedDate = DateTime.Now;

                _unitOfWork.Services.Update(service);
                await _unitOfWork.CompleteAsync();

                // Cache'i temizle
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}Active");
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}{id}");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}