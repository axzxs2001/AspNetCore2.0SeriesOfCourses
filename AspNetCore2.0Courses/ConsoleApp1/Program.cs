using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Working.Controllers;
using Working.Models.DataModel;
using Working.Models.Repository;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Mock<IDepartmentRepository> _departmentMock;
            Mock<IRoleRepository> _roleMock;
            Mock<IUserRepository> _userMock;
            Mock<IWorkItemRepository> _workitemMock;
            Mock<ILogger<HomeController>> _logMock;
            HomeController _homeController;

            _departmentMock = new Mock<IDepartmentRepository>();
            _roleMock = new Mock<IRoleRepository>();
            _userMock = new Mock<IUserRepository>();
            _workitemMock = new Mock<IWorkItemRepository>();
            _logMock = new Mock<ILogger<HomeController>>();
            _homeController = new HomeController(_logMock.Object, _userMock.Object, _departmentMock.Object, _workitemMock.Object, _roleMock.Object)
            {
                ControllerContext = new ControllerContext()
            };
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            var claims = new Claim[]
              {
                    new Claim(ClaimTypes.Sid,"1"),

              };
            _homeController.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProviderMock.Object,
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };


            var list = new List<FullDepartment>() { new FullDepartment { ID = 1, DepartmentName = "a", PDepartmentID = 0, PDepartmentName = "ab" } };
            _departmentMock.Setup(d => d.GetAllPDepartment()).Returns(value: list);

            var actionResult = _homeController.GetAllPDepartments();
            var jsonResult = actionResult as JsonResult;

            var jsonString = jsonResult.Value.ToString();

            var v = GetValue(jsonResult, "result");
        }

        static dynamic GetValue(JsonResult jsonResult, string name)
        {
            return jsonResult.Value.GetType().GetProperty(name).GetValue(jsonResult.Value);
        }

    }
}
