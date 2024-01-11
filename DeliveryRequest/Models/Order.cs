using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeliveryRequest.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        public string? OutcomingCity { get; set; }
        [Required]
        [MinLength(1)]
        public string? OutcomingAddress { get; set; }
        [Required]
        [MinLength(1)]
        public string? IncomingCity { get; set; }
        [Required]
        [MinLength(1)]
        public string? IncomingAddress { get; set; }
        [Required]
        [Range(0.001, 100000)]
        public decimal Weight { get; set; }
        [Required]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PickupDate { get; set; }
    }
}
