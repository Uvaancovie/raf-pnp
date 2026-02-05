using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using Sentry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Enable API controllers

// Add Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Task Management Services
builder.Services.AddScoped<raf_pnp.Services.ITeamService, raf_pnp.Services.TeamService>();
builder.Services.AddScoped<raf_pnp.Services.ITaskService, raf_pnp.Services.TaskService>();
builder.Services.AddScoped<raf_pnp.Services.INotificationService, raf_pnp.Services.NotificationService>();

// Register WhatsApp Service (Mock or Twilio based on configuration)
if (builder.Configuration.GetValue<bool>("UseWhatsAppSimulator", true))
{
    builder.Services.AddScoped<raf_pnp.Services.IWhatsAppService, raf_pnp.Services.MockWhatsAppService>();
}
else
{
    builder.Services.AddScoped<raf_pnp.Services.IWhatsAppService, raf_pnp.Services.TwilioWhatsAppService>();
}

// Add Sentry
builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://b2b496a68fb691efedf3ddaae5d7aea5@o4510794040672256.ingest.us.sentry.io/4510794042114048";
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
});

var app = builder.Build();

// Seed the database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

// Test Sentry integration
SentrySdk.CaptureMessage("Hello Sentry - RAF PNP Application Started");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Map API controller routes

app.Run();
