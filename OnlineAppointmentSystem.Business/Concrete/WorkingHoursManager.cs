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
    public class WorkingHoursManager : IWorkingHoursService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkingHoursManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<WorkingHoursDTO>> GetAllWorkingHoursAsync()
        {
            var workingHours = await _unitOfWork.WorkingHours.GetAllAsync(); 
            return _mapper.Map<List<WorkingHoursDTO>>(workingHours);
        }

        public async Task<WorkingHoursDTO> GetWorkingHoursByIdAsync(int id)
        {
            var workingHours = await _unitOfWork.WorkingHours.GetByIdAsync(id);
            return _mapper.Map<WorkingHoursDTO>(workingHours);
        }

        public async Task<List<WorkingHoursDTO>> GetWorkingHoursByEmployeeIdAsync(int employeeId)
        {
            var workingHours = await _unitOfWork.WorkingHours.GetWorkingHoursByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<WorkingHoursDTO>>(workingHours);
        }

        public async Task<List<WorkingHoursDTO>> GetWorkingHoursByDayOfWeekAsync(int dayOfWeek)
        {
            var workingHours = await _unitOfWork.WorkingHours.GetWorkingHoursByDayOfWeekAsync(dayOfWeek);
            return _mapper.Map<List<WorkingHoursDTO>>(workingHours);
        }

        public async Task<bool> CreateWorkingHoursAsync(WorkingHoursDTO workingHoursDTO)
        {
            try
            {
                Console.WriteLine($"Creating working hours for EmployeeId: {workingHoursDTO.EmployeeId}, DayOfWeek: {workingHoursDTO.DayOfWeek}");
                Console.WriteLine($"StartTime: {workingHoursDTO.StartTime}, EndTime: {workingHoursDTO.EndTime}");

                var workingHours = _mapper.Map<WorkingHours>(workingHoursDTO);
                workingHours.IsActive = true;

                Console.WriteLine("Mapped to WorkingHours entity successfully");

                await _unitOfWork.WorkingHours.AddAsync(workingHours);
                Console.WriteLine("Added to repository");

                var result = await _unitOfWork.CompleteAsync();
                Console.WriteLine($"UnitOfWork.CompleteAsync result: {result}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateWorkingHoursAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
            {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                return false;
            }
        }

        public async Task<bool> UpdateWorkingHoursAsync(WorkingHoursDTO workingHoursDTO)
        {
            try
            {
                var existingWorkingHours = await _unitOfWork.WorkingHours.GetByIdAsync(workingHoursDTO.WorkingHoursId);
                if (existingWorkingHours == null)
                    return false;

                _mapper.Map(workingHoursDTO, existingWorkingHours);

                _unitOfWork.WorkingHours.Update(existingWorkingHours);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteWorkingHoursAsync(int id)
        {
            try
            {
                var workingHours = await _unitOfWork.WorkingHours.GetByIdAsync(id);
                if (workingHours == null)
                    return false;

                _unitOfWork.WorkingHours.Remove(workingHours);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsEmployeeAvailableAtTimeAsync(int employeeId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                var workingHours = await _unitOfWork.WorkingHours.GetWorkingHoursByEmployeeIdAsync(employeeId);
                var dayWorkingHours = workingHours.FirstOrDefault(wh => wh.DayOfWeek == dayOfWeek && wh.IsActive);

                if (dayWorkingHours == null)
                    return false;

                return startTime >= dayWorkingHours.StartTime && endTime <= dayWorkingHours.EndTime;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}