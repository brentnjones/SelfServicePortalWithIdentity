using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SelfServicePortalWithIdentity.Data;
using SelfServicePortalWithIdentity.Models;

namespace SelfServicePortalWithIdentity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public UserProfile UserProfile { get; set; } = new();
        
        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile != null)
            {
                UserProfile = profile;
            }
            else
            {
                UserProfile = new UserProfile { UserId = user.Id };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                StatusMessage = "Error: Unable to load user.";
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Ensure UserId is set before validation
            UserProfile.UserId = user.Id;

            if (!ModelState.IsValid)
            {
                StatusMessage = "Validation errors: ";
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    StatusMessage += $"{error.ErrorMessage}; ";
                }
                return Page();
            }

            try
            {
                var existingProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                if (existingProfile != null)
                {
                    // Update existing profile
                    existingProfile.FirstName = UserProfile.FirstName;
                    existingProfile.LastName = UserProfile.LastName;
                    existingProfile.ManagerFirstName = UserProfile.ManagerFirstName;
                    existingProfile.ManagerLastName = UserProfile.ManagerLastName;
                    existingProfile.ManagerEmailAddress = UserProfile.ManagerEmailAddress;
                    existingProfile.OpenShiftProject = UserProfile.OpenShiftProject;
                    
                    _context.Update(existingProfile);
                }
                else
                {
                    // Create new profile - UserId already set above
                    _context.UserProfiles.Add(UserProfile);
                }

                var result = await _context.SaveChangesAsync();
                StatusMessage = "Your profile has been updated successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error saving profile: {ex.Message}";
            }
            
            return RedirectToPage();
        }
    }
}