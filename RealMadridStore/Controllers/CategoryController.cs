using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealMadridStore.Models;
using RealMadridStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }

        [AllowAnonymous]
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _category.GetCategories());
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _category.CreateCategory(category);
                return RedirectToAction(nameof(Index));

            }
            return View(category);
        }

        [AllowAnonymous]
        // GET: Categories/Details/5  test       
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _category.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [Authorize(Roles = "Editor,Admin")]
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _category.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cate = await _category.UpdateCategory(id, category);

                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _category.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _category.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }

}