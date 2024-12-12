using Repository;
using Repository.IRepository;
using DAO.Data;
using DAO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repository.Helpers;
using Microsoft.AspNetCore.Authentication.Google;
using Repository.PaymentService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Shoopi1Context>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<OrderDAO>();

//authentication dung cookie
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;  // Thêm dòng này
})
    .AddCookie(options =>
    {
        options.Cookie = new CookieBuilder()
        {
            Name = "SHOOPI"
        };
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
        options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
    });



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100000000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//vnpay service
builder.Services.AddSingleton<IVnPayService, VnPayService>();


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

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
