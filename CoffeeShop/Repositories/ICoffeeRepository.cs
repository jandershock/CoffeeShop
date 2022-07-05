using CoffeeShop.Models;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        public void AddCoffee(Coffee coffee);
        public Coffee GetCoffee(int id);
        public List<Coffee> GetAllCoffee();
        public void UpdateCoffee(Coffee coffee);
        public void DeleteCoffee(int id);
    }
}
