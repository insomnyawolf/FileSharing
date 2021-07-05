using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AdminController : BaseController
    {
        public AdminController(ILogger<FileController> Logger) : base(Logger)
        {
        }

        // Change congig

        // Clear Cache
    }
}
