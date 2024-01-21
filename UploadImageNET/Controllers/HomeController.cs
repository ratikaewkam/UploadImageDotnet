using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UploadImageNET.Data;
using UploadImageNET.Models;
using UploadImageNET.ViewModels;

namespace UploadImageNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var items = _db.ImgData.ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ImageDataViewModel vm)
        {
            string fileName = Upload(vm);
            var imgDt = new ImageData
            {
                Name = vm.Name,
                Img = fileName
            };

            _db.ImgData.Add(imgDt);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private string Upload(ImageDataViewModel vm)
        {
            string fileName = null;
            if (vm.Img!=null)
            {
                string dir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + vm.Img.FileName;
                string filePath = Path.Combine(dir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.Img.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}