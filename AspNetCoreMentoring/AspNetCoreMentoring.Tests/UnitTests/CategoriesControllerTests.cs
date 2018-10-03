using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.Controllers;
using AspNetCoreMentoring.UI.Mapping;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AutoFixture;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMentoring.Tests
{
    public class CategoriesControllerTests
    {
        private readonly IMapper _mapper;
        public CategoriesControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoriesProfile>();
            });

            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfCategories()
        {
            var mockRepo = new Mock<ICategoriesService>();

            var expectedCategories = GetTestCategories();
            mockRepo.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(expectedCategories);

            var controller = new CategoriesController(mockRepo.Object, _mapper);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<CategoryReadListViewModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(3, model.Count());
            Assert.Equal(expectedCategories.Select(v => v.CategoryId), model.Select(v => v.CategoryId));
            Assert.Equal(expectedCategories.Select(v => v.CategoryName), model.Select(v => v.CategoryName));
            Assert.Equal(expectedCategories.Select(v => v.Description), model.Select(v => v.Description));

        }

        private IEnumerable<Categories> GetTestCategories()
        {
            Fixture fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expectedCategories = fixture.CreateMany<Categories>(3);

            return expectedCategories;
        }
    }
}
