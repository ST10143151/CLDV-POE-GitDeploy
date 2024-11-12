namespace ABCRetailers.Models
{
    public class Claim{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }  
        public string? Date { get; set; }

        public DateTime StartDate { get; set; } //start date for leave
        public DateTime EndDate { get; set; } //end date for leave

        public string? Reason { get; set;}

        public string? DocumentPath { get; set; }

        public string? Document { get; set; }
        
    }

}