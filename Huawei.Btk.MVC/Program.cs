using Huawei.Btk.Application.Services.EmailServices;
using Huawei.Btk.Application.Services.UserServices;
using Huawei.Btk.Core.Context;
using Huawei.Btk.Core.Models;
using Huawei.Btk.MVC.Localization;
using Huawei.Btk.MVC.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string mySqlConnectionStr = builder.Configuration.GetConnectionString("MySql")!;
builder.Services.AddDbContext<ApplicationDbContext>(
	u => u.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr),
	options => options.EnableRetryOnFailure(
		maxRetryCount: 5,
		maxRetryDelay: TimeSpan.FromSeconds(30),
		errorNumbersToAdd: null
	)));

builder.Services.AddIdentity<User, Role>(options =>
{
	options.User.RequireUniqueEmail = true;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Password.RequiredLength = 6;
	options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders()
	.AddErrorDescriber<LocalizationIdentityErrorDescriber>();

builder.Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.LoginPath = "/Account/Login";
	options.LogoutPath = "/Account/Logout";
	options.ExpireTimeSpan = TimeSpan.FromHours(1);
	options.Cookie.Name = "BtkHuaweiAuth";
});

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();

// Email Configuration
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddControllersWithViews();

builder.Services.Configure<ServerInformation>(builder.Configuration.GetSection("ServerInformation"));

var app = builder.Build();

// Configure the HTTP request pipeline.
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
