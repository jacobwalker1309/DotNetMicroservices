using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket) ?? new ShoppingCart(userName));
        }
    }
}