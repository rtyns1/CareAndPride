**7th September 2026**

- Today the focus is fully on documenting extensively on the Domain laye that i have just written.
- Now, When writing domain it made sense to approach it with a vertical building system in mind-- this means i wroe the whole layer at once. This is because of dpendency inversion and onion architecture, it jsut made sense for me to do this because it is the innermost layer. SO at the very least it has to be complete.
- Now a huge part of this project is the documantation. I am really commited to making sure i am accountable to every line of code and understand the system in and out completely.
- The domain is the heart of the system-- everything else exists to serve it. That s how the system has ben desined.
- We have to remmeber that the business rules determine how everything esle works. For example stuff lie how the students are enrolled etc etc even how grades are calcualated.
- # 2. The Domain Layer Folder Structure

```
CarePrideSystem.Domain/
├── Entities/
│   ├── User.cs
│   ├── Student.cs
│   ├── Class.cs
│   ├── Subject.cs
│   ├── TeacherSubject.cs
│   └── ClassTeacher.cs
├── Enums/
│   └── UserRole.cs
├── Services/
│   ├── UserCreationService.cs
│   ├── StudentCreationService.cs
│   ├── ClassCreationService.cs
│   ├── SubjectCreationService.cs
│   ├── TeacherSubjectCreationService.cs
│   └── ClassTeacherCreationService.cs
├── Interfaces/
│   └── Repositories/
│       ├── IUserRepository.cs
│       ├── IStudentRepository.cs
│       ├── IClassRepository.cs
│       ├── ISubjectRepository.cs
│       ├── ITeacherSubjectRepository.cs
│       └── IClassTeacherRepository.cs
└── Exceptions/
    ├── DomainException.cs
    ├── UserAlreadyApprovedException.cs
    ├── UserNotFoundException.cs
    ├── StudentNotFoundException.cs
    ├── StudentAlreadyEnrolledException.cs
    ├── ClassCapacityExceededException.cs
    ├── ClassNotFoundException.cs
    ├── SubjectNotFoundException.cs
    ├── TeacherNotAssignedToSubjectException.cs
    └── TeacherNotAssignedToClassException.cs
```

- This time round domain is more than just data containers lol. im used to making domain the data container layer. 
- Starting off with Enttities;:::

# 1.1 ENTITIES
- These are the core of the Domain.
- Stuff like user, class etc - represnent real world things in the school -- data containers that represent real world things in the school.
- These have patterns that they follow:::(ggood thing i alrady wrot ethe code so its easier to document) 
```csharp
public class EntityName
{
    // 1. Properties (private setters)
    // 2. Private constructor (for EF Core)
    // 3. Internal constructor (for Domain Services)
    // 4. State-changing methods (business logic)
}
```

Now, obviously in tyical Karel fashion these patterns have to be explained indepth.
Now, this will take a long damn time because the idea is to explain each and every file.
- But where we are now its still general stuff yeah? Yeah
- So, above we have the Entity pattern. The entire entity system follows that pattern.
-  ### **Why private setters?**
- --? Encapsulation. Private setters ensure that the entity controls its own state. You cannot accidentally change 'isAproved' from fals to true without going through the Approve() method.
- Protects the integrity of the enrity. The entity is the ony place where state can be modified.
-  ### **Why a private constructor?** 
 - EF core needs a paramterless constructor to hydrate objects from the database. By making it provate we force external code to always useDomain service to create new instances.
 - This means tht the business rules are always enforsed during creation
 - EF core works by readin data from the databsse and converting it into C# objects.
 - The Process:

EF Core executes a SQL query: SELECT * FROM Users WHERE Id = 1

The database returns a row of data: Id=123, Username=jdoe, Email=john@school.com, ...

EF Core needs to create a User object and set its properties.
- Now, one thing we need to understand fully is the role of DomainService:::
- #### **Role fo DmomainService**----- The gatekeeper fo creation.
1. **it validates business rules.**
- Validation should be in the Domain service--- the entity should be dumb. Doesnt know much.
```
public static User CreateUser(string username, string email, string fullName, UserRole role)
{
    if (string.IsNullOrWhiteSpace(username))
        throw new ArgumentException("Username is required.");

    if (!email.Contains('@'))
        throw new ArgumentException("Invalid email format.");

    // ... more validation
}
```
- Above is an example of a method from Domain service. VALIDATION SHOULD NEVER BE IN THA ENTITY.
2. **It sets default values.**
- Domain service sets defualt values that make sense for the busienss.
```
return new User(
    id: Guid.NewGuid(),      // Generate new ID
    username: username,
    email: email,
    fullName: fullName,
    role: role,
    isApproved: false,       // Default: must be approved by Admin
    isActive: true,          // Default: active by default
    createdAtUtc: DateTime.UtcNow,
    passwordHash: string.Empty
);
```

3. **Domain service calls the internal constructor that retruns the entity.**
4. ** Anytime User is created anywehre in the system, the Domain Serviceis used.

API creates a user → calls Domain Service

Admin creates a user → calls Domain Service

Bulk import creates users → calls Domain Service

Unit tests create users → calls Domain Service

This guarantees consistency: Every User created in the system goes through the same validation and same defaults.



External Code (API, Application, Tests)
    ↓
UserCreationService.CreateUser(...)   ← Domain Service
    ↓ (validates business rules)
    ↓ (sets default values)
    ↓
new User(...)                        ← Internal constructor
    ↓
User entity created
    ↓
Repository saves to database

**Chain of responsibilit**
API / Application ---	Receives the request, maps to DTO, sends to Handler
Handler	Orchestrates:-- calls Domain Service, then Repository
Domain Service---	Validates business rules, creates entity
Entity--	Holds state, encapsulates behavior
Repository--	Saves entity to database
EF Core	---Generates SQL, communicates with database

**The Domain Service is the only way to create a valid entity. Every entity must go through the Domain Service before it exists.**

Okay, got a bit sidetracked there. But it was imnportant for me to completely undeerstand the Domain service and wha tit does before i proceed with anything.

### **Why an INternal Constructor?**
- Domain service is in the same project as the entities. And it needs a way to create the entity without exposing the internal data.
- The internal cnostructor allows the Domain srvice to create the entity while keeping tit hidden from the rest of the system.
- n C# and .NET, internal means the member or class is visible only within files in the same project (assembly)

### **Why Methods instead of public setters?**
- Methods encapsulate behaviours in a way that public setters cannot. They allow us to enforce business rules and maintain the integrity of the entity's state.
- For examl::
- 
```csharp
public void Approve()
{
    if (IsApproved)
        throw new InvalidOperationException("User is already approved.");
    IsApproved = true;
}
```
- This method enforces a busniess rule in which you cannot approve a user who is already apprived.
- A public setter would allow anyone to set IsApproved to true without checks on the current state.
- But, its important to understand that the public setters can also validate the state. Validation can be done in public setters but it is not the best practice.
- It is better to use methods because they are more descriptive and can encapsulate complex logic.Public setters are prone to misuse and can lead to inconsistent states if not carefully managed.

----------> Now we have gone over the general structure of a domain entity. But, to really get tot the next level of understanding the code, i really need to go file to file. Entity to entity.
Obviously yhere is a pattern so i wont berepetitive. At first i will go over everything but then from that point we only focus on the unique or different things in a class. Otherwise i will waste my time explining recurring patterns.

## **DEEP DIVE ON USER ENTITY**
The code is already written (subjected to change ofcourse)::
```
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Domain.Enums.CarePrideSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarePrideSystem.Domain.Entities
{
    public class User
    {
        // Properties
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FullName { get; private set; }
        public UserRole Role { get; private set; }
        public bool IsApproved { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public string? TwoFactorSecret { get; private set; }
        public DateTime? LastLoginAtUtc { get; private set; }


        // Private constructor for EF Core
        private User() { }

        // Internal constructor for Domain Services
        internal User(
            Guid id,
            string username,
            string email,
            string fullName,
            UserRole role,
            bool isApproved,
            bool isActive,
            DateTime createdAtUtc,
            string? passwordHash = null)
        {
            Id = id;
            Username = username;
            Email = email;
            FullName = fullName;
            Role = role;
            IsApproved = isApproved;
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc;
            PasswordHash = passwordHash ?? string.Empty;
        }

        // Methods that change state
        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
            PasswordHash = passwordHash;
        }

        public void Approve()
        {
            if (IsApproved)
                throw new InvalidOperationException("User is already approved.");
            IsApproved = true;
        }

        public void Reject()
        {
            IsActive = false;
            IsApproved = false;
        }

        public void RecordLogin()
        {
            LastLoginAtUtc = DateTime.UtcNow;
        }

        public void EnableTwoFactor(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
                throw new ArgumentException("2FA Secret is required.", nameof(secret));
            TwoFactorSecret = secret;
        }

        public void DisableTwoFactor()
        {
            TwoFactorSecret = null;
        }
    }




}

```
- Now, as i said im going to dissect it piece by piece. And in all honesty it needs to be fast because time is very much limited right now. I still havent had a single working thing. I am yet to implement the CRUD of anything.
- And the proejct is in 3 days.
- So, i need to explain the roperties.
| Property | Type | Purpose |
| :--- | :--- | :--- |
| `Id` | `Guid` | Unique identifier. Generated by the Domain Service. |
| `Username` | `string` | Login username. Must be unique across the system. |
| `Email` | `string` | Email address. Must be unique. Used for login and notifications. |
| `PasswordHash` | `string` | Hashed password. Never stores the plain text password. |
| `FullName` | `string` | Display name. Shown in the UI. |
| `Role` | `UserRole` | User's role. Determines permissions. |
| `IsApproved` | `bool` | Admin has vetted this user. Must be true to log in. |
| `IsActive` | `bool` | Account is not suspended. If false, user cannot log in. |
| `CreatedAtUtc` | `DateTime` | Timestamp when the account was created. |
| `TwoFactorSecret` | `string?` | Secret key for 2FA. Null if 2FA is not enabled. |
| `LastLoginAtUtc` | `DateTime?` | Timestamp of the last successful login. Null if never logged in. |

#### Why GUID for Id isntead of int?
- This si something ive dealt with before.
- Guids are globally uniqueacross the whole system. If 2 schools merge their databases there is no ID collision.
- Also Guids are harder to guess. 
- And a very important one::: No dependency on the Dataabse. With aninteger ID, the database must generate the ID. With GUIDs the Domain serice generates the ID before the entity is ever saved.

#### Why separate IsApproved and Isctive?
| Scenario | IsApproved | IsActive | Meaning |
| :--- | :--- | :--- | :--- |
| Teacher registers | `false` | `true` | Account exists but not vetted. Cannot log in. |
| Admin approves | `true` | `true` | Teacher can log in. |
| Teacher resigns | `true` | `false` | Account is suspended. Cannot log in. |
| Admin rejects | `false` | `false` | Account is deleted. Cannot log in. |

#### Why RecordLogin() method instead of setting LastLoginAtUtc directly?
- Simply-- encapsulatino of behaviour.
- In futue we might want to do other things like check if account is locked, Log the IP address of the login and send a notification for suspicious logins.
- Using methods allows us to add these fetures without changing any other code.
- This is why its vital to understna the basics of inheritance, encapsulation, polyorphism and abstraction.And sometimes maybe composition too.
#### Why PasswordHash is a string and Not a Byte Array>
- To be honest this is my first time encountering this. 
- We will talk about passwords and security when we get there. For nw in the domain layer we dont have to tihink about that.
- But, i still need to understand why we PasswordHarsh is a string and not a byteaarray.
- **This is something im leaving for later.**

## DEEP DIVE:: STUDENT ENTITY

```
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
```

| Property | Type | Purpose |
| :--- | :--- | :--- |
| `Id` | `Guid` | Unique identifier. |
| `Name` | `string` | Class name (e.g., "Year 11A"). |
| `GradeLevel` | `int` | Year group (1 to 13). |
| `Capacity` | `int` | Maximum number of students allowed in the class. |
| `AcademicYearId` | `Guid` | Academic year this class belongs to. |
| `IsActive` | `bool` | Class is currently active. If false, no new students can be enrolled. |
| `CreatedAtUtc` | `DateTime` | Timestamp when the class was created. |

####  Why AdmissionNumber Instead of Id?

- The `Id` is for the system. It is a GUID.
- The `AdmissionNumber` is for the school. It is a human-readable identifier.
- Example: `2026-001` means the first student enrolled in 2026.

####  Why Archive Instead of Hard Delete?

- Students leave the school, but their academic records must be retained.
- Archiving marks the student as inactive but keeps all historical data.
- If a student returns, we can unarchive them.

####  Why No Navigation Property to Class?

The `ClassId` is stored as a GUID. The Domain does not have a navigation property to the `Class` entity. This is intentional.

**Why?**

- Navigation properties are used by EF Core to create relationships in the database.
- They are an Infrastructure concern.
- The Domain should not know about databases or relationships.
- The Domain only cares about the business rule: "A student belongs to a class."

## DEEP DIVE:: CLASS ENTITY

```
using CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Domain.Entities
{
    public class Class
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int GradeLevel { get; private set; }
        public int Capacity { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Class() { }

        internal Class(
            Guid id,
            string name,
            int gradeLevel,
            int capacity,
            Guid academicYearId,
            bool isActive,
            DateTime createdAtUtc)
        {
            Id = id;
            Name = name;
            GradeLevel = gradeLevel;
            Capacity = capacity;
            AcademicYearId = academicYearId;
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc;
        }

        public void UpdateDetails(string name, int gradeLevel, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Class name is required.", nameof(name));

            if (gradeLevel < 1 || gradeLevel > 13)
                throw new ArgumentException("Grade level must be between 1 and 13.", nameof(gradeLevel));

            if (capacity < 1)
                throw new ArgumentException("Capacity must be at least 1.", nameof(capacity));

            Name = name;
            GradeLevel = gradeLevel;
            Capacity = capacity;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Class is already active.");
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Class is already inactive.");
            IsActive = false;
        }
    }
}
```

#### Explanation of Properties

| Property | Type | Purpose |
| :--- | :--- | :--- |
| `Id` | `Guid` | Unique identifier. |
| `Name` | `string` | Class name (e.g., "Year 11A"). |
| `GradeLevel` | `int` | Year group (1 to 13). |
| `Capacity` | `int` | Maximum number of students allowed in the class. |
| `AcademicYearId` | `Guid` | Academic year this class belongs to. |
| `IsActive` | `bool` | Class is currently active. If false, no new students can be enrolled. |
| `CreatedAtUtc` | `DateTime` | Timestamp when the class was created. |


#### Why AcademicYearId is a GUID -
- This is a bit self explanatory. The domain stores the ID not the ibject.
- IDK hwat the hell this means fully but we move ahead.

## DEEP DIVE:: SUBJECT ENTITY
```
namespace CarePrideSystem.Domain.Entities
{
    public class Subject
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Subject() { }

        internal Subject(
            Guid id,
            string name,
            string code,
            string? description,
            bool isActive,
            DateTime createdAtUtc)
        {
            Id = id;
            Name = name;
            Code = code;
            Description = description;
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc;
        }

        public void UpdateDetails(string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Subject name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Subject code is required.", nameof(code));

            Name = name;
            Code = code;
            Description = description;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Subject is already active.");
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Subject is already inactive.");
            IsActive = false;
        }
    }
}
```

**Subject code is requird because we need short unique 'shrtnames" for the subjects like BIO, CHEM, etc etc.**
**Used in reports, timetables and transcripts.**

## DEEP DIVE :: MANY TO MANY LINKS -- TEACHERSUBJECT ENTITY AND THE CLASSTEAHCER ENTITY
```
using CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Domain.Entities
{
    public class TeacherSubject
    {
        public Guid TeacherId { get; private set; }
        public Guid SubjectId { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public DateTime AssignedAtUtc { get; private set; }

        private TeacherSubject() { }

        internal TeacherSubject(
            Guid teacherId,
            Guid subjectId,
            Guid classId,
            Guid academicYearId,
            DateTime assignedAtUtc)
        {
            TeacherId = teacherId;
            SubjectId = subjectId;
            ClassId = classId;
            AcademicYearId = academicYearId;
            AssignedAtUtc = assignedAtUtc;
        }
    }
}
```
- The one above is the TeacherSubject.cs Entity 


```
namespace CarePrideSystem.Domain.Entities
{
    public class ClassTeacher
    {
        public Guid TeacherId { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public bool IsPrimary { get; private set; }
        public DateTime AssignedAtUtc { get; private set; }

        private ClassTeacher() { }

        internal ClassTeacher(
            Guid teacherId,
            Guid classId,
            Guid academicYearId,
            bool isPrimary,
            DateTime assignedAtUtc)
        {
            TeacherId = teacherId;
            ClassId = classId;
            AcademicYearId = academicYearId;
            IsPrimary = isPrimary;
            AssignedAtUtc = assignedAtUtc;
        }

        public void SetAsPrimary()
        {
            if (IsPrimary)
                throw new InvalidOperationException("This teacher is already the primary class teacher.");
            IsPrimary = true;
        }

        public void SetAsSecondary()
        {
            if (!IsPrimary)
                throw new InvalidOperationException("This teacher is already a secondary class teacher.");
            IsPrimary = false;
        }
    }
}
```

- Now tbh i dont think i have enough time to go thru these classes in depth.
- The good thing about domain classe is that they are not inherently hard to understand. Also considering they are our innermost layer in the architectural structure, they dont have alot of complexities.
- **Important to understand what are navigation properties**
- Navigation properties are not in the Entities --> define relationships i the database.
- They belong in infrastructure , not domain.
- The Domain only cares aboutthe business rule: " A teacher teaches a subject ina  class for an academic year".

# ENUMS
```
using System;
using System.Collections.Generic;
using System.Text;

namespace CarePrideSystem.Domain.Enums
{
    
    namespace CarePrideSystem.Domain.Enums
    {
        public enum UserRole
        {
            Admin = 1,
            Teacher = 2,
            Secretary = 3
        }
    }


}

```

- Why are Enums in Domain? -- Because simple, roles are business rules. The domain defines who can do what in the school.
- Infrastructure and API will use the enum from the domain.

Not much to talk about in Enums.
Now we move onto the Domain services

# DOMAIN SERVICES

- All the services follow a pattern, infact alot of the code follows a pattern. That is basically a reason why we use separation of concerns.Let domain stuff be in domain so that we dont have to deal with confusion.
- Stuff of similar patterns stay in one place.
- We will look at one example of a Service class, then dissect the pattern, then verify that this pattern is followed by all the rest of the Domain service classes.
- 
**8TH SEPTEMBER 2026**
- Now why do we need Domain services? Good thing this si a question i have already answered.
- Domain services are like the bodyguards of the system. Ensure than not just anyone can create a user or a student, there are ways things have to be done and the Domain services are there to enforce that.
- We eill look at the example of UserCreationService, look at the code, look at th patterns and verify what we need to verify.

``` UserCreationService.cs

using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Domain.Enums.CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Domain.Services
{
    public static class UserCreationService
    {
        public static User CreateUser(
            string username,
            string email,
            string fullName,
            UserRole role)
        {
            // Business rule validation
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.", nameof(username));

            if (!email.Contains('@'))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.", nameof(fullName));

            // Role-specific rules
            if (role == UserRole.Admin && string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Admin must have a full name.");

            // Create the user
            return new User(
                id: Guid.NewGuid(),
                username: username,
                email: email,
                fullName: fullName,
                role: role,
                isApproved: false,      // Must be approved by Admin
                isActive: true,         // Active by default
                createdAtUtc: DateTime.UtcNow,
                passwordHash: string.Empty // Set later via SetPasswordHash()
            );
        }
    }
}


```
- Above is a domain service for user creation. That will be ou basis for establishing the patterns.
- First, why are they static classes and static methods?
-  Domain Services are static because they::
 - Have no state- Hodl no data.
 - oNLY DO PURE Logic-- take inputs, validate and return an entity. 
 - They do not need to be instantiated.-- no reason to cereate multiple instances.
 - Domain Services are static because they have no state. They are pure functions that take inputs and return outputs. There is no reason to create multiple instances of a stateless class.

-Okay. So wat does it mean to enforece businses rules? basic stuff like  the validation if email contains @< validating names etc etc.
```
public static User CreateUser(string username, string email, string fullName, UserRole role)
{
    if (string.IsNullOrWhiteSpace(username))
        throw new ArgumentException("Username is required.");

    if (!email.Contains('@'))
        throw new ArgumentException("Invalid email format.");

    // Business rule: Admin must have a full name
    if (role == UserRole.Admin && string.IsNullOrWhiteSpace(fullName))
        throw new ArgumentException("Admin must have a full name.");

    return new User(...);
}
```
- Without Domain service, anyone can create a user with an empty username but with domain service, such stuff is prevented because invalid data cannot be allowed to enter into the system.

- **Centralizes creation logic** ---> If the business rules change like if a user must input atleast 3 characters for a username to be valid, then only the Usrcreation service is changed.
- Wthout Domainservice -- you would have to change every place where a user is created.
- The Domain Service is the ONLY way to create a valid entity. No entity should ever be created using new Entity(). It must always go through the Domain Service.
Every entity follows the same pattern:

Entity is a dumb data container.

Domain Service handles creation and validation.

The Domain Service calls the internal constructor.


# DOMAIN EXCEPTIONS
- Custom exceprions that represent business rules vilations-- thrown when something breaks.
- Thy all inherit from a base class called DomainException.
DomainException (abstract)
    ↑
    ├── UserAlreadyApprovedException
    ├── UserNotFoundException
    ├── StudentNotFoundException
    ├── StudentAlreadyEnrolledException
    ├── ClassCapacityExceededException
    ├── ClassNotFoundException
    ├── SubjectNotFoundException
    ├── TeacherNotAssignedToSubjectException
    └── TeacherNotAssignedToClassException

```DomainException.cs
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}
```

- The base class is an abstract class because it should NEVER BE INSTANTIATED DIRECTYL.
- It exists to be inheritd by specific exceptions.
- It has 2 constructores.
- The first constructor takes a message, the second constructor takes a message and an inner exception for wrapping other exceptions.
``` UserNotFoundException.cs
public class UserNotFoundException : DomainException
{
    public UserNotFoundException(Guid userId)
        : base($"User with ID {userId} was not found.") { }

    public UserNotFoundException(string username)
        : base($"User with username '{username}' was not found.") { }
}
```
- An example of an exceptions-- inehrits from base DomainException -- 2 constructors because we might search for a user by ID or by Username - both are valid use cases - the exception message makes it clear what was searched for.
- So there is a pattern for Exceptions::
->Inherits from DomainException -- Ensures all domain exceptions are grouped together.
->Constructor with message -- Passes a clear, descriptive message to the base class.
->Constructors for different scenarios -- Different ways to search (e.g., by ID, by name, by email).

- Now code is all about pattersn as we can see now. And its all about grouping things that share hte same pattern.
SO, WHY DOES THIS PATTERN MATTER?
1.Grouping::
- All domain exceptions inherit from DomainException . This allows the API'S Global Exception Handler to catch all domain exceptions and handle them uniformly.

```
catch (DomainException ex)
{
    // Return 400 Bad Request with the error message
    // Not a 500 Internal Server Error
}

```
- Remember , only the API is executable. The application, domain and infrastructures are all class libraires.


**Now, what are interfaces and what do we need them for?** -- lowkey im spendin too much time on this.

# DOMAIN INTERFACES
- Repository interfaces are contracts that define how to retrieve and persist entities.

```
public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
    Task<IEnumerable<User>> GetPendingApprovalsAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}
```

- Repositories are the key to implementing Dependency inversion.
- Why do theylive in domain? 
- Domain says "i want to find a users by username" it doesnt say "i need a way to query SQLsrver". The structure shouldnt be coupled -- separation of concerns again applying here.
- By putting the interface in Domain:

Domain defines the contract (IUserRepository).

Infrastructure implements the contract (UserRepository).

Domain depends on the abstraction (interface), not the implementation.

- In Onion Architecture and dependency inversion, interfaces act as rules (contracts) that let inner layers talk to outer layers without knowing their real code.
- **Core uses for Interfaces:::-->**
- Invert Dependencies: The core business logic owns the interface. Outer layers (like databases or web frameworks) must follow it. This makes the center independent of the outside world.
- Hide Details: The core code only sees what a tool does (the interface), not how it does it (the messy code or specific database technology).
- Make Testing Easy: You can swap a real database or service with a fake one (a mock or stub) during tests.
- Allow Parallel Work: Teams can build the core business logic using just the interface while another team builds the real database or external API later.
- Keep Code Flexible: You can switch from one technology to another (like changing from a SQL database to a Mongo database) by writing a new class that fits the same interface, without changing your core rules.


- Every entity has a corresponding repository interface
- They also follow a pattern -- they all have the same? or yh the same standard methods/
GetByIdAsync---Get a single entity by ID.
GetAllAsync---Get all entities.
AddAsync---Save a new entity.
UpdateAsync---Update an existing entity.
DeleteAsync---Delete an entity.
ExistsAsync---Check if an entity exists.

- All the methods are async because the system is supposed to be built to be able to handle multiple concurrent requests efficeintly.
- When the system has 200 concurrent users, async prevents thread blocking.
- Asnc is essential for scalability.  Very much essential

- But, it will be good to visualise the flow from Domain to infrastructure.
 

 Domain (Interface)
    ↓
IUserRepository
    ↓
Infrastructure (Implementation)
    ↓
UserRepository (uses EF Core)
    ↓
Database (SQL Server)


- Domain doesnt know about EF Core. Only knows about the IUserRepository or any other repo class.
- Infrastructure implements the IUSerRepository using EF Core. If we switch to DAapper or ADO.NET, only the interface changes not domain/ 

### After understanding the Interfaces and how they work, there was somethign small i couldnt wrap my mind around:::
-  **Why do we put repositoriy intrfaces in Domain and how does infrastructure fit in?**

- #### 1. Without Interfaces

Without interfaces, the Handler needs to get user from the database.
- An example of this ::
```
public class CreateUserCommandHandler
{
    private readonly UserRepository _userRepository;

    public CreateUserCommandHandler()
    {
        // The Handler creates the concrete repository itself
        _userRepository = new UserRepository();
    }
}
```

- There are a few things wrong here. Firs, the Handler depends on the concrete UserRepository class.
- This class lives in infrastructure.
- Now this means the application layer where the Hander lives depends on INfrastrcuture.This breaks architecutre, Application should not depend on infrastrucutre at all.
- Why you may ask? Oh well -- imagine we change from SQL Server to PostgreSQL. We change UserRepository but then we will also have t change the Handler becasue it knows about UserRepository.

#### 2. With INterfaces.

- The interfaces should always be inside the Doamin.
```
// CarePride.Domain/Interfaces/Repositories/IUserRepository.cs
public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
```
- The niterface says::::
- "I need a way to get a user by ID."
- "I need a way to get a user by username."
- "I need a way to add a user."
etc etc etc

- IT DOES NOT SAY :
- usee sql server.
- use EF Core
- Use this connection string

Interfaces only says what must be done, and not How.
- These are very important concepts in building anything. Esepcially with how fast AI is advancing, humans will write less code but correct and debug more, which means you have to be even better thnan ever at understanding how systems work and scale.

**STEP 2:: THE HANDLER USES THE INTERFACE**

```
// CarePride.Application/Features/Auth/Commands/CreateUserCommandHandler.cs
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = UserCreationService.CreateUser(...);
        await _userRepository.AddAsync(user);
        return user.Id;
    }
}

```
- The Handler depends on IUserRepository, NOT UserRepository.
- Handler doesnt know how the user is being saved, where they are being saved or what the connectio string is.
- It just says "Add this user".

**STEP 3:: INFRASTRUCTURE IMPLEMENTS THE INTERFACE**
```
// CarePride.Infrastructure/Repositories/UserRepository.cs
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    // ... other methods
}
```

- This class implements IUserRepository.
- Uses EF Core and SQL Server to do the actual work.
- It is the Implementation of the interface.

**STEP 4:: THE DL CONTAINER WIRED THEM TOGETHER::**

- Inside the Porgam.cs of the API:::
```
// CarePride.API/Program.cs
builder.Services.AddScoped<IUserRepository, UserRepository>();
```

- What does that line mean? sometimes understanding lines inside program.cs is usually hell for me
- This line says " whenever someone asks for IUSerRepository give them UserRepository."
- The Handler does not know this. It just asks for IUserRepository and gets a UserRepository.


Application Layer (CreateUserCommandHandler)
    ↓
    Depends on: IUserRepository (Interface, defined in Domain)
    ↓
    ┌─────────────────────────────────────┐
    │  IUserRepository                    │
    │  - GetByIdAsync                     │
    │  - AddAsync                         │
    │  - UpdateAsync                      │
    │  - DeleteAsync                      │
    └─────────────────────────────────────┘
    ↑
    Implements: UserRepository (Concrete class, in Infrastructure)
    ↓
Infrastructure Layer (UserRepository)
    ↓
    Uses: AppDbContext (EF Core)
    ↓
    Writes to: SQL Server


**Direction of Dependency"::**
Application → IUserRepository (Abstraction)

Infrastructure → IUserRepository (Implementation)

Both depend on the interface, not on each other.

- Im not planning on changing Databases but waht if we change? man thats a topic for nother day 4rl. I cant preapre for everythign we willcross the bdge when i ge tthere for now jus tbe thankful that i have coverd asmuch as i have]
- im still massively behind schedule and im moving onto CRUD.
- 