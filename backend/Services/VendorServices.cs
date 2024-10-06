using backend.DTOs;
using backend.Interfaces;
using backend.Models;

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
            var updatedVendor = await _vendorReopository.GetVendorByIdAsync(updateVendorDTO.Id);

            updatedVendor.VendorName = updateVendorDTO.VendorName;
            updatedVendor.VendorEmail = updateVendorDTO.VendorEmail;
            updatedVendor.VendorPhone = updateVendorDTO.VendorPhone;
            updatedVendor.VendorAddress = updateVendorDTO.VendorAddress;
            updatedVendor.VendorCity = updateVendorDTO.VendorCity;

            await _vendorReopository.UpdateVendorAsync(updatedVendor);

            // Map updated vendor to VendorDTO to response

            var vendorDTO = new VendorDTO
            {
                Id = updatedVendor.Id,
                VendorName = updatedVendor.VendorName,
                VendorEmail = updatedVendor.VendorEmail,
                VendorPhone = updatedVendor.VendorPhone,
                VendorAddress = updatedVendor.VendorAddress,
                VendorCity = updatedVendor.VendorCity
            };

            return vendorDTO;
        }

        // Deleting a paticular Vendor

        public  Task DeleteVendorDTOAsync(string Id) => _vendorReopository.DeleteVendorAsync(Id);
        
    }
}