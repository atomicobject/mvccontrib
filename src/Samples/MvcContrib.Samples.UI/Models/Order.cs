using System;
using System.ComponentModel.DataAnnotations;

namespace MvcContrib.Samples.UI.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Website { get; set; }
        [RegularExpression(@"\(\d\d\d\) \d\d\d\-\d\d\d\d")]
        public string Phone { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        public string Address { get; set; }
        [Range(1, 10)]
        public int Quantity { get; set; }
    }
}