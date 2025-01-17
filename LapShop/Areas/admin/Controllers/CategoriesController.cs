﻿using Microsoft.AspNetCore.Mvc;
using LapShop.Models;
using LapShop.Bl;
using LapShop.Utalties;
using Microsoft.AspNetCore.Authorization;

namespace LapShop.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("admin")]
    public class CategoriesController : Controller
    {
        public CategoriesController(ICategories categories)
        {
            oclsCategories = categories;
        }

            ICategories oclsCategories;
        public IActionResult List()
        {
            
            return View(oclsCategories.GetAll());
        }
        

        public IActionResult Edit(int? categoryId)
        {
                var category = new TbCategory();

            if(categoryId != null)
            {

                category = oclsCategories.GetById(Convert.ToInt32(categoryId) );
            }
            
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task <IActionResult> Save(TbCategory category,List<IFormFile> Files)
        {

            if (!ModelState.IsValid)
                return View("Edit",category);
            category.ImageName = await Helper.UploadImage(Files, "Categories");
            oclsCategories.Save(category);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int categoryId)
        {
            oclsCategories.Dekete(categoryId);
            return RedirectToAction("List");

        }

        async Task<string> UploadImage(List<IFormFile> Files)
        {
            foreach (var file in Files)
            {
                if (file.Length > 0)
                {
                    string ImageName = Guid.NewGuid().ToString() + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".jpg";
                    var filePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads/Categories", ImageName);
                    using (var stream = System.IO.File.Create(filePaths))
                    {
                        await file.CopyToAsync(stream);
                        return ImageName;
                    }
                }
            }
            return string.Empty;
        }

    }
}
