using System.Security.Claims;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApiClient _api;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ApiClient api, ILogger<LoginModel> logger)
        {
            _api = api;
            _logger = logger;
        }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required.";
                return Page();
            }

            try
            {
                var result = await _api.LoginAsync(Username.Trim(), Password);
                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    ErrorMessage = "Invalid username or password.";
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.Username ?? Username),
                    new Claim(ClaimTypes.Role, result.Role ?? "Teacher"),
                    new Claim("access_token", result.Token),
                    new Claim("full_name", result.FullName ?? ""),
                    new Claim("user_id", result.UserId.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToPage("/Index");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Login API unreachable");
                ErrorMessage = "Cannot reach the server. Make sure the API is running on port 5182.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected login error");
                ErrorMessage = "Login failed. Please try again.";
                return Page();
            }
        }
    }
}
