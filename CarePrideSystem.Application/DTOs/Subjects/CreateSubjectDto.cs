namespace CarePrideSystem.Application.DTOs.Subjects
{
    public class CreateSubjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
