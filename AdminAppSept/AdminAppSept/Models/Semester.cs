namespace AdminAppSept.Models
{
    public class Semester
    {
        public string? SemesterCode { get; set; }
        public string? SemesterName { get; set; }
        public virtual ICollection<StudentOnCourse>? StudentOnCourses { get; set; }
    }
}
