using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImtahanProqrami.Models
{
    public class Imtahan
    {
        private string _dersKodu = "";

        //burda StringLength qoymuram - ders siyahidan secilir, jquery validation
        //select-de uzunlugu yox, secilmis option sayini olcur ve hemise xeta verir
        [Display(Name = "Ders")]
        [Required(ErrorMessage = "Ders secilmelidir.")]
        public string DersKodu
        {
            get { return _dersKodu; }
            set { _dersKodu = (value ?? "").Trim().ToUpper(); }
        }

        [Display(Name = "Sagird")]
        [Required(ErrorMessage = "Sagird secilmelidir.")]
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
