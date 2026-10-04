using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly ApiClient _api;
        public ChangePasswordModel(ApiClient api) { _api = api; }

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "New password and confirmation do not match.";
                return Page();
            }

            if (NewPassword.Length < 8)
            {
                ErrorMessage = "New password must be at least 8 characters.";
                return Page();
            }

            try
            {
                var body = new ChangePasswordDto
                {
                    CurrentPassword = CurrentPassword,
                    NewPassword = NewPassword
                };
                await _api.PostAsync("api/auth/change-password", body);
                SuccessMessage = "Password updated successfully.";
                return Page();
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = ExtractError(ex.Message);
                return Page();
            }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Unable to change password.";
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
