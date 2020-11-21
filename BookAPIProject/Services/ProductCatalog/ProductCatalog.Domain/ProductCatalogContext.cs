using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain;

namespace ProductCatalog.Domain
{
    public class ProductCatalogContext : DbContext
    {

        public ProductCatalogContext()
        {

        }

        public ProductCatalogContext (DbContextOptions<ProductCatalogContext> options)
            : base(options)
        {
        }
        public DbSet<ProductCatalog.Domain.Book> Books { get; set; }
    }
}
