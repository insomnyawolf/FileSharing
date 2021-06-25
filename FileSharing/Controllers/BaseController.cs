using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Capitales.Controllers
{
    // Generic Processing Goes Here
    public abstract class BaseController : ControllerBase
    {
        internal ILogger Logger;

        protected BaseController(ILogger Logger)
        {
            this.Logger = Logger;
        }
    }
}