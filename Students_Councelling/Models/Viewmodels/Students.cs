using System.ComponentModel.DataAnnotations;

namespace Students_Councelling.Models.Viewmodels
{
    public class Students
    {
        [Key]
        public long StudentId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[@$!%*?&#]).{6,}$", ErrorMessage = "Password must be at least 6 characters long, contain at least one uppercase letter, and one special character (@, $, !, %, *, ?, &, #).")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Institute Name is required")]
        public string? InstituteName { get; set; }

        [Required(ErrorMessage = "Standard is required")]
        public string? Standard { get; set; }

        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
