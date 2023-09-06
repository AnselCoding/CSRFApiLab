using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSRFApiLab.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        //改用全域的 CSRF 設定
        //[ValidateAntiForgeryToken]
        // POST: api/HomeApi/ApiCSRF
        [HttpPost]
        public ActionResult ApiCSRF([FromBody] string txt)
        {
            return Ok(txt);
        }

    }
}
