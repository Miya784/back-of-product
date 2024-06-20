using System;
using System.ComponentModel.DataAnnotations;

namespace customer.Models
{
    public class RegisterCustomerRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class UserCustomer
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
    public class HistoryCustomer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual UserCustomer ? User { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public string DateTime { get; set;} = string.Empty;
    }
    public class BuyRequest
    {
        public int UserId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}