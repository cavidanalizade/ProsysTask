# Imtahan proqrami

Ev tapsirigi 1. Orta mektebde sagirdlerin imtahan neticelerinin qeydiyyati ucun
sade veb proqramdir.

Istifade etdiyim texnologiyalar: C#, ASP.NET Core MVC, Entity Framework Core,
MS SQL Server, Bootstrap.

## Bazani qurmaq

Evvelce `Imtahan_Proqrami.sql` faylini SSMS-de acib icra etmek lazimdir, cedveller
ve numune melumat orda yaranir.

Sonra `ImtahanProqrami/appsettings.json` faylindaki connection string-e baxin, oz
serverinize gore deyisdirin. Mende LocalDB var, ona gore `(localdb)\MSSQLLocalDB`
yazilib. Serverin adini bilmirsinizse SSMS-i acin, giris pencresinde Server name
nedirse onu yazin.

    tam SQL Server        Server=.
    SQL Server Express    Server=.\SQLEXPRESS
    LocalDB               Server=(localdb)\MSSQLLocalDB

Noqte "bu komputerdeki adsiz instance" demekdir, ona gore LocalDB-ni tapmir -
LocalDB xidmet kimi islemir, ayrica bele yazilmalidir.

## Isletmek

    dotnet run

Brauzerde: http://localhost:5014

## Sehifeler

Dersler, Sagirdler, Imtahanlar ve Hesabatlar bolmeleri var.

Imtahan yazilanda qiymeti bos saxlamaq olar, sonradan qoyulur. Imtahan neticesi
olan dersi ve ya sagirdi silmek olmur, xeberdarliq verilir.

Hesabatlar sehifesinde ders ve sagird uzre orta ballar, kesir alanlar ve hele
imtahan vermeyen sagirdler gosterilir.
