using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobBoardFinalProject.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Email is required.")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [StringLength(25, ErrorMessage = "*Subject must be 25 characters or less.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Message is required.")]
        [UIHint("MultilineText")]
        public string Message { get; set; }
    }
}