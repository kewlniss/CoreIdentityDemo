using CoreIdentityDemo.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

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
    }
}