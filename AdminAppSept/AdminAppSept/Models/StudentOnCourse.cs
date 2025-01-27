namespace AdminAppSept.Models
{
    public class StudentOnCourse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }
        public Guid SemesterId { get; set; }
        public Semester? Semester { get; set; }
    }
}
