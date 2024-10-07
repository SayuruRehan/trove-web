using backend.Models;

namespace backend.Interfaces
{
    // Public interface for methods used in VendorRepository
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();

        Task<Vendor> GetVendorByIdAsync(string id);

        Task<Vendor> GetVendorByEmailAsync(string vendorEmail);

        Task<Vendor> CreateVendorAsync(Vendor vendor);

        // Task<(Vendor updatedVendor, bool wasInactive)> UpdateVendorAsync(string id, Vendor vendor);

        Task ActivateVendorAsync(string id);

        Task DeleteVendorAsync(string id);
        Task AddFeedbackToVendorAsync(string vendorId, CustomerFeedback feedback);


        Task<Vendor> VendorUpdateAsync(Vendor vendor);

    }
}