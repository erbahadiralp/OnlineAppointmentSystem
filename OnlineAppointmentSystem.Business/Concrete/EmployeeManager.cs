using AutoMapper;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            // Repository'de özel bir metot kullanın
            var employees = await _unitOfWork.Employees.GetAllWithDetailsAsync();
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            // Repository'de özel bir metot kullanın
            var employee = await _unitOfWork.Employees.GetEmployeeWithUserByIdAsync(id);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetEmployeeByUserIdAsync(string userId)
        {
            // Repository'de özel bir metot kullanın
            var employee = await _unitOfWork.Employees.GetEmployeeByUserIdAsync(userId);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<List<EmployeeDTO>> GetActiveEmployeesAsync()
        {
            // Repository'de özel bir metot kullanın
            var employees = await _unitOfWork.Employees.GetActiveEmployeesAsync();
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }

        public async Task<List<EmployeeDTO>> GetEmployeesByServiceIdAsync(int serviceId)
        {
            // Repository'de özel bir metot kullanın
            var employees = await _unitOfWork.Employees.GetEmployeesByServiceIdAsync(serviceId);
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }

        public async Task<bool> CreateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            try
            {
                // Kullanıcı zaten var mı kontrol et
                var existingEmployee = await _unitOfWork.Employees.GetEmployeeByUserIdAsync(employeeDTO.UserId);
                if (existingEmployee != null)
                {
                    Console.WriteLine($"Employee already exists with UserId: {employeeDTO.UserId}");
                    return false;
                }

                var employee = _mapper.Map<Employee>(employeeDTO);
                employee.CreatedDate = DateTime.Now;
                employee.IsActive = true;

                Console.WriteLine($"Creating new employee with UserId: {employee.UserId}");
                Console.WriteLine($"Employee details - Name: {employeeDTO.FirstName} {employeeDTO.LastName}, Email: {employeeDTO.Email}");

                await _unitOfWork.Employees.AddAsync(employee);
                var saveResult = await _unitOfWork.CompleteAsync();
                Console.WriteLine($"Save result: {saveResult}");

                if (saveResult > 0)
                {
                    // Çalışana hizmetleri atama
                    if (employeeDTO.ServiceIds != null && employeeDTO.ServiceIds.Count > 0)
                    {
                        foreach (var serviceId in employeeDTO.ServiceIds)
                        {
                            var serviceResult = await AssignServiceToEmployeeAsync(employee.EmployeeId, serviceId);
                            Console.WriteLine($"Service assignment result for service {serviceId}: {serviceResult}");
                        }
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to save employee to database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateEmployeeAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            try
            {
                var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(employeeDTO.EmployeeId);
                if (existingEmployee == null)
                    return false;

                _mapper.Map(employeeDTO, existingEmployee);
                existingEmployee.UpdatedDate = DateTime.Now;

                _unitOfWork.Employees.Update(existingEmployee);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);
                if (employee == null)
                    return false;

                _unitOfWork.Employees.Remove(employee);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ActivateEmployeeAsync(int id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);
                if (employee == null)
                    return false;

                employee.IsActive = true;
                employee.UpdatedDate = DateTime.Now;

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeactivateEmployeeAsync(int id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);
                if (employee == null)
                    return false;

                employee.IsActive = false;
                employee.UpdatedDate = DateTime.Now;

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AssignServiceToEmployeeAsync(int employeeId, int serviceId)
        {
            try
            {
                // Repository'de özel bir metot kullanın
                var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
                if (employee == null)
                    return false;

                var service = await _unitOfWork.Services.GetByIdAsync(serviceId);
                if (service == null)
                    return false;

                // Zaten atanmış mı kontrol et
                var exists = await _unitOfWork.Employees.CheckEmployeeServiceExistsAsync(employeeId, serviceId);
                if (exists)
                    return true; // Zaten atanmış

                var employeeService = new EmployeeService
                {
                    EmployeeId = employeeId,
                    ServiceId = serviceId
                };

                await _unitOfWork.EmployeeServices.AddAsync(employeeService);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveServiceFromEmployeeAsync(int employeeId, int serviceId)
        {
            try
            {
                // Repository'de özel bir metot kullanın
                var employeeServices = await _unitOfWork.EmployeeServices.FindAsync(es => es.EmployeeId == employeeId && es.ServiceId == serviceId);
                var employeeService = employeeServices.FirstOrDefault();

                if (employeeService == null)
                    return false;

                _unitOfWork.EmployeeServices.Remove(employeeService);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}