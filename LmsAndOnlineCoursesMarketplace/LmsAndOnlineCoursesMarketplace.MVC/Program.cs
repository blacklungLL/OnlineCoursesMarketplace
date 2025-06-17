using LmsAndOnlineCoursesMarketplace.Application.Extensions;
using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using LmsAndOnlineCoursesMarketplace.Application.S3;
using LmsAndOnlineCoursesMarketplace.Infrastructure.Extensions;
using LmsAndOnlineCoursesMarketplace.Infrastructure.Services;
using LmsAndOnlineCoursesMarketplace.MVC.Hubs;
using LmsAndOnlineCoursesMarketplace.MVC.Middleware;
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

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3"));
builder.Services.AddSingleton<IS3Service, S3Service>();


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
builder.Services.AddSignalR();
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
app.UseStaticFiles(); 
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    await next();
    
    if (context.Response.StatusCode == 404 && !context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.Redirect("/errors/404.html");
    }
});

app.UseMiddleware<CustomAccessDeniedMiddleware>();

app.MapStaticAssets();

app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller}/{action}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var s3Service = scope.ServiceProvider.GetRequiredService<IS3Service>();
    
    await s3Service.EnsureBucketExistsAsync();
}

app.Run();