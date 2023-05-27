using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/controller")]
    public class CatalogController : ControllerBase
    {
        // remember here to look into microsoft routes
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
    }
}