using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IGenericRepository<Categories> _categoriesRepository;

        public CategoriesService(IGenericRepository<Categories> categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await _categoriesRepository.GetAllAsync();
        }

     
    }
}
