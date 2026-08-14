using ImtahanProqrami.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//baza appsettings.json-daki connection string ile baglanir
builder.Services.AddDbContext<ImtahanContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ImtahanDB")));

builder.Services.AddControllersWithViews(options =>
{
    //bos select ve ya bos tarix gonderilende ingilisce mesaj cixirdi
    var mesajlar = options.ModelBindingMessageProvider;
    mesajlar.SetAttemptedValueIsInvalidAccessor((deyer, sahe) => "Duzgun deyer secin.");
    mesajlar.SetValueIsInvalidAccessor(deyer => "Duzgun deyer secin.");
    mesajlar.SetValueMustNotBeNullAccessor(deyer => "Bu sahe bos ola bilmez.");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
