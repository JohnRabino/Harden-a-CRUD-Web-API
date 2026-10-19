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
                Name = "Mechanical Keyboard",
                Price = 3500,
                SalePrice = 3000
            },
            new Product
            {
                Id = 2,
                Name = "Gaming Mouse",
                Price = 1500,
                SalePrice = 1200
            }
        };

        private static int _nextId = 3;

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var totalItems = _products.Count;

            var items = _products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                data = items,
                meta = new
                {
                    page,
                    pageSize,
                    totalItems,
                    totalPages =
                        (int)Math.Ceiling(
                            totalItems /
                            (double)pageSize)
                }
            });
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
            var existing = _products
                .FirstOrDefault(
                    p => p.Name.ToLower()
                    == request.Name.ToLower());

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
        }​‌
        

    }
      