using AspNetCoreMentoring.Infrastructure.EfEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetCategoriesAsync();
    }
}