using ProductCatalog.Domain;
using ProductCatalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.BusinessObjects
{
    public interface IBookItemBO
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBookById(int id);
        Task<Book> Add(Book item);
        Task Update(Book item);
        Task Delete(int id);
        Task<IEnumerable<Book>> GetBooksByFilter(string filterParam);
    }

    public class BookItemBO : IBookItemBO
    {
        IBookRepository _repository;
        public BookItemBO(IBookRepository repository)
        {
            _repository = repository;
        }
        public async Task<Book> Add(Book item)
        {
            await _repository.Add(item);
            return item;
        }
        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _repository.GetDetails(id);
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _repository.GetAll();
        }

        public async Task Update(Book item)
        {
            await _repository.Update(item);
        }

        public async Task<IEnumerable<Book>> GetBooksByFilter(string filterParam)
        {
            var books = await _repository.GetAll();
            return books.Where(x => (String.Equals(x.BookName, filterParam, StringComparison.CurrentCultureIgnoreCase) || String.Equals(x.Author, filterParam, StringComparison.CurrentCultureIgnoreCase))).ToList();
        }
    }
}
