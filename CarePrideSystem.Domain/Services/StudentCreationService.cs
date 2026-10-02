using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class StudentCreationService
    {
        public static Student CreateStudent(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string admissionNumber,
            Guid classId,
            string? medicalConditions = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            if (dateOfBirth > DateTime.UtcNow)
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

            if (dateOfBirth < DateTime.UtcNow.AddYears(-120))
                throw new ArgumentException("Date of birth is too old.", nameof(dateOfBirth));

            if (string.IsNullOrWhiteSpace(admissionNumber))
                throw new ArgumentException("Admission number is required.", nameof(admissionNumber));

            if (classId == Guid.Empty)
                throw new ArgumentException("Class ID is required.", nameof(classId));

            return new Student(
                id: Guid.NewGuid(),
                firstName: firstName,
                lastName: lastName,
                dateOfBirth: dateOfBirth,
                admissionNumber: admissionNumber,
                classId: classId,
                medicalConditions: medicalConditions,
                isArchived: false,
                createdAtUtc: DateTime.UtcNow,
                archivedAtUtc: null
            );
        }
    }
}