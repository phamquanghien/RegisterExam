using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JavaExamFinal.Data;
using System.Data;
using JavaExamFinal.Models.Process;

namespace JavaExamFinal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        ExcelProcess _excelPro = new ExcelProcess();

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
              return _context.DangKyThi != null ? 
                          View(await _context.DangKyThi.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DangKyThi'  is null.");
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string SecurityKey, [Bind("StudentID,FirstName,LastName,FullName,SubjectGroup,IsActive,CaThi,Subject")] DangKyThi dangKyThi)
        {
            if(string.IsNullOrEmpty(SecurityKey))
            {
                ModelState.AddModelError("","Vui lòng nhập Khoá bảo mật");
            }
            else if(SecurityKey != "14022012")
            {
                ModelState.AddModelError("","Khoá bảo mật không chính xác, vui lòng thử lại");
            }
            else if (ModelState.IsValid)
            {
                _context.Add(dangKyThi);
                await _context.SaveChangesAsync();
                UpdateRegisterCaThi(dangKyThi.Subject, dangKyThi.CaThi);
                return RedirectToAction(nameof(Index));
            }
            return View(dangKyThi);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DangKyThi == null)
            {
                return NotFound();
            }

            var dangKyThi = await _context.DangKyThi.FindAsync(id);
            if (dangKyThi == null)
            {
                return NotFound();
            }
            return View(dangKyThi);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string SecurityKey, [Bind("StudentID,FirstName,LastName,FullName,SubjectGroup,IsActive,CaThi,Subject")] DangKyThi dangKyThi)
        {
            if (id != dangKyThi.StudentID)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(SecurityKey))
            {
                ModelState.AddModelError("","Vui lòng nhập Khoá bảo mật");
            }
            else if(SecurityKey != "14022012")
            {
                ModelState.AddModelError("","Khoá bảo mật không chính xác, vui lòng thử lại");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dangKyThi);
                    await _context.SaveChangesAsync();
                    UpdateRegisterCaThi(dangKyThi.Subject, dangKyThi.CaThi);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DangKyThiExists(dangKyThi.StudentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dangKyThi);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DangKyThi == null)
            {
                return NotFound();
            }

            var dangKyThi = await _context.DangKyThi
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (dangKyThi == null)
            {
                return NotFound();
            }

            return View(dangKyThi);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string SecurityKey)
        {
            if (_context.DangKyThi == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DangKyThi'  is null.");
            }
            if(string.IsNullOrEmpty(SecurityKey))
            {
                ModelState.AddModelError("","Vui lòng nhập Khoá bảo mật");
            }
            else if(SecurityKey != "14022012")
            {
                ModelState.AddModelError("","Khoá bảo mật không chính xác, vui lòng thử lại");
            }
            else
            {
                var dangKyThi = await _context.DangKyThi.FindAsync(id);
                if (dangKyThi != null)
                {
                    _context.DangKyThi.Remove(dangKyThi);
                }
                
                await _context.SaveChangesAsync();
                UpdateRegisterCaThi(dangKyThi.Subject, dangKyThi.CaThi);
            }
            
            return RedirectToAction(nameof(Index));
        }
        public async void UpdateRegisterCaThi(string subject, string caThiUpdate)
        {
            var listBySubject = await _context.DangKyThi.Where(m => m.Subject == subject).ToListAsync();
            var listCaThi = await _context.CaThi.Where(m => m.Subject == subject).ToListAsync();
            var findCaThi = listCaThi.Where(m => m.CaThiName == caThiUpdate).Count();
            if(findCaThi == 0)
            {
                CaThi caThiNew = new CaThi();
                caThiNew.CaThiName = caThiUpdate;
                caThiNew.MaxValue = 80;
                caThiNew.RegistedValue = 1;
                caThiNew.Subject = subject;
                _context.Add(caThiNew);
                await _context.SaveChangesAsync();

            }

            for(int i = 0; i < listCaThi.Count; i++)
            {
                var countRegistedValue = listBySubject.Where(m => m.CaThi == listCaThi[i].CaThiName).Count();
                var caThi = await _context.CaThi.FindAsync(listCaThi[i].CaThiID);
                caThi.RegistedValue = countRegistedValue;
                _context.Update(caThi);
                await _context.SaveChangesAsync();
            }
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
        private bool DangKyThiExists(string id)
        {
          return (_context.DangKyThi?.Any(e => e.StudentID == id)).GetValueOrDefault();
        }
    }
}
