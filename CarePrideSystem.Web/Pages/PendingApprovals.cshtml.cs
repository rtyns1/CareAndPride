using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class PendingApprovalsModel : PageModel
    {
        private readonly ApiClient _api;
        public PendingApprovalsModel(ApiClient api) { _api = api; }

        public List<UserDto> Users { get; set; } = new();
        public string? Error { get; set; }
        public string? Success { get; set; }

        public async Task OnGetAsync() => await LoadAsync();

        private async Task LoadAsync()
        {
            try { Users = await _api.GetAsync<List<UserDto>>("api/users/pending") ?? new(); }
            catch (Exception ex) { Error = ex.Message; }
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid id)
        {
            try { await _api.PostNoBodyAsync($"api/users/{id}/approve"); Success = "Approved."; }
            catch (Exception ex) { Error = ex.Message; }
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostRejectAsync(Guid id)
        {
            try { await _api.PostNoBodyAsync($"api/users/{id}/reject"); Success = "Rejected."; }
            catch (Exception ex) { Error = ex.Message; }
            await LoadAsync();
            return Page();
        }
    }
}
