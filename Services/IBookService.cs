using LMS_WEB_PORTAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_WEB_PORTAL.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBookById(int id);
        Task<bool> AddBook(Book book);
        Task<bool> UpdateBook(Book book);
        Task<bool> DeleteBook(int id);
    }
}



