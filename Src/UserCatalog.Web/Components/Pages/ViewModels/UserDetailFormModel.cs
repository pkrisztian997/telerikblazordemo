using System.ComponentModel.DataAnnotations;

namespace UserCatalog.Web.Components.Pages.ViewModels
{
    public class UserDetailFormModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string BirthPlace { get; set; } = string.Empty;

        [Required]
        public string Residence { get; set; } = string.Empty;

        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please repeat the new password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The new passwords do not match")]
        public string NewPasswordAgain { get; set; } = string.Empty;
    }
}
