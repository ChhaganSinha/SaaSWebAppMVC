using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationMVC.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User")]
        public string UserName { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }

        public IReadOnlyCollection<string> AvailableUsers { get; set; } = new List<string>();
    }
}
