using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImtahanProqrami.Models
{
    public class Ders
    {
        private string _dersKodu = "";

        [Display(Name = "Dersin kodu")]
        [Required(ErrorMessage = "Dersin kodu bos ola bilmez.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Kod 3 simvol olmalidir.")]
        public string DersKodu
        {
            get { return _dersKodu; }
            //kodu her yerde eyni saxlamaq ucun burda temizleyirem,
            //yoxsa " ri" ile "RIY" ayri kod kimi dusur
            set { _dersKodu = (value ?? "").Trim().ToUpper(); }
        }

        [Display(Name = "Dersin adi")]
        [Required(ErrorMessage = "Dersin adi bos ola bilmez.")]
        [StringLength(30)]
        public string DersAdi { get; set; } = "";

        [Display(Name = "Sinif")]
        [Range(1, 11, ErrorMessage = "Sinif 1 ile 11 arasinda olmalidir.")]
        public short Sinif { get; set; }

        [Display(Name = "Muellimin adi")]
        [Required(ErrorMessage = "Muellimin adi bos ola bilmez.")]
        [StringLength(20)]
        public string MuellimAdi { get; set; } = "";

        [Display(Name = "Muellimin soyadi")]
        [Required(ErrorMessage = "Muellimin soyadi bos ola bilmez.")]
        [StringLength(20)]
        public string MuellimSoyadi { get; set; } = "";

        [ValidateNever]
        public List<Imtahan> Imtahanlar { get; set; } = new List<Imtahan>();
    }
}
