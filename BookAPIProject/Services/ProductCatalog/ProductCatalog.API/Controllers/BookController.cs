using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.BusinessObjects;
using ProductCatalog.Domain;

namespace ProductCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IBookItemBO _boBookItem;
        public BookController(IBookItemBO boBookItem)
        {
            _boBookItem = boBookItem;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var items = await _boBookItem.GetBooks();
            return Ok(items.OrderBy(x => x.BookName));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var bookItem = await _boBookItem.GetBookById(id);

            if (bookItem == null)
            {
                return NotFound();
            }

            return bookItem;
        }

        [HttpGet]
        [Route("GetBookByFilters")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByFilters([FromQuery]string filterParam)
        {
            var bookItems = await _boBookItem.GetBooksByFilter(filterParam);
            if (bookItems == null)
            {
                return NotFound();
            }
            return Ok(bookItems.OrderBy(x => x.BookName));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookItem(int id, Book bookItem)
        {
            if (id != bookItem.Id)
            {
                return BadRequest();
            }
            try
            {
                await _boBookItem.Update(bookItem);
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == "Not Found")
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Book>> PostBookItem(Book bookItem)
        {
            await _boBookItem.Add(bookItem);
            return CreatedAtAction("GetBookById", new { id = bookItem.Id }, bookItem);
        }

        [HttpDelete("{id}")]
        public async Task DeleteBookItem(int id)
        {
            await _boBookItem.Delete(id);
        }
    }
}