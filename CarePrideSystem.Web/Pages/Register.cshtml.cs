using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApiClient _api;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ApiClient api, ILogger<RegisterModel> logger)
        {
            _api = api;
            _logger = logger;
        }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string FullName { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email)
                || string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            if (Password.Length < 8)
            {
                ErrorMessage = "Password must be at least 8 characters.";
                return Page();
            }

            var dto = new RegisterDto
            {
                Username = Username.Trim(),
                Email = Email.Trim(),
                FullName = FullName.Trim(),
                Password = Password
            };

            try
            {
                await _api.PostAsync("api/auth/register", dto);
                SuccessMessage = "Your account has been created. An administrator will approve it shortly. You can sign in once approved.";
                Username = "";
                Email = "";
                FullName = "";
                Password = "";
                return Page();
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = ExtractError(ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register error");
                ErrorMessage = "Unable to create account. Please try again.";
                return Page();
            }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Unable to create account.";
            var idx = message.IndexOf("\"error\":\"");
            if (idx >= 0)
            {
                var start = idx + 9;
                var end = message.IndexOf('"', start);
                if (end > start) return message.Substring(start, end - start);
            }
            return message;
        }
    }
}
