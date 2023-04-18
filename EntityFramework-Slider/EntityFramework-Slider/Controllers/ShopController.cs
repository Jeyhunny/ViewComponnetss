using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EntityFramework_Slider.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {


            
            int count = await _context.Products.Where(m => !m.SoftDelete).CountAsync();
            ViewBag.Count = count;
                                                   
            IEnumerable<Product> products =await _context.Products
                                                  .Include(m =>m.Images)
                                                   .Where(m => !m.SoftDelete)
                                                   //.OrderByDescending(m => m.Id)  
                                                   .Take(4)  
                                                   //.Skip(8)    

                                                   .ToListAsync();
            return View(products);
        }

       
        public async Task<IActionResult> LoadMore(int skip)
        {
            IEnumerable<Product> products = await _context.Products
                                                  .Include(m => m.Category)
                                                  .Include(m => m.Images)
                                                   .Where(m => !m.SoftDelete)
                                                   .Skip(skip)
                                                    .Take(4)
                                                   .ToListAsync();
            
            return PartialView("_ProductsPartial",products);
        }

        //method -Search
        public IActionResult Search(string searchText)
        {
            var products = _context.Products.Include(m=>m.Images)
                                            .Include(m => m.Category)
                                            .Where(m=>m.Name.ToLower().Contains(searchText.ToLower()))
                                            .Take(5)                      
                                            .ToList();  

            return PartialView("_SearchPartial",products);
        }


  


    }
}


