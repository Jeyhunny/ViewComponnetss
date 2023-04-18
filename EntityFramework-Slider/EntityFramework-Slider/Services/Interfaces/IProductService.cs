using EntityFramework_Slider.Models;
using System.Collections;

namespace EntityFramework_Slider.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);

        Task<IEnumerable<Product>> GetAll();

        Task<Product> GetFullDataById(int id);

    }
}
