using System.ComponentModel.DataAnnotations;

namespace JavaExamFinal.Models
{
    public class CaThi{
        [Key]
        public int CaThiID { get; set; }
        public string CaThiName { get; set; }
        public int MaxValue { get; set; }
        public int RegistedValue { get; set; }
        public string Subject { get; set; }
    }
}