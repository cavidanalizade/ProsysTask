using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImtahanProqrami.Models
{
    public class Imtahan
    {
        private string _dersKodu = "";

        [Display(Name = "Ders")]
        [Required(ErrorMessage = "Ders secilmelidir.")]
        [StringLength(3, MinimumLength = 3)]
        public string DersKodu
        {
            get { return _dersKodu; }
            set { _dersKodu = (value ?? "").Trim().ToUpper(); }
        }

        [Display(Name = "Sagird")]
        [Range(1, 99999, ErrorMessage = "Sagird secilmelidir.")]
        public int SagirdNomresi { get; set; }

        [Display(Name = "Imtahan tarixi")]
        [DataType(DataType.Date)]
        public DateOnly ImtahanTarixi { get; set; }

        //imtahan yazilanda qiymet hele bilinmeye biler, ona gore null-a icaze verilir
        [Display(Name = "Qiymet")]
        [Range(2, 5, ErrorMessage = "Qiymet 2 ile 5 arasinda olmalidir.")]
        public short? Qiymet { get; set; }

        [ValidateNever]
        public Ders? Ders { get; set; }

        [ValidateNever]
        public Sagird? Sagird { get; set; }
    }
}
