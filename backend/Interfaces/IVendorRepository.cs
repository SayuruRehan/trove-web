using backend.Models;

namespace backend.Interfaces
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();

        Task<Vendor> GetVendorByIdAsync(string id);

        Task<Vendor> CreateVendorAsync(Vendor vendor);

        Task<Vendor> UpdateVendorAsync(Vendor vendor);

        Task DeleteVendorAsync(string id);
    }
}