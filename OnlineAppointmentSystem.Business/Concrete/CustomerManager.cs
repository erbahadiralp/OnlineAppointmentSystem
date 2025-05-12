using AutoMapper;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllWithUserAsync();
            return _mapper.Map<List<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetCustomerWithUserByIdAsync(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<CustomerDTO> GetCustomerByUserIdAsync(string userId)
        {
            var customer = await _unitOfWork.Customers.GetCustomerByUserIdAsync(userId);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<bool> CreateCustomerAsync(CustomerDTO customerDTO)
        {
            try
            {
                // Kullanıcı zaten var mı kontrol et
                var employee = await _unitOfWork.Employees.GetEmployeeByUserIdAsync(customerDTO.UserId);
                if (employee != null)
                    return false;

                var customer = _mapper.Map<Customer>(customerDTO);
                customer.CreatedDate = DateTime.Now;

                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCustomerAsync(CustomerDTO customerDTO)
        {
            try
            {
                var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customerDTO.CustomerId);
                if (existingCustomer == null)
                    return false;

                _mapper.Map(customerDTO, existingCustomer);
                existingCustomer.UpdatedDate = DateTime.Now;

                _unitOfWork.Customers.Update(existingCustomer);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                    return false;

                _unitOfWork.Customers.Remove(customer);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ActivateCustomerAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                    return false;

                var user = await _unitOfWork.Users.GetByIdAsync(customer.UserId);
                if (user == null)
                    return false;

                user.IsActive = true;
                customer.IsActive = true;
                _unitOfWork.Users.Update(user);
                _unitOfWork.Customers.Update(customer);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeactivateCustomerAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                    return false;

                var user = await _unitOfWork.Users.GetByIdAsync(customer.UserId);
                if (user == null)
                    return false;

                user.IsActive = false;
                customer.IsActive = false;
                _unitOfWork.Users.Update(user);
                _unitOfWork.Customers.Update(customer);
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