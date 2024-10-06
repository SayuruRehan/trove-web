using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{

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

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching Vendors", ex);
            }
        }

        // Get a particular Vendor
        public async Task<Vendor> GetVendorByIdAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Vendor Id not valid");

            try
            {
                var filteredResult = Builders<Vendor>.Filter.Eq(v => v.Id, id);

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
        public async Task<(Vendor updatedVendor, bool wasInactive)> UpdateVendorAsync(Vendor vendor)
        {
            if (vendor == null)
                throw new ArgumentException("Vendor not found");

            if (string.IsNullOrEmpty(vendor.Id))
                throw new ArgumentException("Invalid Vendor Id!");

            try
            {
                var existingVendor = await GetVendorByIdAsync(vendor.Id);

                // Check if IsActive state has changed to true
                bool wasInactive = existingVendor != null && !existingVendor.IsActive && vendor.IsActive;

                var filteredResult = Builders<Vendor>.Filter.Eq(v => v.Id, vendor.Id);
                var updatedVendor = await _vendor.FindOneAndReplaceAsync(filteredResult, vendor)
                    ?? throw new KeyNotFoundException("Vendor is not found ");

                return (updatedVendor, wasInactive);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured when updating a Vendor", ex);
            }
        }

        // Activate paticular Vendor

        public async Task ActivateVendorAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Invalid Vendor Id!");

            var filter = Builders<Vendor>.Filter.Eq(v => v.Id, id);
            var updateStatus = Builders<Vendor>.Update.Set(v => v.IsActive, true);

            try
            {
                var result = await _vendor.UpdateOneAsync(filter, updateStatus);

                if (result.ModifiedCount == 0)
                    throw new KeyNotFoundException("Vendor not found or already active");


            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured when updating status of a Vendor", ex);
            }
        }

        // Delete existing Vendor
        public async Task DeleteVendorAsync(string id)
        {

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

        public async Task AddFeedbackToVendorAsync(string vendorId, CustomerFeedback feedback)
        {
            if (string.IsNullOrEmpty(vendorId))
                throw new ArgumentException("Invalid Vendor Id!");

            if (feedback == null)
                throw new ArgumentNullException(nameof(feedback));

            var filter = Builders<Vendor>.Filter.Eq(v => v.Id, vendorId);
            var update = Builders<Vendor>.Update.Push(v => v.Feedbacks, feedback);

            try
            {
                var result = await _vendor.UpdateOneAsync(filter, update);

                if (result.ModifiedCount == 0)
                    throw new KeyNotFoundException("Vendor not found or feedback not added");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while adding feedback to vendor", ex);
            }
        }

    }
}