using backend.DTOs;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class FeedbackService
    {
        private readonly IFeedBackRepository _feedbackRepository;

        public FeedbackService(IFeedBackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<IEnumerable<CustomerFeedbackDTO>> GetAllFeedbacksAsync()
        {
            var feedbacks =await _feedbackRepository.GetAllFeedbacksAsync();
            return feedbacks.Select(feedback => new CustomerFeedbackDTO
            {
                FeedbackId = feedback.FeedbackId,
                UserId = feedback.UserId,
                VendorId = feedback.VendorId,
                FirstName = feedback.FirstName,
                LastName = feedback.LastName,
                CustomerFeedbackText = feedback.CustomerFeedbackText,
                Rating = feedback.Rating
            });
        }

        public async Task<CustomerFeedbackDTO> GetFeedbackByIdAsync(string id)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            if (feedback == null)
                return null;

            return new CustomerFeedbackDTO
            {
                FeedbackId = feedback.FeedbackId,
                UserId = feedback.UserId,
                VendorId = feedback.VendorId,
                FirstName = feedback.FirstName,
                LastName = feedback.LastName,
                CustomerFeedbackText = feedback.CustomerFeedbackText,
                Rating = feedback.Rating
            };
        }


        public async Task<CustomerFeedback> AddFeedbackAsync(CreateFeedbackDTO createFeedbackDTO)
        {
            var feedback = new CustomerFeedback
            {
                UserId = createFeedbackDTO.UserId,
                VendorId =createFeedbackDTO.VendorId,
                FirstName = createFeedbackDTO.FirstName,
                LastName = createFeedbackDTO.LastName,
                CustomerFeedbackText = createFeedbackDTO.CustomerFeedbackText,
                Rating = createFeedbackDTO.Rating
            };

            return await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        // Update customer feedback

         public async Task<CustomerFeedbackDTO> UpdateFeedbackAsync(string id, UpdateFeedbackDTO updateFeedbackDTO)
         {
            if (updateFeedbackDTO == null)
                throw new ArgumentNullException(nameof(updateFeedbackDTO));

            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            if (feedback == null)
                throw new KeyNotFoundException($"Feddback with ID {updateFeedbackDTO.FeedbackId} not found.");

            feedback.FeedbackId = updateFeedbackDTO.FeedbackId;
            feedback.UserId = updateFeedbackDTO.UserId;
            feedback.VendorId = updateFeedbackDTO.VendorId;
            feedback.FirstName = updateFeedbackDTO.FirstName;
            feedback.LastName = updateFeedbackDTO.LastName;
            feedback.CustomerFeedbackText = updateFeedbackDTO.CustomerFeedbackText;
            feedback.Rating = updateFeedbackDTO.Rating;

            var updateFeedback = await _feedbackRepository.UpdateFeedbackAsync(id, feedback);

            // Return the updated order as a DTO
            return new CustomerFeedbackDTO
            {
                FeedbackId = updateFeedback.FeedbackId,
                UserId = updateFeedback.UserId,
                VendorId = updateFeedback.VendorId,
                FirstName = updateFeedback.FirstName,
                LastName = updateFeedback.LastName,
                CustomerFeedbackText = updateFeedback.CustomerFeedbackText,
                Rating = updateFeedback.Rating
            };
         }
    }
}