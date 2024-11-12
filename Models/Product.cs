using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRetailers.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
    }

}
