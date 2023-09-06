using Microsoft.AspNetCore.Antiforgery;

namespace CSRFApiLab.Middleware
{
    public static class CSRFMiddlewareExtensions
    {
        public static void UseMyCSRF(this IApplicationBuilder app)
        {
            app.UseMiddleware<CSRFMiddleware>();
        }
    }
    public class CSRFMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public CSRFMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }
        public async Task Invoke(HttpContext context)
        {
            // 在處理請求之前執行操作
            string path = context.Request.Path.Value;

            if (
                string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase))
            {
                var tokens = _antiforgery.GetAndStoreTokens(context);
                // 這行程式碼會將Cookie設置到前端瀏覽器中
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                    new CookieOptions() { HttpOnly = false });
            }

            // 調用下一個 Middleware 或終端 Middleware
            await _next(context);
        }
    }

}
