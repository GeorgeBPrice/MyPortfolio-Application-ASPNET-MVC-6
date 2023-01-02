using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PortfolioApp1.Models
{
    public class EmailModel
    {
        // required fields
        // limit user input (field types and character numbers)
        [Required]
        [StringLength(50)]
        public string? name { get; set; }

        [Required]
        [EmailAddress]
        public string? email { get; set; }

        [Phone]
        public string? phone { get; set; }

        [StringLength(100)]
        public string? subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string? message { get; set; }

        // Google reCaptcha reponse token
        [Required]
        public string Token { get; set; }
    }
}
