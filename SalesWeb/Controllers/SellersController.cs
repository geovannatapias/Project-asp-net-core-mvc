using Microsoft.AspNetCore.Mvc;
using SalesWeb.Models;
using SalesWeb.Services;


namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        
        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindaAll(); //retorna lista de seller
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] //acao post
        [ValidateAntiForgeryToken] //protection

        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction("Index");
        }
    }
}
