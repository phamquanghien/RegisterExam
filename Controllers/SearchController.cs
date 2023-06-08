using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using JavaExamFinal.Data;
using Microsoft.EntityFrameworkCore;
using JavaExamFinal.Models;

namespace JavaExamFinal.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string studentID, string subject)
        {
            var message = "";
            if(String.IsNullOrEmpty(studentID) || String.IsNullOrEmpty(subject)) message = "Vui lòng nhập thông tin để tìm kiếm";
            else
            {
                var dkt = _context.DangKyThi.Where(m => m.StudentID == studentID && m.Subject == subject).ToList();
                if(dkt.Count == 0) 
                {
                    message = "Không tìm thấy thông tin";
                }
                else
                {
                    var dkSuccess = dkt.First();
                     message = "Sinh viên: " + dkSuccess.FullName + "(" + dkSuccess.StudentID + ") đã đăng ký thi thành công môn ";
                    if(dkSuccess.Subject == "THVPNC") message += "Tin học Văn phòng Nâng cao - " + dkSuccess.CaThi;
                    else if(dkSuccess.Subject == "TMDT") message += "Thương mại Điện tử - " + dkSuccess.CaThi;
                    else if(dkSuccess.Subject == "JavaOOP") message += "Lập trình Hướng Đối tượng với java - " + dkSuccess.CaThi;
                }
                
            }
            ViewBag.thongBao = message;
            return View();
        }
        public async Task<IActionResult> Student()
        {
            return View(await _context.Student.Take(20).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Student(string stdID)
        {
            if(!string.IsNullOrEmpty(stdID))
            {
                return View(await _context.Student.Where(m => m.StudentID == stdID || m.Subject == stdID || m.Subject == stdID).ToListAsync());
            }
            else
            {
                return View(await _context.Student.Take(20).ToListAsync());
            }
        }
        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Security, [Bind("ID,StudentID,FirstName,LastName,FullName,SubjectGroup,Subject,IsActive")] Student student)
        {
            if(string.IsNullOrEmpty(Security)) {
                ModelState.AddModelError("","Mã xác thực không được để trống");
            } else if(Security != "14022012") {
                ModelState.AddModelError("","Mã xác thực không chính xác");
            } else {
                if (ModelState.IsValid)
                {
                    student.FullName = student.FirstName + " " + student.LastName;
                    student.ID = Guid.NewGuid();
                    _context.Add(student);
                    var dktByStudentID = await _context.DangKyThi.Where(m => m.StudentID == student.StudentID).ToListAsync();
                    var stdByStudentID = await _context.Student.Where(m => m.StudentID == student.StudentID).ToListAsync();
                    foreach(var item in dktByStudentID)
                    {
                        item.FirstName = student.FirstName;
                        item.LastName = student.LastName;
                        item.FullName = student.FullName;
                    }
                    foreach(var item2 in stdByStudentID)
                    {
                        item2.FirstName = student.FirstName;
                        item2.LastName = student.LastName;
                        item2.FullName = student.FullName;
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Student));
                }
            }
            
            return View(student);
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, string SecurityCode, [Bind("ID,StudentID,FirstName,LastName,FullName,SubjectGroup,Subject,IsActive")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(SecurityCode)) {
                ModelState.AddModelError("", "Mã xác thực không được để trống");
            } else
            {
                if(SecurityCode != "14022012") {
                    ModelState.AddModelError("", "Mã xác thực không chính xác");
                }
                else {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            student.FullName = student.FirstName + " " + student.LastName;
                            _context.Update(student);
                            var dktByStudentID = await _context.DangKyThi.Where(m => m.StudentID == student.StudentID).ToListAsync();
                            var stdByStudentID = await _context.Student.Where(m => m.StudentID == student.StudentID).ToListAsync();
                            foreach(var item in dktByStudentID)
                            {
                                item.FirstName = student.FirstName;
                                item.LastName = student.LastName;
                                item.FullName = student.FullName;
                            }
                            foreach(var item2 in stdByStudentID)
                            {
                                item2.FirstName = student.FirstName;
                                item2.LastName = student.LastName;
                                item2.FullName = student.FullName;
                            }
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!StudentExists(student.ID))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(Student));
                    }
                }
            }
            return View(student);
        }
        public async Task<IActionResult> NoRegister()
        {
            string tt = "Tong: ";
            int dem = 0;
            var stdList = await _context.Student.ToListAsync();
            //tt += stdList.Count() + " SV, da dang ky: ";
            var dktList = await _context.DangKyThi.ToListAsync();
            //tt += dktList.Count() + ", con lai: (" + (stdList.Count()-dktList.Count()) + ") ";
            tt += dktList.Count() + ", ";
            // for (int i = 0; i < stdList.Count; i++)
            // {
            //     var checkExist = dktList.Where(m => m.StudentID == stdList[i].StudentID && m.Subject == stdList[i].Subject & m.SubjectGroup == stdList[i].SubjectGroup).Count();
            //     if (checkExist > 0)
            //     {
            //         stdList.RemoveAt(i);
            //         i--;
            //     }
            // }
            // tt += stdList.Count();
            for (int i = 0; i < dktList.Count; i++)
            {
                var checkExist = stdList.Where(m => m.StudentID ==  dktList[i].StudentID && m.Subject == dktList[i].Subject && m.SubjectGroup == dktList[i].SubjectGroup).Count();
                if(checkExist >0)
                {
                    dktList.RemoveAt(i);
                    i--;
                }
            }
            tt += "con: " + dktList.Count + "; ";
            for(int i = 0; i < dktList.Count; i++)
            {
                tt += dktList[i].StudentID + "; ";
            }
            ViewBag.thongBao = tt;
            return View(stdList);
        }
        private bool StudentExists(Guid id)
        {
          return (_context.Student?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}