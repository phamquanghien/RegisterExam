using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JavaExamFinal.Models;
using JavaExamFinal.Data;
namespace JavaExamFinal.Controllers;

public class ThuongMaiDienTuController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ThuongMaiDienTuController> _logger;
    private string subGroup = "10,11,12,13,14,15,16,17,18,19,20,21,22";
    private string ct =  "Ca1, Ca2, Ca3, Ca4, Ca5, Ca6, Ca7, Ca8";

    public ThuongMaiDienTuController(ILogger<ThuongMaiDienTuController> logger, ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        DisplayNumberRegisted();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(string studentID, string studentName, string subjectGroup, string caThi)
    {
        if(String.IsNullOrEmpty(studentID))
        {
            ModelState.AddModelError("", "Mã sinh viên không được để trống");
        }
        else if(String.IsNullOrEmpty(studentName))
        {
            ModelState.AddModelError("", "Tên sinh viên không được để trống");
        }
        else if(String.IsNullOrEmpty(subjectGroup))
        {
            ModelState.AddModelError("", "Nhóm môn học không được để trống");
        }
        else if(String.IsNullOrEmpty(caThi))
        {
            ModelState.AddModelError("", "Vui lòng chọn ca thi");
        }
        else{
            studentID = studentID.Trim();
            studentName = studentName.Trim();
            subjectGroup = subjectGroup.Trim();
            caThi = caThi.Trim();
        }
        var std = _context.Student.Where(m => m.StudentID == studentID && m.Subject == "TMDT").FirstOrDefault();
        if(std == null){
            ModelState.AddModelError("", "Mã sinh viên không chính xác");
        }
        else{
            if(std.IsActive == false){
                ModelState.AddModelError("", "Sinh viên bị cấm thi, vui lòng liên hệ với Giảng viên.");
            }
            else if(String.Compare(LocDau(std.FullName.Trim()),LocDau(studentName), true)!=0){
                ModelState.AddModelError("", "Họ tên sinh viên không chính xác.");
            }
            else if(subGroup.IndexOf(subjectGroup)<0 || String.Compare(std.SubjectGroup, subjectGroup,true)!=0 || subjectGroup.Length != 3){
                ModelState.AddModelError("", "Nhóm môn học không chính xác.");
            }
            else if(ct.IndexOf(caThi)<0 || caThi.Length != 3){
                ModelState.AddModelError("", "Ca thi không chính xác." + caThi + "-" + ct.IndexOf(caThi) + "-" + caThi.Length);
            }
            else{
                var stdRegisted = _context.DangKyThi.Where(m => m.StudentID == studentID && m.Subject == "TMDT").FirstOrDefault();
                if(stdRegisted != null) {
                    ModelState.AddModelError("", "Sinh viên đã đăng ký ca thi");
                }
                else{
                    //kiem tra xem so luong dang ky da full chua
                    var checkCaThi = _context.CaThi.Where(m => m.CaThiName == caThi && m.Subject == "TMDT").FirstOrDefault();
                    // neu chua co du lieu trong bang ca thi thi can them vao
                    if(checkCaThi== null)
                    {
                        if(RegisterStudent(studentID, std.FirstName, std.LastName, subjectGroup, caThi)==true)
                        {
                            CaThi ctnew = new CaThi();
                            ctnew.CaThiName = caThi;
                            ctnew.MaxValue = 80;
                            ctnew.RegistedValue = 1;
                            ctnew.Subject = "TMDT";
                            _context.CaThi.Add(ctnew);
                            await _context.SaveChangesAsync();
                            ViewBag.thongBao = "Đăng ký thi thành công";
                        }
                        else{
                            ModelState.AddModelError("", "Đăng ký thi thất bại. Vui lòng liên hệ với Giảng viên để được hỗ trợ.");
                        }
                    }else{
                        if(checkCaThi.MaxValue<=checkCaThi.RegistedValue){
                            ModelState.AddModelError("","Ca thi đã đủ sinh viên, vui lòng chọn ca thi khác");
                        }else{
                            if(RegisterStudent(studentID, std.FirstName, std.LastName, subjectGroup, caThi)==true){
                                var checkIDCaThi = checkCaThi.CaThiID;
                                var cathiUpdate = _context.CaThi.Find(checkIDCaThi);
                                cathiUpdate.RegistedValue = cathiUpdate.RegistedValue + 1;
                                _context.Update(cathiUpdate);
                                await _context.SaveChangesAsync();
                                ViewBag.thongBao = "Đăng ký thi thành công";
                            }
                            else{
                                ModelState.AddModelError("", "Đăng ký thi thất bại. Vui lòng liên hệ với Giảng viên để được hỗ trợ.");
                            }
                        }
                    }
                }
            }
        }
        DisplayNumberRegisted();
        return View();
    }
    private bool RegisterStudent(string id, string fname, string lname, string sgroup, string cthi)
    {
        try
        {
            DangKyThi dkt = new DangKyThi();
            dkt.StudentID = id;
            dkt.FirstName = fname;
            dkt.LastName = lname;
            dkt.FullName = fname + " " + lname;
            dkt.SubjectGroup = sgroup;
            dkt.IsActive = true;
            dkt.CaThi = cthi;
            dkt.Subject = "TMDT";
            _context.DangKyThi.Add(dkt);
            _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static readonly string[] VietNamChar = new string[] 
    { 
        "aAeEoOuUiIdDyY", 
        "áàạảãâấầậẩẫăắằặẳẵ", 
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", 
        "éèẹẻẽêếềệểễ", 
        "ÉÈẸẺẼÊẾỀỆỂỄ", 
        "óòọỏõôốồộổỗơớờợởỡ", 
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", 
        "úùụủũưứừựửữ", 
        "ÚÙỤỦŨƯỨỪỰỬỮ", 
        "íìịỉĩ", 
        "ÍÌỊỈĨ", 
        "đ", 
        "Đ", 
        "ýỳỵỷỹ", 
        "ÝỲỴỶỸ" 
    };
    public static string LocDau(string str)    
    {   
        //Thay thế và lọc dấu từng char      
        for (int i = 1; i < VietNamChar.Length; i++)        
        {
            for (int j = 0; j < VietNamChar[i].Length; j++)
                str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);        
        }        
        return str;    
    }
    private void DisplayNumberRegisted(){
        var cathiList = _context.CaThi.OrderBy(m => m.CaThiName).Where(m => m.Subject == "TMDT").ToList();
        if(cathiList.Count > 0){
            var ca1 = cathiList.Where(m => m.CaThiName == "Ca1").FirstOrDefault();
            if(ca1!=null){
                ViewBag.ca1 = ca1.RegistedValue + "/" +ca1.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca1 = "Chưa có Sinh Viên đăng ký";
            }
            var ca2 = cathiList.Where(m => m.CaThiName == "Ca2").FirstOrDefault();
            if(ca2!=null){
                ViewBag.ca2 = ca2.RegistedValue + "/" +ca2.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca2 = "Chưa có Sinh Viên đăng ký";
            }
            var ca3 = cathiList.Where(m => m.CaThiName == "Ca3").FirstOrDefault();
            if(ca3!=null){
                ViewBag.ca3 = ca3.RegistedValue + "/" +ca3.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca3 = "Chưa có Sinh Viên đăng ký";
            }
            var ca4 = cathiList.Where(m => m.CaThiName == "Ca4").FirstOrDefault();
            if(ca4!=null){
                ViewBag.ca4 = ca4.RegistedValue + "/" +ca4.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca4 = "Chưa có Sinh Viên đăng ký";
            }var ca5 = cathiList.Where(m => m.CaThiName == "Ca5").FirstOrDefault();
            if(ca5!=null){
                ViewBag.ca5 = ca5.RegistedValue + "/" +ca5.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca5 = "Chưa có Sinh Viên đăng ký";
            }
            var ca6 = cathiList.Where(m => m.CaThiName == "Ca6").FirstOrDefault();
            if(ca6!=null){
                ViewBag.ca6 = ca6.RegistedValue + "/" +ca6.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca6 = "Chưa có Sinh Viên đăng ký";
            }
            var ca7 = cathiList.Where(m => m.CaThiName == "Ca7").FirstOrDefault();
            if(ca7!=null){
                ViewBag.ca7 = ca7.RegistedValue + "/" +ca7.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca7 = "Chưa có Sinh Viên đăng ký";
            }
            var ca8 = cathiList.Where(m => m.CaThiName == "Ca8").FirstOrDefault();
            if(ca8!=null){
                ViewBag.ca8 = ca8.RegistedValue + "/" +ca8.MaxValue + " SV đã đăng ký";
            }
            else{
                ViewBag.ca8 = "Chưa có Sinh Viên đăng ký";
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
