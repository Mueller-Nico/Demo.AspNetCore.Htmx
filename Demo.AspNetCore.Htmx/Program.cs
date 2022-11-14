using Demo.AspNetCore.Htmx.Data;
using Demo.AspNetCore.Htmx.Extensions;
using Demo.AspNetCore.Htmx.Services;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    //options.Cookie.Domain = "contoso.com";
    options.Cookie.Path = "/";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.IsEssential = true;
});


// Lib.AspNetCore.ServerSentEvents by Tomasz Peczek is used for sse.
// see https://github.com/tpeczek/Lib.AspNetCore.ServerSentEvents
// and https://tpeczek.github.io/Lib.AspNetCore.ServerSentEvents/articles/getting-started.html

builder.Services.AddServerSentEvents();

builder.Services.AddSingleton<IHostedService, SseHeartbeatService>();

builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
});


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

app.Use(async (context, next) =>
{
    if (context.Request.IsHtmxRequest())
    {
        // Htmx browser history support is still a bit buggy
        // https://github.com/bigskysoftware/htmx/issues/497
        // workaround:
        context.Response.Headers.Add("Cache-Control", "no-store,no-cache,max-age=0");
    }

    await next();

    if (context.Response.StatusCode == 404 && context.Request.IsHtmxRequest() == false)
    {
        context.Request.Path = "/NotFound";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapServerSentEvents("/sse-heartbeat");

app.Run();
