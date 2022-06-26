using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RealMadridStore.Models;
using RealMadridStore.Models.ViewModel;
using RealMadridStore.Services;
using System;
using System.Collections.Generic;
//using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealMadridStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _product;
        private readonly IConfiguration _configuration;
       

        public ProductController(IProduct product, IConfiguration configuration)
        {
            _product = product;
            _configuration = configuration;            
        }

        [AllowAnonymous]
        public async Task<IActionResult> AllProducts()
        {
            return View(await _product.GetAllProducts());
        }

        [AllowAnonymous]
        // GET: Products
        [Route("Product/Index/{CategoryId}")]
        public async Task<IActionResult> Index(int CategoryId)
        {
            ViewData["CategoryId"] = CategoryId;
            return View(await _product.GetProducts(CategoryId));
        }

        [AllowAnonymous]
        // GET: Product/5
        public async Task<IActionResult> GetProduct(int Id)
        {
            return View(await _product.GetProduct(Id));
        }

        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            
            var product = await _product.GetProduct(id);           
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct()
        {
            ProductVM viewModel = new ProductVM
            {
                Categories = await _product.GetCategories()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product viewModel, IFormFile file)
        {

            BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("AzureBlob"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();

            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };
            if (!blob.Exists())
            {
                await blob.UploadAsync(stream, options);
            }

            viewModel.ImageUrl = blob.Uri.ToString();
            if (ModelState.IsValid)
            {
                await _product.CreateProduct(viewModel, viewModel.CategoryId);
            }
            stream.Close();

            return RedirectToAction("AllProducts");
        }


        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Product/Create/{categoryId}")]
        public async Task<IActionResult> Create(Product product, int categoryId)
        {           
            if (ModelState.IsValid)
            {
                await _product.CreateProduct(product, categoryId);
            }

            return RedirectToAction("Index", new { CategoryId = categoryId });
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _product.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile file)
        {
            BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("AzureBlob"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();

            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };
            if (!blob.Exists())
            {
                await blob.UploadAsync(stream, options);
            }

            product.ImageUrl = blob.Uri.ToString();

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pro = await _product.UpdateProduct(id, product);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction("Index", new { CategoryId = product.CategoryId });
            }
            stream.Close();
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _product.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _product.GetProduct(id);
            await _product.DeleteProduct(id);
            return RedirectToAction("Index", new { CategoryId = product.CategoryId });
        }
        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            // this is used to save the previous URL
            string urlAnterior = Request.Headers["Referer"].ToString();
            var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName

            Product productInDb = await _product.GetProduct(id);

            string product = productInDb.Name;

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(7));

            if (HttpContext.Request.Cookies[userName] == null)
            {
                HttpContext.Response.Cookies.Append(userName, product, cookieOptions);
            }
            else
            {
                string cookie = HttpContext.Request.Cookies[userName] + "," + product;
                HttpContext.Response.Cookies.Append(userName, cookie, cookieOptions);
            }

            return Redirect(urlAnterior);
        }     
    }
}
