using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImtahanProqrami.Models
{
    public class Sagird
    {
        [Display(Name = "Nomresi")]
        [Range(1, 99999, ErrorMessage = "Nomre 1 ile 99999 arasinda olmalidir.")]
        public int Nomre { get; set; }

        [Display(Name = "Adi")]
        [Required(ErrorMessage = "Ad bos ola bilmez.")]
        [StringLength(30)]
        public string Adi { get; set; } = "";

        [Display(Name = "Soyadi")]
        [Required(ErrorMessage = "Soyad bos ola bilmez.")]
        [StringLength(30)]
        public string Soyadi { get; set; } = "";

        [Display(Name = "Sinif")]
        [Range(1, 11, ErrorMessage = "Sinif 1 ile 11 arasinda olmalidir.")]
        public short Sinif { get; set; }

        [ValidateNever]
        public List<Imtahan> Imtahanlar { get; set; } = new List<Imtahan>();

        //cedvellerde ad ve soyadi bir yerde yazmaq ucun
        public string TamAd
        {
            get { return Adi + " " + Soyadi; }
        }
    }
}
