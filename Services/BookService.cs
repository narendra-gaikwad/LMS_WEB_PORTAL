using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LMS_WEB_PORTAL.Models;

namespace LMS_WEB_PORTAL.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var response = await _httpClient.GetAsync("books");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Book>>();
        }

        public async Task<Book> GetBookById(int id)
        {
            var response = await _httpClient.GetAsync($"books/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>();
        }

        public async Task<bool> AddBook(Book book)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("books", book);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddBook: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateBook(Book book)
        {
            var response = await _httpClient.PutAsJsonAsync($"books/{book.Id}", book);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var response = await _httpClient.DeleteAsync($"books/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
