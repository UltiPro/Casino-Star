var  CasinoStarPolicy = "CasinoStarPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddCors(setup => { //to
    setup.AddPolicy(name: CasinoStarPolicy, policy => {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(
            "https://localhost:44447",
            "http://localhost:44447",
            "https://localhost:5050",
            "http://localhost:5000"
        );
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(CasinoStarPolicy); //to

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
