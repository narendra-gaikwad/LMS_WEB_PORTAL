using LMS_WEB_PORTAL.Models;
using LMS_WEB_PORTAL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LMS_WEB_PORTAL.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var books = await _bookService.GetBooks();

        //    // Count the number of issued books
        //    int issuedBooksCount = books.Count(b => b.IsIssued);

        //    ViewBag.IssuedBooksCount = issuedBooksCount; // Pass to the view

        //    return View(books);
        //}

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetBooks(); // Fetch all books

            // Debugging: Check total and issued copies
            foreach (var book in books)
            {
                Console.WriteLine($"Book: {book.Title}, Total: {book.TotalCopies}, Issued: {book.IssuedCopies}, Available: {book.AvailableCopies}");
            }

            ViewBag.IssuedBooksCount = books.Sum(b => b.IssuedCopies); // Count total issued books
            return View(books);
        }


        [HttpGet]
        public async Task<IActionResult> SearchBooks(string searchTerm)
        {
            var books = await _bookService.GetBooks();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                books = books.Where(b =>
                    b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    b.ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return Json(books); // Return JSON response
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Helps prevent CSRF attacks
        public async Task<IActionResult> Create(Book book)
        {
            Console.WriteLine($"Received Data: Title={book.Title}, Author={book.Author}, ISBN={book.ISBN}");

            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                // Log ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
                return View(book);
            }

            try
            {
                bool result = await _bookService.AddBook(book);
                Console.WriteLine($"AddBook result: {result}");

                if (result)
                {
                    Console.WriteLine("Book added successfully!");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("Failed to save book in the database.");
                    ModelState.AddModelError("", "Failed to save book. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the book.");
            }

            return View(book);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                bool result = await _bookService.UpdateBook(book);
                if (result)
                    return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"DeleteConfirmed called for ID: {id}");
            bool result = await _bookService.DeleteBook(id);

            if (result)
                return RedirectToAction(nameof(Index));

            return View();
        }



    }
}
