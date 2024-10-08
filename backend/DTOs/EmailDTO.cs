// IT21470004 - BOPITIYA S. R. - EmailDTO class for email management

namespace backend.DTOs
{
    public class EmailDTO
    {
        public required string ToEmail { get; set; }

        public required string Subject { get; set; }

        public required string Body { get; set; }
    }
}