using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JavaExamFinal.Models;
using JavaExamFinal.Models.Process;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavaExamFinal.Controllers;

public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;
    ExcelProcess _excelPro = new ExcelProcess();
    public StudentController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public async Task<IActionResult> Index()
    {
        var students = _context.Student.Take(20);
        return View(await students.ToListAsync());
    }
    [HttpPost]
    public async Task<IActionResult> Index(string studentSubject, string Subject)
    {
        var students = from m in _context.Student
                    select m;

        if (!String.IsNullOrEmpty(studentSubject))
        {
            students = students.Where(s => s.SubjectGroup == studentSubject);
        }
        ViewData["subjectGroup"] = new SelectList(_context.Student, "SubjectGroup", "SubjectGroup");
        return View(await students.ToListAsync());
    }
    public IActionResult Upload()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Upload(string checkKey, IFormFile file, string Subject)
    {
        if(checkKey == "14022004")
        {
            int countStudent = 0;
            if (file!=null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    //rename file when upload to server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/Uploads/Excels", "File" + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + fileExtension);
                    var fileLocation = new FileInfo(filePath).ToString();
                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            //save file to server
                            await file.CopyToAsync(stream);
                            //read data from file and write to database
                            var dt = _excelPro.ExcelToDataTable(fileLocation);
                            countStudent = WriteListStudent(dt, Subject);
                        }
                    }
                }
            }
            ViewBag.mess = countStudent;
        }
        else {
            ModelState.AddModelError("", "Key xac thuc khong chinh xac");
        }
        
        return View();
    }
    public int WriteListStudent(DataTable dt, string Subject)
    {
        try
        {
            for(int i=0; i < dt.Rows.Count; i++)
            {
                Student std = new Student();
                std.StudentID = dt.Rows[i][0].ToString();
                std.FirstName = dt.Rows[i][1].ToString();
                std.LastName = dt.Rows[i][2].ToString();
                std.FullName = std.FirstName + " " + std.LastName;
                std.SubjectGroup = dt.Rows[i][3].ToString();
                if(dt.Rows[i][4].ToString() == "1") std.IsActive = false;
                else std.IsActive = true;
                std.Subject = Subject;
                _context.Student.Add(std);
            }
            _context.SaveChanges();
            return dt.Rows.Count;
        }catch
        {
            return 0;
        }
    }
}