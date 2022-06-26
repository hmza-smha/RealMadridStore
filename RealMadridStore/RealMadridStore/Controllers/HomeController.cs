using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealMadridStore.Data;

namespace RealMadridStore.Controllers
{
    public class HomeController : Controller
    {
        private RealMadridDBContext _db;

        public HomeController(RealMadridDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.categories.Include(x => x.products).ToList());
        }
    }
}
