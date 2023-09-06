using CSRFApiLab.Middleware;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // �t�m���쪺 AutoValidateAntiforgeryTokenAttribute Filter
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// For prevent XSS. Limit cookie access.
builder.Services.AddAuthentication().AddCookie(c => c.Cookie.HttpOnly = true);

builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(options =>
{
    // �t�mAntiforgery�ﶵ�A���U�e�ݨӪ�request���A�n�d����� Header�A������ CSRF token
    options.HeaderName = "X-CSRF-TOKEN"; // �۩w�q���d�аO��Header�W��
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

// �[�J�ۭq�� CSRF Middleware
app.UseMyCSRF();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
