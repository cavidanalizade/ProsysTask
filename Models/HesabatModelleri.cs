namespace ImtahanProqrami.Models
{
    public class DersOrtaBal
    {
        public string DersAdi { get; set; } = "";
        public int ImtahanSayi { get; set; }
        public decimal? OrtaBal { get; set; }
    }

    public class SagirdOrtaBal
    {
        public int Nomre { get; set; }
        public string Adi { get; set; } = "";
        public string Soyadi { get; set; } = "";
        public short Sinif { get; set; }
        public int ImtahanSayi { get; set; }
        public decimal? OrtaBal { get; set; }
    }

    //hesabat sehifesinde 4 cedvel var, hamisini bir modelde gonderirem
    public class HesabatGorunusu
    {
        public List<DersOrtaBal> DersUzre { get; set; } = new List<DersOrtaBal>();
        public List<SagirdOrtaBal> SagirdUzre { get; set; } = new List<SagirdOrtaBal>();
        public List<Imtahan> Kesirler { get; set; } = new List<Imtahan>();
        public List<Sagird> Imtahansizlar { get; set; } = new List<Sagird>();
    }
}
