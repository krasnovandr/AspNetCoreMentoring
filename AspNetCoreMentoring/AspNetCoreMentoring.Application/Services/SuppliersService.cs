using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core.Services
{
    public class SuppliersService : ISupplierService
    {
        private readonly IGenericRepository<Suppliers> _suppliersRepository;

        public SuppliersService(IGenericRepository<Suppliers> suppliersRepository)
        {
            _suppliersRepository = suppliersRepository;
        }

        public async Task<IEnumerable<Suppliers>> GetSuppliersAsync()
        {
            return await _suppliersRepository.GetAllAsync();
        }
    }
}
