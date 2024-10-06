using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using backend.Utilities;

namespace backend.Services
{
    // Constructor dependency injection
    public class VendorService(IVendorRepository vendorRepository, EmailService emailService)
    {
        // _vendorReopository Can only be set in constructor and not changed afterward
        private readonly IVendorRepository _vendorReopository = vendorRepository;
        private readonly EmailService _emailService = emailService;

        // Get All Vendors

        public async Task<IEnumerable<VendorDTO>> GetAllVendorsDTOAsync()
        {
            var vendorsResult = await _vendorReopository.GetAllVendorsAsync();

            var vendorDtos = new List<VendorDTO>();
            foreach (var vendor in vendorsResult)
            {
                vendorDtos.Add(new VendorDTO
                {
                    Id = vendor.Id,
                    VendorName = vendor.VendorName,
                    VendorEmail = vendor.VendorEmail,
                    VendorPhone = vendor.VendorPhone,
                    VendorAddress = vendor.VendorAddress,
                    VendorCity = vendor.VendorCity,
                    IsActive = vendor.IsActive,
                    Products = vendor.Products,
                    Feedbacks = vendor.Feedbacks
                });
            }
            return vendorDtos;
        }

        // Get paticular Vendor

        public async Task<VendorDTO> GetVendorByIdDTOAsync(string Id)
        {
            var vendor = await _vendorReopository.GetVendorByIdAsync(Id);
            if (vendor == null) return null;

            var vendorDTO = new VendorDTO
            {
                Id = vendor.Id,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorAddress = vendor.VendorAddress,
                VendorCity = vendor.VendorCity,
                IsActive = vendor.IsActive,
                Products = vendor.Products,
                Feedbacks = vendor.Feedbacks
            };

            return vendorDTO;
        }

        // Create new Vendor

        public async Task<VendorDTO> CreateVendorDTOAsync(CreateVendorDTO createVendorDTO)
        {
            // This needed because repository layer works with actual Vendor Model

            var vendor = new Vendor
            {
                VendorName = createVendorDTO.VendorName,
                VendorEmail = createVendorDTO.VendorEmail,
                VendorPhone = createVendorDTO.VendorPhone,
                VendorAddress = createVendorDTO.VendorAddress,
                VendorCity = createVendorDTO.VendorCity,
                IsActive = createVendorDTO.IsActive,
                Products = new List<string>(),
                Feedbacks = new List<CustomerFeedback>()
            };

            var createdVendor = await _vendorReopository.CreateVendorAsync(vendor);

            // Then map the vendor model to vendor dto to pass it to vendor controller

            return new VendorDTO
            {
                Id = createdVendor.Id,
                VendorName = createdVendor.VendorName,
                VendorEmail = createdVendor.VendorEmail,
                VendorPhone = createdVendor.VendorPhone,
                VendorAddress = createdVendor.VendorAddress,
                VendorCity = createdVendor.VendorCity,
                IsActive = createdVendor.IsActive,
                Products = createdVendor.Products,
                Feedbacks = createdVendor.Feedbacks,
            };

        }

        // Update existing Vendor

        public async Task<VendorDTO> UpdateVendorDTOAsync(UpdateVendorDTO updateVendorDTO)
        {
            if (string.IsNullOrEmpty(updateVendorDTO.Id)) throw new ArgumentException("Invalid Id");

            var vendor = new Vendor
            {
                Id = updateVendorDTO.Id,
                VendorName = updateVendorDTO.VendorName,
                VendorEmail = updateVendorDTO.VendorEmail,
                VendorPhone = updateVendorDTO.VendorPhone,
                VendorAddress = updateVendorDTO.VendorAddress,
                VendorCity = updateVendorDTO.VendorCity,
                IsActive = updateVendorDTO.IsActive,
                Products = updateVendorDTO.Products,
                Feedbacks = updateVendorDTO.Feedbacks,

            };

            var (updatedVendor, wasInactive) = await _vendorReopository.UpdateVendorAsync(vendor);

            // Send email if Vendor is activated
            // if(!updateVendorDTO.IsActive && updatedVendor.IsActive)
            if (wasInactive)
            {
                string newPassword = PasswordGenerator.GenerateRandomPassword();
                vendor.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                var emailDto = new EmailDTO
                {
                    ToEmail = updatedVendor.VendorEmail,
                    Subject = "Your Account Has Been Activated",
                    Body = $@"
                        <h1>Hello {updatedVendor.VendorName},</h1>
                        <h3>Your account has been activated.</h3>
                        <p><b>Username:</b> {updatedVendor.VendorEmail}<br>
                        <b>Password:</b> {newPassword}</p>
                        <p>You can log in using the following link:</p>
                        <p><a href='http://localhost:5173/login'>Login to your account</a></p>
                        <p>Best regards,<br>E-com</p>"
                };

                try
                {
                    await _emailService.SendEmailAsync(emailDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {updatedVendor.VendorEmail}. Error: {ex.Message}");

                    throw new Exception("Vendor details updated but failed to send activation email. Please contact support.");
                }
            }

            // Map updated vendor to VendorDTO to response

            return new VendorDTO
            {
                Id = updatedVendor.Id,
                VendorName = updatedVendor.VendorName,
                VendorEmail = updatedVendor.VendorEmail,
                VendorPhone = updatedVendor.VendorPhone,
                VendorAddress = updatedVendor.VendorAddress,
                VendorCity = updatedVendor.VendorCity,
                IsActive = updatedVendor.IsActive,
                Products = updatedVendor.Products,
                Feedbacks = updatedVendor.Feedbacks,
            };
        }

        // Deleting a paticular Vendor

        public Task DeleteVendorDTOAsync(string id) => _vendorReopository.DeleteVendorAsync(id);
        public async Task AddFeedbackToVendorAsync(string vendorId, CustomerFeedback feedback)
        {
            if (string.IsNullOrEmpty(vendorId))
                throw new ArgumentException("Invalid Vendor Id!");

            if (feedback == null)
                throw new ArgumentNullException(nameof(feedback));

            await _vendorReopository.AddFeedbackToVendorAsync(vendorId, feedback);
        }


    }
}