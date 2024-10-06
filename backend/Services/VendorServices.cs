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

        public async Task<List<VendorDTO>> GetAllVendorsDTOAsync()
        {
            var vendorsResult = await _vendorReopository.GetAllVendorsAsync();

            return vendorsResult.Select(vendor => new VendorDTO
            {
                Id = vendor.Id,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorAddress = vendor.VendorAddress,
                VendorCity = vendor.VendorCity

            }).ToList();

        }

        // Get paticular Vendor

        public async Task<VendorDTO> GetVendorByIdDTOAsync(string Id)
        {
            var vendor = await _vendorReopository.GetVendorByIdAsync(Id);

            var vendorDTO = new VendorDTO
            {
                Id = vendor.Id,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorAddress = vendor.VendorAddress,
                VendorCity = vendor.VendorCity
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
                VendorCity = createVendorDTO.VendorCity
            };
            
            await _vendorReopository.CreateVendorAsync(vendor);

            // Then map the vendor model to vendor dto to pass it to vendor controller

            var vendorDTO = new VendorDTO
            {
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorAddress = vendor.VendorAddress,
                VendorCity = vendor.VendorCity
            };

            return vendorDTO;
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
                VendorCity = updateVendorDTO.VendorCity
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
                VendorCity = updatedVendor.VendorCity
            };
        }

        // Deleting a paticular Vendor

        public  Task DeleteVendorDTOAsync(string Id) => _vendorReopository.DeleteVendorAsync(Id);
        
    }
}