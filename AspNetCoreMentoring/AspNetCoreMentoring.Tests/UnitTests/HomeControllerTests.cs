using AspNetCoreMentoring.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMentoring.Tests.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_CallAction_ReturnsAViewResult()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
