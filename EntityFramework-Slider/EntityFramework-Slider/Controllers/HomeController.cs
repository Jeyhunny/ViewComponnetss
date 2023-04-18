using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        #region Gizli Datalar ucun
        //private readonly ILogger<HomeController> _logger;

        //private readonly IConfiguration _configuration;


        //public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        //{
        //    _logger = logger;
        //    _configuration = configuration;
          
        //}

        //public IActionResult Test()
        //{
        //    var user = _configuration.GetSection("Login:User").Value;

        //    var mail = _configuration.GetSection("Login:Mail").Value;

        //    return Content($"{user} {mail}");
        //}


        #endregion




        private readonly AppDbContext _context;

        private readonly IBasketService _basketService;

        private readonly IProductService _productService;

        private readonly ICategoryService _categoryService;
        public HomeController(AppDbContext context,
                             IBasketService basketService,
                             IProductService productService,
                             ICategoryService categoryService)
        {
            _context = context;
            _basketService = basketService;
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet] 
        public async Task<IActionResult> Index()
        {
            



            List<Slider> sliders = await _context.Sliders.Where(m => !m.SoftDelete).ToListAsync();

            SliderInfo? sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

            IEnumerable<Category> categories = await _categoryService.GetAll();


            

            IEnumerable<Product> products = await _productService.GetAll();

            About abouts = await _context.Abouts.Include(m => m.Adventages).FirstOrDefaultAsync();

            IEnumerable<Experts> experts = await _context.Experts.Where(m => !m.SoftDelete).ToListAsync();

            ExpertsHeader expertsheaders = await _context.ExpertsHeaders.FirstOrDefaultAsync();

            Subscribe subscribs = await _context.Subscribs.FirstOrDefaultAsync();

            BlogHeader blogheaders = await _context.BlogHeaders.FirstOrDefaultAsync();
            IEnumerable<Say> says = await _context.Says.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Instagram> instagrams = await _context.Instagrams.Where(m => !m.SoftDelete).ToListAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Categories = categories,
                Products = products,
                Abouts = abouts,
                Experts = experts,
                ExpertsHeaders = expertsheaders,
                Subscribs = subscribs,
                BlogHeaders = blogheaders,
                Says = says,
                Instagrams = instagrams
            };

            return View(model);
        }





        
        [HttpPost] 
        /* [ValidateAntiForgeryToken]*/  
        public async Task<IActionResult> AddBasket(int? id)  
        {


            if (id == null) return BadRequest();   

            Product dbProduct = await _productService.GetById((int)id);   


            if (dbProduct == null) return NotFound();    

            
            

            List<BasketVM> basket = _basketService.GetBasketDatas();   

            BasketVM? existProduct = basket.FirstOrDefault(m => m.Id == dbProduct.Id);  


            _basketService.AddProductToBasket(existProduct, dbProduct, basket);    

            int basketCount = basket.Sum(m => m.Count); 
            return Ok(basketCount);
        }
     

    }

    
}