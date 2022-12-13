using Microsoft.AspNetCore.Mvc;
using JavaExamFinal.Data;

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
                    else if(dkSuccess.Subject == "QTDA") message += "Quản trị Dự án CNTT - " + dkSuccess.CaThi;
                }
                
            }
            ViewBag.thongBao = message;
            return View();
        }
    }
}