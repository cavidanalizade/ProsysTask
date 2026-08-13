using ImtahanProqrami.Models;
using Microsoft.EntityFrameworkCore;

namespace ImtahanProqrami.Data
{
    public class ImtahanContext : DbContext
    {
        public ImtahanContext(DbContextOptions<ImtahanContext> options) : base(options)
        {
        }

        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Sagird> Sagirdler { get; set; }
        public DbSet<Imtahan> Imtahanlar { get; set; }

        //cedveller SQL skripti ile yaradilib, burda sadece uygunlasdirilir
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Ders>(e =>
            {
                e.ToTable("Dersler");
                e.HasKey(x => x.DersKodu);
                e.Property(x => x.DersKodu).HasColumnType("char(3)");
                e.Property(x => x.DersAdi).HasColumnType("varchar(30)").IsRequired();
                e.Property(x => x.Sinif).HasColumnType("numeric(2,0)").HasConversion<decimal>();
                e.Property(x => x.MuellimAdi).HasColumnType("varchar(20)").IsRequired();
                e.Property(x => x.MuellimSoyadi).HasColumnType("varchar(20)").IsRequired();
            });

            model.Entity<Sagird>(e =>
            {
                e.ToTable("Sagirdler");
                e.HasKey(x => x.Nomre);

                //nomreni ozumuz veririk, identity deyil
                e.Property(x => x.Nomre).HasColumnType("numeric(5,0)")
                    .HasConversion<decimal>()
                    .ValueGeneratedNever();

                e.Property(x => x.Adi).HasColumnType("varchar(30)").IsRequired();
                e.Property(x => x.Soyadi).HasColumnType("varchar(30)").IsRequired();
                e.Property(x => x.Sinif).HasColumnType("numeric(2,0)").HasConversion<decimal>();
                e.Ignore(x => x.TamAd);
            });

            model.Entity<Imtahan>(e =>
            {
                e.ToTable("Imtahanlar");
                e.HasKey(x => new { x.DersKodu, x.SagirdNomresi, x.ImtahanTarixi });

                e.Property(x => x.DersKodu).HasColumnType("char(3)");
                e.Property(x => x.SagirdNomresi).HasColumnType("numeric(5,0)").HasConversion<decimal>();
                e.Property(x => x.ImtahanTarixi).HasColumnType("date");
                e.Property(x => x.Qiymet).HasColumnType("numeric(1,0)").HasConversion<decimal?>();

                //Restrict - ders ve ya sagird silinende neticeler ozbasina silinmesin
                e.HasOne(x => x.Ders).WithMany(d => d.Imtahanlar)
                    .HasForeignKey(x => x.DersKodu)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Sagird).WithMany(s => s.Imtahanlar)
                    .HasForeignKey(x => x.SagirdNomresi)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
