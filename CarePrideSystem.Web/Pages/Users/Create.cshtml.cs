using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApiClient _api;
        public CreateModel(ApiClient api) { _api = api; }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string FullName { get; set; } = "";
        [BindProperty] public UserRole Role { get; set; } = UserRole.Teacher;
        [BindProperty] public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

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

            var dto = new CreateUserDto
            {
                Username = Username.Trim(),
                Email = Email.Trim(),
                FullName = FullName.Trim(),
                Role = Role,
                Password = Password
            };

            try
            {
                await _api.PostAsync("api/users", dto);
                return RedirectToPage("/Users/Index");
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = ExtractError(ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Unable to create user.";
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
