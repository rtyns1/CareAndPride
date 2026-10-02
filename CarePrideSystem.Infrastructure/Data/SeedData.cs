using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Application.Interfaces.Services;
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Infrastructure.Data
{
    public static class SeedData
    {
        private static readonly Guid DefaultAcademicYearId =
            Guid.Parse("00000000-0000-0000-0000-000000000001");

        public static async Task InitializeAsync(AppDbContext context, IPasswordHasher hasher)
        {
            await SeedAdminAsync(context, hasher);
            await SeedClassesAsync(context);
            await SeedSubjectsAsync(context);
            await SeedTeachersAsync(context, hasher);
            await SeedClassTeachersAsync(context);
            await SeedTeacherSubjectsAsync(context);
            await SeedStudentsAsync(context);
            await SeedGradesAsync(context);
        }

        private static async Task SeedAdminAsync(AppDbContext context, IPasswordHasher hasher)
        {
            if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin)) return;
            var admin = UserCreationService.CreateUser("admin", "admin@school.com", "System Administrator", UserRole.Admin);
            admin.SetPasswordHash(hasher.HashPassword("Admin@123"));
            admin.Approve();
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        private static async Task SeedClassesAsync(AppDbContext context)
        {
            if (await context.Classes.AnyAsync()) return;
            var classes = new (string Name, int Grade, int Capacity)[]
            {
                ("PP1", 0, 30), ("PP2", 0, 30),
                ("Year 1", 1, 40), ("Year 2", 2, 40), ("Year 3", 3, 40),
                ("Year 4", 4, 40), ("Year 5", 5, 40), ("Year 6", 6, 40),
                ("Year 7", 7, 40), ("Year 8", 8, 40), ("Year 9", 9, 40),
                ("Year 10", 10, 40), ("Year 11", 11, 40),
                ("Year 12", 12, 40), ("Year 13", 13, 40),
            };
            foreach (var c in classes)
            {
                var cls = ClassCreationService.CreateClass(c.Name, c.Grade, c.Capacity, DefaultAcademicYearId);
                await context.Classes.AddAsync(cls);
            }
            await context.SaveChangesAsync();
        }

        private static async Task SeedSubjectsAsync(AppDbContext context)
        {
            if (await context.Subjects.AnyAsync()) return;
            var subjects = new (string Name, string Code, string Description)[]
            {
                ("Mathematics", "MATH", "Core mathematics"),
                ("English", "ENG", "English language"),
                ("Literature", "LIT", "English literature"),
                ("Business", "BUS", "Business studies"),
                ("Biology", "BIO", "Biology"),
                ("Chemistry", "CHEM", "Chemistry"),
                ("Physics", "PHY", "Physics"),
                ("History", "HIST", "History"),
                ("Geography", "GEO", "Geography"),
                ("Computer Science", "CS", "Computer studies"),
            };
            foreach (var s in subjects)
                await context.Subjects.AddAsync(SubjectCreationService.CreateSubject(s.Name, s.Code, s.Description));
            await context.SaveChangesAsync();
        }

        private static async Task SeedTeachersAsync(AppDbContext context, IPasswordHasher hasher)
        {
            if (await context.Users.AnyAsync(u => u.Role == UserRole.Teacher)) return;
            var teachers = new (string Username, string Email, string FullName)[]
            {
                ("acristina", "anne.cristina@school.com", "Anne Cristina"),
                ("jmwangi", "james.mwangi@school.com", "James Mwangi"),
                ("sochieng", "sarah.ochieng@school.com", "Sarah Ochieng"),
                ("motieno", "michael.otieno@school.com", "Michael Otieno"),
                ("rwanjiru", "rebecca.wanjiru@school.com", "Rebecca Wanjiru"),
            };
            foreach (var t in teachers)
            {
                var u = UserCreationService.CreateUser(t.Username, t.Email, t.FullName, UserRole.Teacher);
                u.SetPasswordHash(hasher.HashPassword("Teacher@123"));
                u.Approve();
                await context.Users.AddAsync(u);
            }
            await context.SaveChangesAsync();
        }

        private static async Task SeedClassTeachersAsync(AppDbContext context)
        {
            if (await context.ClassTeachers.AnyAsync()) return;
            var assignments = new (string Teacher, string Class)[]
            {
                ("acristina", "Year 11"),
                ("jmwangi", "Year 10"),
                ("sochieng", "Year 9"),
                ("motieno", "Year 12"),
                ("rwanjiru", "Year 13"),
            };
            foreach (var a in assignments)
            {
                var t = await context.Users.FirstOrDefaultAsync(u => u.Username == a.Teacher);
                var c = await context.Classes.FirstOrDefaultAsync(cl => cl.Name == a.Class);
                if (t == null || c == null) continue;
                await context.ClassTeachers.AddAsync(
                    ClassTeacherCreationService.CreateClassTeacher(t.Id, c.Id, DefaultAcademicYearId, true));
            }
            await context.SaveChangesAsync();
        }

        private static async Task SeedTeacherSubjectsAsync(AppDbContext context)
        {
            if (await context.TeacherSubjects.AnyAsync()) return;
            var assignments = new (string Teacher, string Subject, string Class)[]
            {
                ("acristina", "CHEM", "Year 10"),
                ("acristina", "BIO", "Year 9"),
                ("acristina", "CHEM", "Year 11"),
                ("acristina", "BIO", "Year 11"),
                ("jmwangi", "ENG", "Year 10"),
                ("jmwangi", "LIT", "Year 10"),
                ("jmwangi", "BUS", "Year 11"),
                ("sochieng", "CHEM", "Year 9"),
                ("sochieng", "BIO", "Year 10"),
                ("sochieng", "CHEM", "Year 12"),
                ("sochieng", "BIO", "Year 12"),
                ("motieno", "PHY", "Year 12"),
                ("motieno", "MATH", "Year 11"),
                ("motieno", "PHY", "Year 13"),
                ("rwanjiru", "MATH", "Year 13"),
                ("rwanjiru", "CS", "Year 13"),
                ("rwanjiru", "MATH", "Year 12"),
            };
            foreach (var a in assignments)
            {
                var t = await context.Users.FirstOrDefaultAsync(u => u.Username == a.Teacher);
                var s = await context.Subjects.FirstOrDefaultAsync(su => su.Code == a.Subject);
                var c = await context.Classes.FirstOrDefaultAsync(cl => cl.Name == a.Class);
                if (t == null || s == null || c == null) continue;
                await context.TeacherSubjects.AddAsync(
                    TeacherSubjectCreationService.CreateTeacherSubject(t.Id, s.Id, c.Id, DefaultAcademicYearId));
            }
            await context.SaveChangesAsync();
        }

        private static async Task SeedStudentsAsync(AppDbContext context)
        {
            if (await context.Students.AnyAsync()) return;
            var year9  = await context.Classes.FirstOrDefaultAsync(c => c.Name == "Year 9");
            var year10 = await context.Classes.FirstOrDefaultAsync(c => c.Name == "Year 10");
            var year11 = await context.Classes.FirstOrDefaultAsync(c => c.Name == "Year 11");
            var year12 = await context.Classes.FirstOrDefaultAsync(c => c.Name == "Year 12");
            var year13 = await context.Classes.FirstOrDefaultAsync(c => c.Name == "Year 13");
            if (year9 == null || year10 == null || year11 == null || year12 == null || year13 == null) return;

            var students = new (string First, string Last, DateTime Dob, string Adm, Guid ClassId, string? Medical)[]
            {
                ("Amani","Kamau",new DateTime(2011,3,12),"ADM-Y9-001",year9.Id,null),
                ("Blessing","Njeri",new DateTime(2011,7,4),"ADM-Y9-002",year9.Id,"Asthma"),
                ("Cynthia","Mwangi",new DateTime(2011,11,22),"ADM-Y9-003",year9.Id,null),
                ("Daniel","Akinyi",new DateTime(2011,5,9),"ADM-Y9-004",year9.Id,null),
                ("Eunice","Ochieng",new DateTime(2011,1,15),"ADM-Y9-005",year9.Id,"Peanut allergy"),
                ("Faith","Wanjiku",new DateTime(2011,9,3),"ADM-Y9-006",year9.Id,null),
                ("George","Otieno",new DateTime(2011,12,18),"ADM-Y9-007",year9.Id,null),
                ("Hilda","Mutiso",new DateTime(2011,4,7),"ADM-Y9-008",year9.Id,null),
                ("Isaac","Kipchoge",new DateTime(2011,8,25),"ADM-Y9-009",year9.Id,null),
                ("Joy","Nafula",new DateTime(2011,6,11),"ADM-Y9-010",year9.Id,null),

                ("Kevin","Omondi",new DateTime(2010,2,14),"ADM-Y10-001",year10.Id,null),
                ("Lydia","Wambui",new DateTime(2010,6,30),"ADM-Y10-002",year10.Id,null),
                ("Martin","Kariuki",new DateTime(2010,10,8),"ADM-Y10-003",year10.Id,"Diabetes"),
                ("Nancy","Achieng",new DateTime(2010,3,21),"ADM-Y10-004",year10.Id,null),
                ("Oscar","Barasa",new DateTime(2010,8,17),"ADM-Y10-005",year10.Id,null),
                ("Purity","Chebet",new DateTime(2010,12,5),"ADM-Y10-006",year10.Id,null),
                ("Quinter","Adhiambo",new DateTime(2010,5,27),"ADM-Y10-007",year10.Id,null),
                ("Robert","Njoroge",new DateTime(2010,1,9),"ADM-Y10-008",year10.Id,null),
                ("Susan","Atieno",new DateTime(2010,9,15),"ADM-Y10-009",year10.Id,null),
                ("Tom","Kilonzo",new DateTime(2010,7,23),"ADM-Y10-010",year10.Id,null),

                ("Ursula","Mutindi",new DateTime(2009,4,2),"ADM-Y11-001",year11.Id,null),
                ("Victor","Wekesa",new DateTime(2009,8,19),"ADM-Y11-002",year11.Id,null),
                ("Winnie","Muthoni",new DateTime(2009,12,11),"ADM-Y11-003",year11.Id,"Migraine"),
                ("Xavier","Ouma",new DateTime(2009,6,24),"ADM-Y11-004",year11.Id,null),
                ("Yvonne","Nduta",new DateTime(2009,2,8),"ADM-Y11-005",year11.Id,null),
                ("Zachary","Kiptoo",new DateTime(2009,10,30),"ADM-Y11-006",year11.Id,null),
                ("Alice","Auma",new DateTime(2009,5,16),"ADM-Y11-007",year11.Id,null),
                ("Benjamin","Muriuki",new DateTime(2009,1,28),"ADM-Y11-008",year11.Id,null),
                ("Carol","Wanjala",new DateTime(2009,9,4),"ADM-Y11-009",year11.Id,null),
                ("Dennis","Onyango",new DateTime(2009,7,22),"ADM-Y11-010",year11.Id,null),

                ("Esther","Chepkoech",new DateTime(2008,3,6),"ADM-Y12-001",year12.Id,null),
                ("Felix","Mwendwa",new DateTime(2008,7,14),"ADM-Y12-002",year12.Id,null),
                ("Gladys","Anyango",new DateTime(2008,11,2),"ADM-Y12-003",year12.Id,null),
                ("Henry","Kibet",new DateTime(2008,5,20),"ADM-Y12-004",year12.Id,"Epilepsy"),
                ("Irene","Wairimu",new DateTime(2008,1,12),"ADM-Y12-005",year12.Id,null),
                ("Joseph","Odhiambo",new DateTime(2008,9,28),"ADM-Y12-006",year12.Id,null),
                ("Karen","Mueni",new DateTime(2008,6,8),"ADM-Y12-007",year12.Id,null),
                ("Lawrence","Karanja",new DateTime(2008,2,24),"ADM-Y12-008",year12.Id,null),
                ("Mercy","Jepkorir",new DateTime(2008,10,16),"ADM-Y12-009",year12.Id,null),
                ("Nathan","Odongo",new DateTime(2008,8,30),"ADM-Y12-010",year12.Id,null),

                ("Ophelia","Wanjala",new DateTime(2007,3,18),"ADM-Y13-001",year13.Id,null),
                ("Patrick","Kimani",new DateTime(2007,7,26),"ADM-Y13-002",year13.Id,null),
                ("Queenie","Atieno",new DateTime(2007,11,9),"ADM-Y13-003",year13.Id,null),
                ("Ronald","Mutua",new DateTime(2007,5,3),"ADM-Y13-004",year13.Id,"Hypertension"),
                ("Stella","Wambui",new DateTime(2007,1,21),"ADM-Y13-005",year13.Id,null),
                ("Timothy","Obara",new DateTime(2007,9,12),"ADM-Y13-006",year13.Id,null),
                ("Unity","Nyambura",new DateTime(2007,6,30),"ADM-Y13-007",year13.Id,null),
                ("Vincent","Ochieng",new DateTime(2007,2,14),"ADM-Y13-008",year13.Id,null),
                ("Wendy","Chepkirui",new DateTime(2007,10,25),"ADM-Y13-009",year13.Id,null),
                ("Yusuf","Abdalla",new DateTime(2007,8,7),"ADM-Y13-010",year13.Id,null),
            };
            foreach (var s in students)
            {
                await context.Students.AddAsync(StudentCreationService.CreateStudent(
                    s.First, s.Last, s.Dob, s.Adm, s.ClassId, s.Medical));
            }
            await context.SaveChangesAsync();
        }

        private static async Task SeedGradesAsync(AppDbContext context)
        {
            if (await context.Grades.AnyAsync()) return;

            var teacher = await context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Teacher);
            if (teacher == null) return;

            var students = await context.Students.ToListAsync();
            var subjects = await context.Subjects.ToListAsync();
            if (students.Count == 0 || subjects.Count == 0) return;

            // Each student gets grades in 4 subjects: MATH, ENG, BIO, CHEM
            var targetCodes = new[] { "MATH", "ENG", "BIO", "CHEM" };
            var targetSubjects = subjects.Where(s => targetCodes.Contains(s.Code)).ToList();

            foreach (var student in students)
            {
                int seed = student.AdmissionNumber.Sum(c => (int)c);

                for (int i = 0; i < targetSubjects.Count; i++)
                {
                    var subject = targetSubjects[i];
                    int subjSeed = seed + (i * 37);

                    // Midterm
                    var midScore = 45 + (subjSeed % 45);           // 45..89
                    var midGrade = GradeCreationService.CreateGrade(
                        student.Id, subject.Id, student.ClassId, DefaultAcademicYearId,
                        teacher.Id, "Midterm", "Term 1", midScore, 100, null);
                    await context.Grades.AddAsync(midGrade);

                    // Endterm
                    var endScore = 50 + ((subjSeed * 3) % 45);     // 50..94
                    var endGrade = GradeCreationService.CreateGrade(
                        student.Id, subject.Id, student.ClassId, DefaultAcademicYearId,
                        teacher.Id, "Endterm", "Term 1", endScore, 100, null);
                    await context.Grades.AddAsync(endGrade);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
