using ProductCatalog.Domain;
using ProductCatalog.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.EFRepositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        ProductCatalogContext _context;
        public BookRepository(bool isCachingEnabled, ProductCatalogContext context) : base(false, context)
        {
            _context = context;
        }
    }
}
