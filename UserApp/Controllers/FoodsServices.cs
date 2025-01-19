using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.Data;
using Resturant.Models;

namespace Resturant.Controllers
{
    public class FoodsServices 
    {
        private readonly AppDbContext _context;

        public FoodsServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Foods>> GetAllFoodsAsync()
        {
            return await _context.Foods.ToListAsync();
        }
        public async Task<Foods?> GetFoodByIdAsync(int id)
        {
            return await _context.Foods.FindAsync(id);
        }
        public async Task AddFoodAsync(Foods food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateFoodAsync(Foods food)
        {

            var existingFood = await _context.Foods.FindAsync(food.Id);
            if (existingFood != null)
            {
                _context.Entry(existingFood).CurrentValues.SetValues(food);
                await _context.SaveChangesAsync();
            }
            
        }
        public async Task DeleteFoodAsync(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }
        }

    }
}
