using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Bookshop.Controllers
{
    public class AuthorController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ImageHelper _imageHelper;


        public AuthorController(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imageHelper = new ImageHelper(webHostEnvironment);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.ToListAsync());
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                // CREATE
                Author author = new Author();
                return View(author);
            }

            else
            {
                // EDIT
                var obj = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
                if (obj == null)
                {
                    return NotFound();
                }

                return View(obj);
            }


        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Author author)
        {
            bool IsCreate = false;
            var files = Request.Form.Files;

            

            //todo image input ayarla
            if (ModelState.IsValid)
            {
                if (author.Id == 0)
                {
                    // CREATE
                    //todo files'dan gelen veriyi al ve resmi sunucuda tut.
                    // yükleme esnasında resim yüklenme durumu
                    if (files.Count > 0)
                    {
                        string fileName = _imageHelper.CreateImageFileName(files);
                        author.Image = _imageHelper.SaveImage(author, fileName, files);
                    }
                    _context.Authors.Add(author);
                    
                }

                else
                {
                    var obj = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == author.Id);
                    if (!string.IsNullOrEmpty(obj.Image) && files.Count > 0)
                    {
                        _imageHelper.DeleteImage(author, obj.Image);
                    }

                    string fileName = _imageHelper.CreateImageFileName(files);
                    author.Image = _imageHelper.SaveImage(author, fileName, files);
                    //todo edit kısmında, eğer önceden resim varsa önceki resmi sil daha sonra yeni resmi yükle
                    _context.Authors.Update(author);
                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                return View(author);
            }



        }

        public async Task<IActionResult> Details(int id)
        {
            var obj = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            return View(obj);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {

            // sunucuda author'a ait resim varsa sil
            var obj = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            //DeleteImageFile(obj);
            _imageHelper.DeleteImage(obj, obj.Image);
            _context.Authors.Remove(obj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
