using backend.Models;

namespace backend.Interfaces
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();

        Task<Vendor> GetVendorByIdAsync(string Id);

        Task<Vendor> CreateVendorAsync(Vendor vendor);

        Task<Vendor> UpdateVendorAsync(Vendor vendorDTO);

        Task DeleteVendorAsync(string Id);
    }
}