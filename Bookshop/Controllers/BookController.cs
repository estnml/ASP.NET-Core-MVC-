using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Utility;
using Bookshop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ImageHelper _imageHelper;

        public BookController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imageHelper = new ImageHelper(webHostEnvironment);

        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.Include(b => b.Author).AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            BookVM bookVM = new BookVM()
            {

                AuthorSelectList = _context.Authors.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return View(bookVM);
            }

            else
            {
                bookVM.Book = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                return View(bookVM);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Book book)
        {

            var files = HttpContext.Request.Form.Files;

            if (book == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (book.Id == 0)
                {
                    // Create
                    if (files.Count > 1)
                    {
                        string fileName = _imageHelper.CreateImageFileName(files);
                        book.Image = _imageHelper.SaveImage(book, null, files);
                    }
                    _context.Books.Add(book);
                }

                else
                {
                    // Edit
                    var obj = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == book.Id);
                    if (!string.IsNullOrEmpty(obj.Image) && files.Count > 1)
                    {
                        // daha önceden resim var ve yeni resim yüklenmiş.
                        _imageHelper.DeleteImage(book, obj.Image);
                    }

                    string fileName = _imageHelper.CreateImageFileName(files);
                    book.Image = _imageHelper.SaveImage(book, fileName, files);
                    _context.Books.Update(book);
                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                return View(book);
            }



        }

        public async Task<IActionResult> Details(int id)
        {
            var obj = await _context.Books.Include(b => b.Author).AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var obj = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _imageHelper.DeleteImage(obj, obj.Image);
            _context.Books.Remove(obj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
