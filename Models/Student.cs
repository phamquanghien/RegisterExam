using System.ComponentModel.DataAnnotations;

public class Student
{
    [Key]
    public Guid ID { get; set; }
    public string StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string SubjectGroup { get; set; }
    public string Subject { get; set; }
    public bool IsActive { get; set; }
}