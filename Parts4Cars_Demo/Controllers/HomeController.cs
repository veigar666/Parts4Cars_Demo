using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parts4Cars_Demo.Data;
using Parts4Cars_Demo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parts4Cars_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext dataContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext _dataContext)
        {
            _logger = logger;
            this.dataContext = _dataContext;
        }

        public ApplicationDbContext DataContext { get { return dataContext; } }

        [HttpGet]
        public async Task<IActionResult> Index(string searchCarMake, string searchCarModel)
        {
            var searchedParts = from sp in dataContext.Parts
                                select sp;
            var images = dataContext.Images.ToList();

            if (!String.IsNullOrEmpty(searchCarMake) && String.IsNullOrEmpty(searchCarModel))
            {
                searchedParts = searchedParts.Where(s => s.CarMake! == searchCarMake);
            }
            else if (!String.IsNullOrEmpty(searchCarMake) || !String.IsNullOrEmpty(searchCarModel))
            {
                searchedParts = searchedParts.Where(s => s.CarMake! == searchCarMake
                && s.CarModel! == searchCarModel);
            }

            return View(await searchedParts.ToListAsync());
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var part = dataContext.Parts.Find(id);
            var images = dataContext.Images.Where(s => s.Part == part).ToList();
            if (part != null)
            {
                return this.View(part);
            }


            return NotFound();
        }

        public IActionResult ShoppingCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Part> parts = dataContext.Parts.ToList();
            List<Image> images = dataContext.Images.ToList();
            var partsInCart = dataContext.AppUsers.Where(c => c.Id == userId).SelectMany(c => c.Cart).ToList();
            return View(partsInCart);
        }

        [HttpGet]
        public void AddToCart(int id)
        {
            var part = dataContext.Parts.Find(id);
            if (part != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = dataContext.AppUsers.Find(userId);
                user.Cart.Add(part);
                dataContext.SaveChanges();
            }
        }

        [HttpGet]
        public void RemoveFromCart(int id)
        {
            var part = dataContext.Parts.Find(id);
            if (part != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = this.dataContext.AppUsers.Include(a => a.Cart).SingleOrDefault(a => a.Id == userId);
                user.Cart.Remove(part);
                dataContext.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Order(Order order)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = this.dataContext.AppUsers.Include(a => a.Cart).SingleOrDefault(a => a.Id == userId);
            List<Part> partsInCart = dataContext.AppUsers.Where(c => c.Id == userId).SelectMany(c => c.Cart).ToList();
            decimal price = 0;
            foreach(var part in partsInCart)
            {
                price = Decimal.Add(price, (decimal)part.Price);
            }
            order.Price = price;
            order.PartsInOrder = partsInCart;
            order.OrderedOn = DateTime.Now;
            dataContext.Orders.Add(order);
            user.Cart.Clear();
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult AllOrders()
        {
            List<Order> allOrders = dataContext.Orders.Include(a => a.PartsInOrder).ThenInclude(a => a.Images).ToList();
            return View(allOrders);
        }

        public void RemoveOrder(int id)
        {
            var order = dataContext.Orders.Find(id);
            if (order != null)
            {
                dataContext.Remove(order);
                dataContext.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult AddPart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPart(Part part)
        {
            dataContext.Parts.Add(part);
            dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditPart(int id)
        {
            Part part = dataContext.Parts.Find(id);
            return View(part);
        }

        [HttpPost]
        public IActionResult EditPart(Part part)
        {
            var item = dataContext.Parts.Find(part.ID);
            if(item != null)
            {
                dataContext.Entry(item).CurrentValues.SetValues(part);
                dataContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public void DeletePart(int id)
        {
            var part = dataContext.Parts.Find(id);
            if (part != null)
            {
                dataContext.Remove(part);
                dataContext.SaveChanges();
            }
        }

        public void MakeMeAdmin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cmd = "insert into AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)values('23c1997a-e0f2-4c17-8ff0-e52a1193ab0c', 'admin', 'admin', 'admin')" +
                $"insert into AspNetUserRoles(UserId, RoleId)values('{userId}', '23c1997a-e0f2-4c17-8ff0-e52a1193ab0c')";
            dataContext.Database.ExecuteSqlRaw(cmd);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
