using Microsoft.AspNetCore.Mvc;
using Resturant.Models;
namespace Resturant.Controllers
{
    [Route("api/foods")]
    [ApiController]
    public class FoodsApi : ControllerBase
    {
        private readonly FoodsServices _foodsServices;

        public FoodsApi(FoodsServices foodsServices)
        {
            _foodsServices = foodsServices;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Foods>>> GetAllFoods()
        {
            var foods = await _foodsServices.GetAllFoodsAsync();
            return Ok(foods);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Foods?>> GetFoodById(int id)
        {
            var food = await _foodsServices.GetFoodByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return Ok(food);
        }
        [HttpPost]
        public async Task<ActionResult> AddFood([FromBody] Foods food)
        {
            if (food == null)
            {
                return BadRequest();
            }
            await _foodsServices.AddFoodAsync(food);
            return CreatedAtAction(nameof(GetFoodById), new { id = food.Id }, food);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFood(int id, [FromBody] Foods food)
        {
            if (food == null || food.Id != id)
            {
                return BadRequest();
            }
            var existingFood = await _foodsServices.GetFoodByIdAsync(id);
            if (existingFood == null)
            {
                return NotFound();
            }
            await _foodsServices.UpdateFoodAsync(food);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFood(int id)
        {
            var food = await _foodsServices.GetFoodByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            await _foodsServices.DeleteFoodAsync(id);
            return NoContent();
        }
    }
}
