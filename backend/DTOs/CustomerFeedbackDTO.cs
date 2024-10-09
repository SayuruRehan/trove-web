// IT21470004 - BOPITIYA S. R. - CUSTOMER FEEDBACK DTO

using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CustomerFeedbackDTO
    {
        public string FeedbackId {get; set;}

        public string UserId { get; set; }  

        public string VendorId { get; set; } 

        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string CustomerFeedbackText {get; set;}

        public int Rating {get; set;}
    }

    public class CreateFeedbackDTO
    {
        public string UserId { get; set; }  

        public string VendorId { get; set; } 

        public string FirstName {get; set;}

        public string LastName {get; set;}

        [StringLength(100, ErrorMessage = "Feedback cannot exceed 100 characters.")]
        public string CustomerFeedbackText {get; set;}

        [Range(0, int.MaxValue, ErrorMessage = "Rating cannot be negative.")]
        public int Rating {get; set;}
    }

    public class UpdateFeedbackDTO
    {
        public string FeedbackId {get; set;}

        public string UserId { get; set; }  

        public string VendorId { get; set; } 

        public string FirstName {get; set;}

        public string LastName {get; set;}

        [StringLength(100, ErrorMessage = "Feedback cannot exceed 100 characters.")]
        public string CustomerFeedbackText {get; set;}

        [Range(0, int.MaxValue, ErrorMessage = "Rating cannot be negative.")]
        public int Rating {get; set;}
    }
}