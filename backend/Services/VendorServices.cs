using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services
{
    // Constructor dependency injection
    public class VendorService(IVendorRepository vendorRepository)
    {
        // _vendorReopository Can only be set in constructor and not changed afterward
        private readonly IVendorRepository _vendorReopository = vendorRepository;

        // Get All Vendors

        public async Task<IEnumerable<VendorDTO>> GetAllVendorsDTOAsync()
        {
            var vendorsResult = await _vendorReopository.GetAllVendorsAsync();

            var vendorDtos = new List<VendorDTO>();
            foreach(var vendor in vendorsResult)
            {
                vendorDtos.Add(new VendorDTO
                {
                    Id = vendor.Id,
                    VendorName = vendor.VendorName,
                    VendorEmail = vendor.VendorEmail,
                    VendorPhone = vendor.VendorPhone,
                    VendorAddress = vendor.VendorAddress,
                    VendorCity = vendor.VendorCity,
                    CustomerFeedback = vendor.CustomerFeedback,
                    Rating = vendor.Rating
                });
            }
            return vendorDtos;
        }

        // Get paticular Vendor

        public async Task<VendorDTO> GetVendorByIdDTOAsync(string Id)
        {
            var vendor = await _vendorReopository.GetVendorByIdAsync(Id);
            if(vendor == null) return null;

            var vendorDTO = new VendorDTO
            {
                Id = vendor.Id,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorAddress = vendor.VendorAddress,
                VendorCity = vendor.VendorCity,
                CustomerFeedback = vendor.CustomerFeedback,
                Rating = vendor.Rating
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
                CustomerFeedback = new List<string>(),
                Rating = new List<int>()
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
                CustomerFeedback = createdVendor.CustomerFeedback,
                Rating = createdVendor.Rating
            };

        }

        // Update existing Vendor

        public async Task<VendorDTO> UpdateVendorDTOAsync(UpdateVendorDTO updateVendorDTO)
        {
            if(string.IsNullOrEmpty(updateVendorDTO.Id)) throw new ArgumentException("Invalid Id");

            var vendor = new Vendor
            {
                Id = updateVendorDTO.Id,
                VendorName = updateVendorDTO.VendorName,
                VendorEmail = updateVendorDTO.VendorEmail,
                VendorPhone = updateVendorDTO.VendorPhone,
                VendorAddress = updateVendorDTO.VendorAddress,
                VendorCity = updateVendorDTO.VendorCity,
                CustomerFeedback = updateVendorDTO.CustomerFeedback,
                Rating = updateVendorDTO.Rating
            };

            var updatedVendor = await _vendorReopository.UpdateVendorAsync(vendor);

            // Map updated vendor to VendorDTO to response

            return new VendorDTO
            {   
                Id = updatedVendor.Id,
                VendorName = updatedVendor.VendorName,
                VendorEmail = updatedVendor.VendorEmail,
                VendorPhone = updatedVendor.VendorPhone,
                VendorAddress = updatedVendor.VendorAddress,
                VendorCity = updatedVendor.VendorCity,
                CustomerFeedback = updatedVendor.CustomerFeedback,
                Rating = updatedVendor.Rating
            };
        }

        // Deleting a paticular Vendor

        public  Task DeleteVendorDTOAsync(string id) => _vendorReopository.DeleteVendorAsync(id);
        
    }
}