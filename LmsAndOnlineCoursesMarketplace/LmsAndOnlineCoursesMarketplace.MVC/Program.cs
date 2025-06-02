using LmsAndOnlineCoursesMarketplace.Application.Extensions;
using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using LmsAndOnlineCoursesMarketplace.Infrastructure.Extensions;
using LmsAndOnlineCoursesMarketplace.Infrastructure.Services;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using LmsAndOnlineCoursesMarketplace.Persistence.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; 
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<IEmailSender>(provider =>
    new SmtpEmailSender(
        smtpServer: "smtp.yandex.com",
        smtpPort: 465,
        smtpUsername: "nmefimov@kpfu.ru",
        smtpPassword: "wmhjkrpywcrvzege"
    ));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();