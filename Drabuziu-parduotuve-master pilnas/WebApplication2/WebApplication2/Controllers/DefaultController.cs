using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        private DatabaseContext db = new DatabaseContext();

    }
}