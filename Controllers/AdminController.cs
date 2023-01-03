using System.Drawing;
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
using OfficeOpenXml;
using JavaExamFinal.Models;
using OfficeOpenXml.Style;

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
            // var models = await _context.DangKyThi.Where(m => m.Subject == "THVPNC" && m.SubjectGroup == "Ca1").ToListAsync();
            //         return View(models);
              return _context.DangKyThi != null ? 
                          View(await _context.DangKyThi.Take(20).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DangKyThi'  is null.");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string Subject, string CaThi)
        {
            if(string.IsNullOrEmpty(Subject)) ModelState.AddModelError("", "Vui lòng chọn học phần để tìm thông tin");
            else
            {
                if(string.IsNullOrEmpty(CaThi))
                {
                    ModelState.AddModelError("", "Vui lòng chọn nhóm môn học để tìm thông tin");
                    return View(await _context.DangKyThi.Where(m => m.Subject == "ZZZ").ToListAsync());
                }
                else
                {
                    var models = await _context.DangKyThi.Where(m => m.Subject == Subject && m.CaThi == CaThi).ToListAsync();
                    return View(models);
                }
            }
            return View(await _context.DangKyThi.Where(m => m.Subject == "ZZZ").ToListAsync());
        }
        public IActionResult Student()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AllStudent(string Subject, string SubjectGroup)
        {
            if(string.IsNullOrEmpty(Subject)) ModelState.AddModelError("", "Vui lòng chọn học phần để tìm thông tin");
            else
            {
                var fileName = DateTime.Now.ToLongTimeString() + ".xlsx";
                    using(ExcelPackage excelPackage = new ExcelPackage())
                    {
                        //create a WorkSheet
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                        //get list sinh vien chua dang ky thi
                        //1. get danh sach sinh vien cua hoc phan
                        var listChuaDangKyThi = _context.Student.Where(m => m.Subject == Subject).ToList();
                        var listDangKy = _context.DangKyThi.Where(m => m.Subject == Subject).ToList();
                        //2. get danh sach sinh vien da dang ky cua hoc phan
                        if(!string.IsNullOrEmpty(SubjectGroup) && SubjectGroup != "")
                        {
                            listChuaDangKyThi = listChuaDangKyThi.Where(m => m.SubjectGroup == SubjectGroup).ToList();
                            listDangKy = listDangKy.Where(m => m.SubjectGroup == SubjectGroup).ToList();
                        }
                        List<StudentViewModel2> list = new List<StudentViewModel2>();
                        for(int i = 0; i < listChuaDangKyThi.Count; i++)
                        {
                            StudentViewModel2 stdVM2 = new StudentViewModel2();
                            stdVM2.STT = i + 1;
                            stdVM2.StudentID = listChuaDangKyThi[i].StudentID;
                            stdVM2.FirstName = listChuaDangKyThi[i].FirstName;
                            stdVM2.LastName = listChuaDangKyThi[i].LastName;
                            stdVM2.SubjectGroup = listChuaDangKyThi[i].SubjectGroup;
                            if(listChuaDangKyThi[i].IsActive == false)
                            {
                                stdVM2.IsActive = "Cấm thi";
                            }
                            var checkRegisted = listDangKy.Where(m => m.StudentID == listChuaDangKyThi[i].StudentID && m.Subject == listChuaDangKyThi[i].Subject && m.SubjectGroup == listChuaDangKyThi[i].SubjectGroup).ToList().Count();
                            if(checkRegisted == 0)
                            {
                                stdVM2.Registed = "Chưa đăng ký";
                            }
                            
                            list.Add(stdVM2);
                        }
                        
                        worksheet.Cells["A1:G1"].Merge = true;
                        switch(Subject)
                        {
                            case "THVPNC": worksheet.Cells["A1"].Value = "Danh sách Sinh viên chưa đăng ký ca thi môn Tin học Văn phòng Nâng cao nhóm " + SubjectGroup;
                            break;
                            case "TMDT": worksheet.Cells["A1"].Value = "Danh sách Sinh viên chưa đăng ký ca thi môn Thương mại Điện tử nhóm " + SubjectGroup;
                            break;
                            case "QTDA": worksheet.Cells["A1"].Value = "Danh sách Sinh viên chưa đăng ký ca thi môn Quản trị Dự án CNTT nhóm " + SubjectGroup;
                            break;
                        }

                        worksheet.Cells["A3"].Value = "STT";
                        worksheet.Cells["B3"].Value = "Mã Sinh viên";
                        worksheet.Cells["C3"].Value = "Họ tên";
                        worksheet.Cells["E3"].Value = "Nhóm môn học";
                        worksheet.Cells["F3"].Value = "Ghi chú";
                        worksheet.Cells["G3"].Value = "Đăng ký thi";
                        worksheet.Cells["C3:D3"].Merge = true;
                        worksheet.Cells["A3:F3"].Style.Font.Bold = true;
                        worksheet.Cells["A3:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A4"].LoadFromCollection(list);
                        worksheet.Cells["A:G"].Style.Font.Size = 13;
                        worksheet.Cells["A:B"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["E:G"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Column(1).AutoFit();
                        worksheet.Column(2).AutoFit();
                        worksheet.Column(3).AutoFit();
                        worksheet.Column(4).AutoFit();
                        worksheet.Column(5).AutoFit();
                        worksheet.Column(6).AutoFit();
                        worksheet.Column(7).AutoFit();

                        string modelRange = "A1:G" + (list.Count() + 3);
                        var modelTable = worksheet.Cells[modelRange];

                        // Assign borders
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        var stream = new MemoryStream(excelPackage.GetAsByteArray()); //Get updated stream
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
            }
            return RedirectToAction("Student");
        }
        [HttpPost]
        public async Task<IActionResult> FileStudent(string Subject, string SubjectGroup)
        {
            if(string.IsNullOrEmpty(Subject)) ModelState.AddModelError("", "Vui lòng chọn học phần để tìm thông tin");
            else
            {
                var fileName = DateTime.Now.ToLongTimeString() + ".xlsx";
                    using(ExcelPackage excelPackage = new ExcelPackage())
                    {
                        //create a WorkSheet
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                        //get list sinh vien chua dang ky thi
                        //1. get danh sach sinh vien cua hoc phan
                        var listChuaDangKyThi = _context.Student.Where(m => m.Subject == Subject).ToList();
                        var listDangKy = _context.DangKyThi.Where(m => m.Subject == Subject).ToList();
                        //2. get danh sach sinh vien da dang ky cua hoc phan
                        if(!string.IsNullOrEmpty(SubjectGroup) && SubjectGroup != "")
                        {
                            listChuaDangKyThi = listChuaDangKyThi.Where(m => m.SubjectGroup == SubjectGroup).ToList();
                            listDangKy = listDangKy.Where(m => m.SubjectGroup == SubjectGroup).ToList();
                        }
                        
                        for(int i = 0; i < listChuaDangKyThi.Count; i++)
                        {
                            var checkRegisted = listDangKy.Where(m => m.StudentID == listChuaDangKyThi[i].StudentID && m.Subject == listChuaDangKyThi[i].Subject && m.SubjectGroup == listChuaDangKyThi[i].SubjectGroup).ToList().Count();
                            if(checkRegisted > 0)
                            {
                                listChuaDangKyThi.RemoveAt(i);
                                i--;
                            }
                        }
                        List<StudentViewModel> list = new List<StudentViewModel>();
                        for (var i = 0; i < listChuaDangKyThi.Count; i++)
                        {
                            StudentViewModel stdVM = new StudentViewModel();
                            stdVM.STT = i + 1;
                            stdVM.StudentID = listChuaDangKyThi[i].StudentID;
                            stdVM.FirstName = listChuaDangKyThi[i].FirstName;
                            stdVM.LastName = listChuaDangKyThi[i].LastName;
                            stdVM.SubjectGroup = listChuaDangKyThi[i].SubjectGroup;
                            if(listChuaDangKyThi[i].IsActive == false)
                            {
                                stdVM.IsActive = "Cấm thi";
                            }
                            list.Add(stdVM);
                        }
                        
                        worksheet.Cells["A1:F1"].Merge = true;
                        switch(Subject)
                        {
                            case "THVPNC": worksheet.Cells["A1"].Value = "Danh sách Sinh viên chưa đăng ký ca thi môn Tin học Văn phòng Nâng cao nhóm " + SubjectGroup;
                            break;
                            case "TMDT": worksheet.Cells["A1"].Value = "Danh sách Sinh viên chưa đăng ký ca thi môn Thương mại Điện tử nhóm " + SubjectGroup;
                            break;
                            case "QTDA": worksheet.Cells["A1"].Value = "Danh sách Sinh viên chưa đăng ký ca thi môn Quản trị Dự án CNTT nhóm " + SubjectGroup;
                            break;
                        }

                        worksheet.Cells["A3"].Value = "STT";
                        worksheet.Cells["B3"].Value = "Mã Sinh viên";
                        worksheet.Cells["C3"].Value = "Họ tên";
                        worksheet.Cells["E3"].Value = "Nhóm môn học";
                        worksheet.Cells["F3"].Value = "Ghi chú";
                        worksheet.Cells["C3:D3"].Merge = true;
                        worksheet.Cells["A3:F3"].Style.Font.Bold = true;
                        worksheet.Cells["A3:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A4"].LoadFromCollection(list);
                        worksheet.Cells["A:F"].Style.Font.Size = 13;
                        worksheet.Cells["A:B"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["E:F"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Column(1).AutoFit();
                        worksheet.Column(2).AutoFit();
                        worksheet.Column(3).AutoFit();
                        worksheet.Column(4).AutoFit();
                        worksheet.Column(5).AutoFit();
                        worksheet.Column(6).AutoFit();
                        worksheet.Column(7).AutoFit();

                        string modelRange = "A1:F" + (list.Count() + 3);
                        var modelTable = worksheet.Cells[modelRange];

                        // Assign borders
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        var stream = new MemoryStream(excelPackage.GetAsByteArray()); //Get updated stream
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
            }
            return RedirectToAction("Student");
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
        public async Task<IActionResult> Create(string SecurityKey, [Bind("ID,StudentID,FirstName,LastName,FullName,SubjectGroup,IsActive,CaThi,Subject")] DangKyThi dangKyThi)
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
        public async Task<IActionResult> Edit(Guid? id)
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
        public async Task<IActionResult> Edit(Guid id, string SecurityKey, [Bind("ID,StudentID,FirstName,LastName,FullName,SubjectGroup,IsActive,CaThi,Subject")] DangKyThi dangKyThi)
        {
            if (id != dangKyThi.ID)
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
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.DangKyThi == null)
            {
                return NotFound();
            }

            var dangKyThi = await _context.DangKyThi
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dangKyThi == null)
            {
                return NotFound();
            }

            return View(dangKyThi);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, string SecurityKey)
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
        
        public async Task<IActionResult> Download(string Subject, string CaThi)
        {
            if(string.IsNullOrEmpty(Subject)) ModelState.AddModelError("", "Vui lòng chọn học phần để tìm thông tin");
            else
            {
                if(string.IsNullOrEmpty(CaThi))
                {
                    ModelState.AddModelError("", "Vui lòng chọn nhóm môn học để tìm thông tin");
                    return View(await _context.DangKyThi.Where(m => m.Subject == "ZZZ").ToListAsync());
                }
                else
                {
                    var models = await _context.DangKyThi.Where(m => m.Subject == Subject && m.CaThi == CaThi).ToListAsync();
                    return View(models);
                }
            }
            return View(await _context.DangKyThi.Where(m => m.Subject == "ZZZ").ToListAsync());
        }
        public IActionResult GetFile(string Subject, string CaThi)
        {
            if(string.IsNullOrEmpty(Subject)) ModelState.AddModelError("", "Vui lòng chọn học phần để tìm thông tin");
            else
            {
                if(string.IsNullOrEmpty(CaThi))
                {
                    ModelState.AddModelError("", "Vui lòng chọn nhóm môn học để tìm thông tin");
                }
                else
                {
                    var fileName = DateTime.Now.ToLongTimeString() + ".xlsx";
                    using(ExcelPackage excelPackage = new ExcelPackage())
                    {
                        //create a WorkSheet
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                        var listDangKyThi = _context.DangKyThi.Where(m => m.Subject == Subject && m.CaThi == CaThi).ToList();
                        List<DangKyThiViewModel> list = new List<DangKyThiViewModel>();
                        for (var i = 0; i < listDangKyThi.Count; i++)
                        {
                            DangKyThiViewModel dktVM = new DangKyThiViewModel();
                            dktVM.ID = i + 1;
                            dktVM.StudentID = listDangKyThi[i].StudentID;
                            dktVM.FirstName = listDangKyThi[i].FirstName;
                            dktVM.LastName = listDangKyThi[i].LastName;
                            dktVM.SubjectGroup = listDangKyThi[i].SubjectGroup;
                            dktVM.CaThi = listDangKyThi[i].CaThi;
                            dktVM.Subject = listDangKyThi[i].Subject;
                            list.Add(dktVM);
                        }
                        if(Subject == "THVPNC")
                        {
                            worksheet.Cells["A1"].Value = "Thi kết thúc học phần: Tin học Văn phòng Nâng cao";
                            switch(CaThi){
                                case "Ca1" : worksheet.Cells["A2"].Value = "Ca 1 (Thời gian: 8:00 AM - 8:50AM)";
                                break;
                                case "Ca2" : worksheet.Cells["A2"].Value = "Ca 2 (Thời gian: 9:00 AM - 9:50AM)";
                                break;
                                case "Ca3" : worksheet.Cells["A2"].Value = "Ca 3 (Thời gian: 10:00 AM - 10:50AM)";
                                break;
                                case "Ca4" : worksheet.Cells["A2"].Value = "Ca 4 (Thời gian: 11:00 AM - 11:50AM)";
                                break;
                                case "Ca5" : worksheet.Cells["A2"].Value = "Ca 5 (Thời gian: 13:30 PM - 14:20PM)";
                                break;
                                case "Ca6" : worksheet.Cells["A2"].Value = "Ca 6 (Thời gian: 14:30 PM - 15:20PM)";
                                break;
                                case "Ca7" : worksheet.Cells["A2"].Value = "Ca 7 (Thời gian: 15:30 PM - 16:20PM)";
                                break;
                                case "Ca8" : worksheet.Cells["A2"].Value = "Ca 8 (Thời gian: 16:30 PM - 17:20PM)";
                                break;
                                default: break;
                            }
                        } else if(Subject == "QTDA")
                        {
                            worksheet.Cells["A1"].Value = "Thi kết thúc học phần: Quản trị Dự án Công nghệ Thông tin";
                            switch(CaThi){
                                case "Ca1" : worksheet.Cells["A2"].Value = "Ca 1 (Thời gian: 8:00 AM - 8:50AM)";
                                break;
                                case "Ca2" : worksheet.Cells["A2"].Value = "Ca 2 (Thời gian: 9:00 AM - 9:50AM)";
                                break;
                                case "Ca3" : worksheet.Cells["A2"].Value = "Ca 3 (Thời gian: 10:00 AM - 10:50AM)";
                                break;
                                case "Ca4" : worksheet.Cells["A2"].Value = "Ca 4 (Thời gian: 11:00 AM - 11:50AM)";
                                break;
                                default: break;
                            }
                        } else if(Subject == "TMDT")
                        {
                            worksheet.Cells["A1"].Value = "Thi kết thúc học phần: Quản trị Dự án Công nghệ Thông tin";
                            switch(CaThi){
                                case "Ca1" : worksheet.Cells["A2"].Value = "Ca 1 (Thời gian: 13:30 PM - 14:20PM)";
                                break;
                                case "Ca2" : worksheet.Cells["A2"].Value = "Ca 2 (Thời gian: 14:30 PM - 15:20PM)";
                                break;
                                case "Ca3" : worksheet.Cells["A2"].Value = "Ca 3 (Thời gian: 15:30 PM - 16:20PM)";
                                break;
                                case "Ca4" : worksheet.Cells["A2"].Value = "Ca 4 (Thời gian: 16:30 PM - 17:20PM)";
                                break;
                                default: break;
                            }
                        }
                        worksheet.Cells["A1:G1"].Merge = true;
                        worksheet.Cells["A2:G2"].Merge = true;

                        worksheet.Cells["A3"].Value = "STT";
                        worksheet.Cells["B3"].Value = "Mã Sinh viên";
                        worksheet.Cells["C3"].Value = "Họ tên";
                        worksheet.Cells["E3"].Value = "Nhóm môn học";
                        worksheet.Cells["F3"].Value = "Ca đăng ký thi";
                        worksheet.Cells["G3"].Value = "Ghi chú";
                        worksheet.Cells["C3:D3"].Merge = true;
                        worksheet.Cells["A3:G3"].Style.Font.Bold = true;
                        worksheet.Cells["A3:G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A4"].LoadFromCollection(list);
                        worksheet.Cells["A:G"].Style.Font.Size = 13;
                        worksheet.Cells["A:B"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["E:G"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Column(1).AutoFit();
                        worksheet.Column(2).AutoFit();
                        worksheet.Column(3).AutoFit();
                        worksheet.Column(4).AutoFit();
                        worksheet.Column(5).AutoFit();
                        worksheet.Column(6).AutoFit();
                        worksheet.Column(7).AutoFit();

                        string modelRange = "A1:G" + (list.Count() + 3);
                        var modelTable = worksheet.Cells[modelRange];

                        // Assign borders
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        var stream = new MemoryStream(excelPackage.GetAsByteArray()); //Get updated stream
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            return RedirectToAction("Index");
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
        
        public async Task<IActionResult> CaThi()
        {
            return View(await _context.CaThi.ToListAsync());
        }
        public async Task<IActionResult> EditCaThi(int? id)
        {
            if (id == null || _context.CaThi == null)
            {
                return NotFound();
            }

            var caThi = await _context.CaThi.FindAsync(id);
            if (caThi == null)
            {
                return NotFound();
            }
            return View(caThi);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCaThi(int id, string SecurityKey, [Bind("CaThiID,CaThiName,MaxValue,RegistedValue,Subject")] CaThi caThi)
        {
            if (id != caThi.CaThiID)
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
                    _context.Update(caThi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaThiExists(caThi.CaThiID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CaThi));
            }
            return View(caThi);
        }
        private bool DangKyThiExists(string id)
        {
          return (_context.DangKyThi?.Any(e => e.StudentID == id)).GetValueOrDefault();
        }
        private bool CaThiExists(int id)
        {
          return (_context.CaThi?.Any(e => e.CaThiID == id)).GetValueOrDefault();
        }
    }
}
