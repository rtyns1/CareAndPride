using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Students
{
    [Authorize(Roles = "Admin,Secretary")]
    public class CreateModel : PageModel
    {
        private readonly ApiClient _api;
        public CreateModel(ApiClient api) { _api = api; }

        [BindProperty] public string FirstName { get; set; } = "";
        [BindProperty] public string LastName { get; set; } = "";
        [BindProperty] public DateTime? DateOfBirth { get; set; }
        [BindProperty] public Guid ClassId { get; set; }
        [BindProperty] public string? MedicalConditions { get; set; }

        public List<ClassDto> Classes { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); } catch { }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); } catch { }

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)
                || ClassId == Guid.Empty || !DateOfBirth.HasValue)
            {
                ErrorMessage = "All required fields must be filled.";
                return Page();
            }

            var dto = new CreateStudentDto
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                DateOfBirth = DateOfBirth.Value,
                ClassId = ClassId,
                MedicalConditions = MedicalConditions
            };

            try
            {
                await _api.PostAsync("api/students", dto);
                return RedirectToPage("/Students/Index");
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); return Page(); }
            catch (Exception ex) { ErrorMessage = ex.Message; return Page(); }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Unable to create student.";
            var idx = message.IndexOf("\"error\":\"");
            if (idx >= 0) { var s = idx + 9; var e = message.IndexOf('"', s); if (e > s) return message.Substring(s, e - s); }
            return message;
        }
    }
}
