using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Students
{
    [Authorize(Roles = "Admin,Secretary")]
    public class EditModel : PageModel
    {
        private readonly ApiClient _api;
        public EditModel(ApiClient api) { _api = api; }

        [BindProperty] public Guid Id { get; set; }
        [BindProperty] public string FirstName { get; set; } = "";
        [BindProperty] public string LastName { get; set; } = "";
        [BindProperty] public DateTime? DateOfBirth { get; set; }
        [BindProperty] public string? MedicalConditions { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var student = await SafeGet(id);
            if (student == null) return RedirectToPage("/Students/Index");
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            DateOfBirth = student.DateOfBirth;
            MedicalConditions = student.MedicalConditions;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || !DateOfBirth.HasValue)
            {
                ErrorMessage = "All required fields must be filled.";
                return Page();
            }

            var dto = new UpdateStudentDto
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                DateOfBirth = DateOfBirth.Value,
                MedicalConditions = MedicalConditions
            };

            try
            {
                await _api.PutAsync($"api/students/{Id}", dto);
                return RedirectToPage("/Students/Details", new { id = Id });
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); return Page(); }
        }

        private async Task<StudentDto?> SafeGet(Guid id)
        {
            try { return await _api.GetAsync<StudentDto>($"api/students/{id}"); }
            catch { return null; }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Unable to update student.";
            var idx = message.IndexOf("\"error\":\"");
            if (idx >= 0) { var s = idx + 9; var e = message.IndexOf('"', s); if (e > s) return message.Substring(s, e - s); }
            return message;
        }
    }
}
