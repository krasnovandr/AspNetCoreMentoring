using AspNetCoreMentoring.Infrastructure.EfEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetCategoriesAsync();
        Task<Categories> GetCategoryAsync(int id);
        Task UpdateCategoryImageAsync(int id, byte[] newImage);
        Task UpdateCategoryAsync(Categories model);

        Task FixBrokenImages();
    }
}