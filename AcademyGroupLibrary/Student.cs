namespace AcademyGroupLibrary
{
    public class Student
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public double? GPA { get; set; }

        public virtual AcademyGroup? AcademyGroup { get; set; }
    }
}
