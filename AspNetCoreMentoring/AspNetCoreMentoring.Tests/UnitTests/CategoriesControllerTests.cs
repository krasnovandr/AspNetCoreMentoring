using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.Controllers;
using AspNetCoreMentoring.UI.Mapping;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AutoFixture;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMentoring.Tests.UnitTests
{
    public class CategoriesControllerTests
    {
        private readonly IMapper _mapper;
        Fixture fixture = new Fixture();
        public CategoriesControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoriesProfile>();
            });

            _mapper = new Mapper(config);
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
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
            mockRepo.Verify(mock => mock.GetCategoriesAsync(), Times.Once());
        }

        [Fact]
        public async Task GetEditCategory_CorrectInputId_ReturnsAViewResultForCategory()
        {
            var mockRepo = new Mock<ICategoriesService>();

            var testId = fixture.Create<int>();
            var expectedCategory = fixture.Create<Categories>();

            mockRepo.Setup(service => service.GetCategoryAsync(It.Is<int>(v => v == testId)))
                .ReturnsAsync(expectedCategory);

            var controller = new CategoriesController(mockRepo.Object, _mapper);

            var result = await controller.EditCategory(testId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var actualModel = Assert.IsAssignableFrom<CategoryWriteItemViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedCategory.CategoryId, actualModel.CategoryId);
            Assert.Equal(expectedCategory.CategoryName, actualModel.CategoryName);
            Assert.Equal(expectedCategory.Description, actualModel.Description);
            Assert.Equal(expectedCategory.Picture, actualModel.Picture);
            mockRepo.Verify(mock => mock.GetCategoryAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task EditCategory_WrongInputId_ReturnsNotFoundResultPage()
        {
            var mockRepo = new Mock<ICategoriesService>();

            var testId = fixture.Create<int>();
            var expectedCategory = fixture.Create<Categories>();

            mockRepo.Setup(service => service.GetCategoryAsync(It.Is<int>(v => v == testId)))
                .ReturnsAsync(expectedCategory);

            var controller = new CategoriesController(mockRepo.Object, _mapper);

            var result = await controller.EditCategory(11);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("NotFoundView", redirectResult.ActionName);
            Assert.Equal("Error", redirectResult.ControllerName);
            mockRepo.Verify(mock => mock.GetCategoryAsync(It.IsAny<int>()), Times.Once());
        }
        [Fact]
        public async Task PostEditCategory_CorrectModel_CallServiceAndRedirectToList()
        {
            var mockRepo = new Mock<ICategoriesService>();

            var editedCategory = fixture.Create<CategoryWriteItemViewModel>();

            var controller = new CategoriesController(mockRepo.Object, _mapper);

            var result = await controller.EditCategory(editedCategory);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            mockRepo.Verify(mock => mock.UpdateCategoryAsync(It.IsAny<Categories>()), Times.Once());
        }


        [Fact]
        public async Task PostEditCategory_IncorrectModel_ReturnViewWithModel()
        {
            var mockRepo = new Mock<ICategoriesService>();

            var editedCategory = fixture.Create<CategoryWriteItemViewModel>();
            var controller = new CategoriesController(mockRepo.Object, _mapper);

            controller.ModelState.AddModelError("error", "some error");
            var result = await controller.EditCategory(editedCategory);

            var viewResult = Assert.IsType<ViewResult>(result);

            var actualModel = Assert.IsAssignableFrom<CategoryWriteItemViewModel>(
              viewResult.ViewData.Model);

            Assert.Equal(editedCategory.CategoryId, actualModel.CategoryId);
            Assert.Equal(editedCategory.CategoryName, actualModel.CategoryName);
            Assert.Equal(editedCategory.Description, actualModel.Description);
            Assert.Equal(editedCategory.Picture, actualModel.Picture);
        }

        [Fact]
        public async Task UploadCategoryImage_CorrectData_ReturnViewWithModel()
        {
            var mockRepo = new Mock<ICategoriesService>();
            var mockedFile = new Mock<IFormFile>();

            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            mockedFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            mockedFile.Setup(_ => _.FileName).Returns(fileName);
            mockedFile.Setup(_ => _.Length).Returns(ms.Length);

            var editedCategory = new CategoryUpdateImageViewModel()
            {
                CategoryId = fixture.Create<int>(),
                CategoryImage = mockedFile.Object
            };

            var controller = new CategoriesController(mockRepo.Object, _mapper);

            var result = await controller.UploadCategoryImage(editedCategory);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);

            mockRepo.Verify(mock =>
            mock.UpdateCategoryImageAsync(
                It.Is<int>(v => v == editedCategory.CategoryId),
                It.IsAny<byte[]>()
            ), Times.Once());
        }


        private IEnumerable<Categories> GetTestCategories()
        {
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expectedCategories = fixture.CreateMany<Categories>(3);

            return expectedCategories;
        }
    }
}
