using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AspNetCoreMentoring.Core.Exceptions;
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
            _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await _categoriesRepository.GetAllAsync();
        }

        public async Task<Categories> GetCategoryAsync(int id)
        {
            var category = await _categoriesRepository.FindByIdAsync(id);

            return category;
        }

        public async Task UpdateCategoryImageAsync(int id, byte[] newImage)
        {
            var category = await _categoriesRepository.FindByIdAsync(id);

            if (category == null)
            {
                throw new EntityNotFoundException($"Category with id {id} wasn't found");
            }

            category.Picture = newImage;
            await _categoriesRepository.UpdateAsync(category);
        }


        //public async Task UpdateCategoryImage(int id, byte[] newImage)
        //{
        //    var category = await _categoriesRepository.FindByIdAsync(id);

        //    if (category == null)
        //    {
        //        throw new EntityNotFoundException($"Category with id {id} wasn't found");
        //    }

        //    category.Picture = newImage;
        //    await _categoriesRepository.UpdateAsync(category);
        //}



        public async Task UpdateCategoryAsync(Categories category)
        {
            await _categoriesRepository.UpdateAsync(category);
        }

        public async Task FixBrokenImages()
        {
            var allCategories = await this.GetCategoriesAsync();
            foreach (var category in allCategories)
            {
                category.Picture = FixNortwindBrokenImage(category.Picture);

                await this.UpdateCategoryAsync(category);
            }
        }

        private static byte[] FixNortwindBrokenImage(byte[] categoryImage)
        {
            var memoryStream = new MemoryStream();
            memoryStream.Write(categoryImage, 78, categoryImage.Length - 78);

            return memoryStream.ToArray();
        }
    }
}
