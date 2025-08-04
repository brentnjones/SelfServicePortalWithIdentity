using System.ComponentModel.DataAnnotations;

namespace SelfServicePortalWithIdentity.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        
        // Remove [Required] attribute - UserId is set in code, not from form
        public string UserId { get; set; } = string.Empty;
        
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Manager First Name")]
        public string? ManagerFirstName { get; set; }
        
        [Display(Name = "Manager Last Name")]
        public string? ManagerLastName { get; set; }

        [Display(Name = "Manager Email Address")]
        [EmailAddress]
        public string? ManagerEmailAddress { get; set; }

        [Display(Name = "OpenShift Project")]
        public string? OpenShiftProject { get; set; }
    }
}