using macrix_wk_backend.Data;
using macrix_wk_backend.Interfaces;
using macrix_wk_backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(Directory.GetCurrentDirectory(), "db"));
//Registering services
builder.Services.AddScoped<IPersonService, PersonService>();
var dataDirectory = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "macrixdb.mdf");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection").Replace("|DataDirectory|", dataDirectory)));

builder.Services.AddRazorPages();

//Adding authorization with setting policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();