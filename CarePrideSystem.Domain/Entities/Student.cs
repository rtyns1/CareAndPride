using CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string AdmissionNumber { get; private set; }
        public Guid ClassId { get; private set; }
        public string? MedicalConditions { get; private set; }
        public bool IsArchived { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? ArchivedAtUtc { get; private set; }

        private Student() { }

        internal Student(
            Guid id,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string admissionNumber,
            Guid classId,
            string? medicalConditions,
            bool isArchived,
            DateTime createdAtUtc,
            DateTime? archivedAtUtc = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            AdmissionNumber = admissionNumber;
            ClassId = classId;
            MedicalConditions = medicalConditions;
            IsArchived = isArchived;
            CreatedAtUtc = createdAtUtc;
            ArchivedAtUtc = archivedAtUtc;
        }

        public void UpdateDetails(string firstName, string lastName, DateTime dateOfBirth, string? medicalConditions)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            if (dateOfBirth > DateTime.UtcNow)
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            MedicalConditions = medicalConditions;
        }

        public void TransferToClass(Guid newClassId)
        {
            if (newClassId == Guid.Empty)
                throw new ArgumentException("Class ID is required.", nameof(newClassId));

            if (IsArchived)
                throw new InvalidOperationException("Cannot transfer an archived student.");

            ClassId = newClassId;
        }

        public void Archive()
        {
            if (IsArchived)
                throw new InvalidOperationException("Student is already archived.");

            IsArchived = true;
            ArchivedAtUtc = DateTime.UtcNow;
        }

        public void Unarchive()
        {
            if (!IsArchived)
                throw new InvalidOperationException("Student is not archived.");

            IsArchived = false;
            ArchivedAtUtc = null;
        }
    }
}