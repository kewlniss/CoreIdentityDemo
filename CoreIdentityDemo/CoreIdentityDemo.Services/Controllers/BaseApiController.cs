using CoreIdentityDemo.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace CoreIdentityDemo.Services.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private IdentityContext _context;

        protected IdentityContext Context
        {
            get { return _context ?? (_context = new IdentityContext()); }
        }

        protected override void Dispose(bool disposing)
        {
            if (_context != null)
                _context.Dispose();
            base.Dispose(disposing);
        }

        protected override ExceptionResult InternalServerError(Exception exception)
        {
            var result = default(ExceptionResult);
#if (DEBUG)
            result = base.InternalServerError(exception);
#else
            result = base.InternalServerError();
#endif
            return result;
        }
    }
}