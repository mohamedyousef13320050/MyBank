using BankSystem.Models;
using BankSystem.Repository;
using BankSystem.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace BankSystem.BL
{
    public class CustomerBL : ICustomerBL
    {
        private readonly ICustomerRepository customerRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CustomerBL(ICustomerRepository customerRepository, UserManager<ApplicationUser> userManager)
        {
            this.customerRepository = customerRepository;
            this.userManager = userManager;
        }

        public List<CustomerListVM> GetAllCustomers()
        {
            var customers = customerRepository.GetAll();

            return customers.Select(c => new CustomerListVM
            {
                Id = c.Id,
                FullName = c.FullName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Status = c.Status
            }).ToList();
        }

        public CustomerDetailsVM? GetCustomerById(int id)
        {
            var customer = customerRepository.GetById(id);

            if (customer == null)
                return null;

            return new CustomerDetailsVM
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.ApplicationUser?.Email ?? "",
                NationalId = customer.NationalId,
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = customer.CreatedAt,
                Status = customer.Status
            };
        }

        public async Task<(bool Success, List<string> Errors)> CreateCustomer(CustomerCreateVM model)
        {
            var errors = new List<string>();

            // Check if NationalId already exists
            if (customerRepository.GetByNationalId(model.NationalId) != null)
            {
                errors.Add("National ID is already registered.");
                return (false, errors);
            }

            // Create the ApplicationUser account
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            // Assign Customer role
            await userManager.AddToRoleAsync(user, "Customer");

            // Create Customer record
            var customer = new Customer
            {
                UserId = user.Id,
                FullName = model.FullName,
                NationalId = model.NationalId,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now,
                Status = true
            };

            customerRepository.Add(customer);
            customerRepository.Save();

            return (true, errors);
        }

        public async Task<(bool Success, List<string> Errors)> UpdateCustomer(CustomerEditVM model)
        {
            var errors = new List<string>();

            var customer = customerRepository.GetById(model.Id);

            if (customer == null)
            {
                errors.Add("Customer not found.");
                return (false, errors);
            }

            // Check NationalId uniqueness
            var existingWithNationalId = customerRepository.GetByNationalId(model.NationalId);
            if (existingWithNationalId != null && existingWithNationalId.Id != customer.Id)
            {
                errors.Add("National ID is already registered to another customer.");
                return (false, errors);
            }

            customer.FullName = model.FullName;
            customer.NationalId = model.NationalId;
            customer.DateOfBirth = model.DateOfBirth;
            customer.Address = model.Address;
            customer.PhoneNumber = model.PhoneNumber;

            // Update the ApplicationUser FullName as well
            var user = await userManager.FindByIdAsync(customer.UserId);
            if (user != null)
            {
                user.FullName = model.FullName;
                await userManager.UpdateAsync(user);
            }

            customerRepository.Update(customer);
            customerRepository.Save();

            return (true, errors);
        }

        public bool ActivateCustomer(int id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null) return false;

            customer.Status = true;
            customerRepository.Update(customer);
            customerRepository.Save();
            return true;
        }

        public bool DeactivateCustomer(int id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null) return false;

            customer.Status = false;
            customerRepository.Update(customer);
            customerRepository.Save();
            return true;
        }

        public CustomerProfileVM? GetMyProfile(string userId)
        {
            var customer = customerRepository.GetByUserId(userId);
            if (customer == null) return null;

            return new CustomerProfileVM
            {
                FullName = customer.FullName,
                Email = customer.ApplicationUser?.Email ?? "",
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber
            };
        }

        public async Task<(bool Success, List<string> Errors)> UpdateMyProfile(string userId, CustomerProfileVM model)
        {
            var errors = new List<string>();

            var customer = customerRepository.GetByUserId(userId);
            if (customer == null)
            {
                errors.Add("Customer profile not found.");
                return (false, errors);
            }

            customer.FullName = model.FullName;
            customer.DateOfBirth = model.DateOfBirth;
            customer.Address = model.Address;
            customer.PhoneNumber = model.PhoneNumber;

            // Update the ApplicationUser FullName
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.FullName = model.FullName;
                await userManager.UpdateAsync(user);
            }

            customerRepository.Update(customer);
            customerRepository.Save();

            return (true, errors);
        }
    }
}
