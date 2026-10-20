using Microsoft.AspNetCore.Mvc;
using Harden_a_CRUD_Web_API.Models;
using Harden_a_CRUD_Web_API.Exceptions;

namespace Harden_a_CRUD_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> _products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Umbrella",
                Price = 200,
                SalePrice = 150
            },
            new Product
            {
                Id = 2,
                Name = "Samsung S25",
                Price = 45000,
                SalePrice = 30000
            }
        };

        private static int _nextId = 3;

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 5 : pageSize;

            var totalItems = _products.Count;

            var items = _products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new PagedResponse<Product>
            {
                Data = items,
                Meta = new MetaData
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling(
                        totalItems / (double)pageSize)
                }
            };

            return Ok(response);
        }


        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _products
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                throw new NotFoundException(
                    $"Product {id} was not found.");
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create(
            CreateProductRequest request)
        {
            var existing = _products.FirstOrDefault(
                p => p.Name.ToLower() ==
                     request.Name.ToLower());

            if (existing is not null)
            {
                throw new ConflictException(
                    $"Product '{request.Name}' already exists.");
            }

            var product = new Product
            {
                Id = _nextId++,
                Name = request.Name,
                Price = request.Price,
                SalePrice = request.SalePrice
            };

            _products.Add(product);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Product> Update(
            int id,
            UpdateProductRequest request)
        {
            var product = _products
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                throw new NotFoundException(
                    $"Product {id} was not found.");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.SalePrice = request.SalePrice;

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = _products
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                throw new NotFoundException(
                    $"Product {id} was not found.");
            }

            _products.Remove(product);

            return NoContent();
        }
    }
}