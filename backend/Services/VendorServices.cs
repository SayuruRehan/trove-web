using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using backend.Utilities;

namespace backend.Services
{
    // Constructor dependency injection
    public class VendorService
    {
        // _vendorReopository Can only be set in constructor and not changed afterward
        private readonly IVendorRepository _vendorReopository;
        private readonly EmailService _emailService;

        public VendorService(IVendorRepository vendorRepository, EmailService emailService)
        {
            _vendorReopository = vendorRepository;
            _emailService = emailService;
        }

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
                HashedPassword = PasswordHelper.HashPassword(createVendorDTO.HashedPassword),
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

        // Login for Vendor
        public async Task<VendorDTO> LoginAsync(VendorLoginDTO vendorLoginDTO)
        {
            var vendor = await _vendorReopository.GetVendorByEmailAsync(vendorLoginDTO.VendorEmail);

            if (vendor == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Check for null or empty password
            if (string.IsNullOrEmpty(vendorLoginDTO.HashedPassword))
            {
                throw new UnauthorizedAccessException("Password cannot be empty.>>> input");
            }

            // Check if stored hashed password is null
            if (string.IsNullOrEmpty(vendor.HashedPassword))
            {
                throw new UnauthorizedAccessException("No password found for this vendor. >>> stored");
            }

            if (vendor == null || !PasswordHelper.VerifyPassword(vendorLoginDTO.HashedPassword, vendor.HashedPassword))
            {
                throw new UnauthorizedAccessException("Invalid email or pasword!");
            }

            return new VendorDTO
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

        }

        // Update existing Vendor
        // public async Task<VendorDTO> UpdateVendorDTOAsync(string id, UpdateVendorDTO updateVendorDTO)
        // {

        //     var vendor = new Vendor
        //     {
        //         VendorName = updateVendorDTO.VendorName,
        //         VendorEmail = updateVendorDTO.VendorEmail,
        //         VendorPhone = updateVendorDTO.VendorPhone,
        //         VendorAddress = updateVendorDTO.VendorAddress,
        //         VendorCity = updateVendorDTO.VendorCity,
        //         IsActive = updateVendorDTO.IsActive,

        //     };

        //     var (updatedVendor, wasInactive) = await _vendorReopository.UpdateVendorAsync(id, vendor);

        //     // Send email if Vendor is activated
        //     if (wasInactive)
        //     {
        //         var emailDto = new EmailDTO
        //         {
        //             ToEmail = updatedVendor.VendorEmail,
        //             Subject = "Your Account Has Been Activated",
        //             Body = $@"
        //                 <h1>Hello {updatedVendor.VendorName},</h1>
        //                 <h3>Your account has been activated.</h3>
        //                 <p><b>Username:</b> {updatedVendor.VendorEmail}<br>
        //                 <p>You can log in using the following link:</p>
        //                 <p><a href='http://localhost:5173/vendor/login'>Login to your account</a></p>
        //                 <p>Best regards,<br>E-com</p>"
        //         };

        //         try
        //         {
        //             await _emailService.SendEmailAsync(emailDto);
        //         }
        //         catch (Exception ex)
        //         {
        //             Console.WriteLine($"Failed to send email to {updatedVendor.VendorEmail}. Error: {ex.Message}");

        //             throw new Exception("Vendor details updated but failed to send activation email. Please contact support.");
        //         }
        //     }

        //     // Map updated vendor to VendorDTO to response

        //     return new VendorDTO
        //     {
        //         Id = updatedVendor.Id,
        //         VendorName = updatedVendor.VendorName,
        //         VendorEmail = updatedVendor.VendorEmail,
        //         VendorPhone = updatedVendor.VendorPhone,
        //         VendorAddress = updatedVendor.VendorAddress,
        //         VendorCity = updatedVendor.VendorCity,
        //         IsActive = updatedVendor.IsActive,
        //     };
        // }

        // Deleting a paticular Vendor

        public async Task<VendorDTO> UpdateVendorAsync(string id, UpdateVendorDTO updateVendorDTO)
        {
            // Check if the product exists
            var existingVendor = await _vendorReopository.GetVendorByIdAsync(id);
            if (existingVendor == null)
            {
                throw new KeyNotFoundException("Vendor not found.");
            }

            var vendor = new Vendor
            {
                Id = existingVendor.Id,
                VendorName = updateVendorDTO.VendorName ?? existingVendor.VendorName,
                VendorEmail = updateVendorDTO.VendorEmail ?? existingVendor.VendorEmail,
                VendorPhone = updateVendorDTO.VendorPhone ?? existingVendor.VendorPhone,
                VendorAddress = updateVendorDTO.VendorAddress ?? existingVendor.VendorAddress,
                VendorCity = updateVendorDTO.VendorCity ?? existingVendor.VendorCity,
            };

            // Step 4: Update the product in the repository
            var updatedProduct = await _vendorReopository.VendorUpdateAsync(vendor);

            // Step 5: Return the updated product DTO
            return new VendorDTO
            {
                Id = updatedProduct.Id,
                VendorName = updatedProduct.VendorName,
                VendorEmail = updatedProduct.VendorEmail,
                VendorAddress = updatedProduct.VendorAddress,
                VendorPhone = updatedProduct.VendorPhone,
                VendorCity = updatedProduct.VendorCity
            };
        }

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