using System;
using System.ComponentModel.DataAnnotations;

public class DangKyThi
{
    [Key]
    public Guid ID { get; set; }
    public string StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string SubjectGroup { get; set; }
    public bool IsActive { get; set; }
    public string CaThi { get; set; }
    public string Subject { get; set; }
}