using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories{

    public class VendorRepository : IVendorRepository
    {
        private readonly IMongoCollection<Vendor> _vendor;

        public VendorRepository(IMongoDatabase mongoDatabase)
        {
            _vendor = mongoDatabase.GetCollection<Vendor>("Vendors");
        }

        // Get the All Vendors
        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            try
            {
                // Convert query result into list of Vendor objects asynchronusly
                return await _vendor.Find(FilterDefinition<Vendor>.Empty).ToListAsync();

            } catch (Exception ex)
            {
                throw new ApplicationException ("Error fetching Vendors", ex);
            }
        }

        // Get a particular Vendor
        public async Task<Vendor> GetVendorByIdAsync(string id)
        {

            if(string.IsNullOrEmpty(id))
                throw new ArgumentException("Vendor Id not valid");

            try
            {
                var filteredResult = Builders<Vendor>.Filter.Eq( v => v.Id, id);

                // Retrives the first document that matches the filter
                return await _vendor.Find(filteredResult).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching Vendor with this particular Id", ex);
            }
            
        }

        // Add a new Vendor
        public async Task<Vendor> CreateVendorAsync(Vendor vendor)
        {

            ArgumentNullException.ThrowIfNull(vendor);

            try
            {
                // If vendor id null, mongodb generate that id
                await _vendor.InsertOneAsync(vendor);
                return vendor;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured when creating new Vendor", ex);
            }
        }

        // Update existing Vendor
        public async Task<Vendor> UpdateVendorAsync(Vendor vendor)
        {
            if(vendor == null)
                throw new ArgumentException("Vendor not found");          

            if(string.IsNullOrEmpty(vendor.Id))
                throw new ArgumentException("Invalid Vendor Id!");
            
            try
            {
                var filteredResult = Builders<Vendor>.Filter.Eq(v => v.Id, vendor.Id);
                var result = await _vendor.FindOneAndReplaceAsync(filteredResult, vendor)
                    ?? throw new KeyNotFoundException("Product is not found ");

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured when updating a Vendor", ex);
            }
        }

        // Delete existing Vendor
        public async Task DeleteVendorAsync(string id){

            ArgumentException.ThrowIfNullOrEmpty(id);

            var filteredResult = Builders<Vendor>.Filter.Eq(v => v.Id, id);

            try
            {
                var deleteResult = await _vendor.DeleteOneAsync(filteredResult);

                if (deleteResult.DeletedCount == 0)
                    throw new KeyNotFoundException("No Vendor deleted with the particular Id");
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting paticular vendor", ex);
            }

        }

    }
}