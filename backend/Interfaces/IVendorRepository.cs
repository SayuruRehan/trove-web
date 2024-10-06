using backend.Models;

namespace backend.Interfaces
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();

        Task<Vendor> GetVendorByIdAsync(string id);

        Task<Vendor> CreateVendorAsync(Vendor vendor);

        Task<(Vendor updatedVendor, bool wasInactive)> UpdateVendorAsync(Vendor vendor);

        Task ActivateVendorAsync(string id);

        Task DeleteVendorAsync(string id);
        Task AddFeedbackToVendorAsync(string vendorId, CustomerFeedback feedback);

    }
}