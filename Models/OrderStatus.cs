using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;


namespace ABCRetailers.Models
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatus_Id { get; set; }

        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        
        [Required(ErrorMessage = "Please select a birder.")]
        public int Customer_ID { get; set; } // FK to the Birder who made the sighting

        [Required(ErrorMessage = "Please select a bird.")]
        public int Product_ID { get; set; } // FK to the Bird being sighted

        [Required(ErrorMessage = "Please select the date.")]
        public DateTime OrderStatus_Date { get; set; } 

        [Required(ErrorMessage = "Please enter the location.")]
        public string? OrderStatus_Location { get; set; } 
    }
}
