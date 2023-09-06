using CSRFApiLab.Middleware;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // 配置全域的 AutoValidateAntiforgeryTokenAttribute Filter
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// For prevent XSS. Limit cookie access.
builder.Services.AddAuthentication().AddCookie(c => c.Cookie.HttpOnly = true);

builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(options =>
{
    // 配置Antiforgery選項，註冊前端來的request中，要查找哪個 Header，來驗證 CSRF token
    options.HeaderName = "X-CSRF-TOKEN"; // 自定義防範標記的Header名稱
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 加入自訂的 CSRF Middleware
app.UseMyCSRF();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
