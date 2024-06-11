using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace simpleWebApp.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public ICollection<Product>? Products { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User ? User { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }
    public  class  UserCustomer
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public ICollection<ProductCustomer>? ProductCustomers { get; set; } = new List<ProductCustomer>();
    }
    public class ProductCustomer
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public DateTimeOffset Time { get; set; } 
        public ProductCustomer()
        {
            Time = DateTimeOffset.UtcNow;
        }
        public virtual UserCustomer ? UserCustomer { get; set; }
    }
}