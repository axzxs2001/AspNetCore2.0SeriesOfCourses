using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;
using Moq.Dapper;
using Working.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Working.XUnitTest
{
    /// <summary>
    /// HomeController单元测试
    /// </summary>
    [Trait("HomeController单元测试", "HomeControllerTest")]
    public class HomeControllerTest
    {
        Mock<IDepartmentRepository> _departmentMock;
        Mock<IRoleRepository> _roleMock;
        Mock<IUserRepository> _userMock;
        Mock<IWorkItemRepository> _workitemMock;
        Mock<ILogger<HomeController>> _logMock;
        HomeController _homeController;
        public HomeControllerTest()
        {
            _departmentMock = new Mock<IDepartmentRepository>();
            _roleMock = new Mock<IRoleRepository>();
            _userMock = new Mock<IUserRepository>();
            _workitemMock = new Mock<IWorkItemRepository>();
            _logMock = new Mock<ILogger<HomeController>>();
            _homeController = new HomeController(_logMock.Object, _userMock.Object, _departmentMock.Object, _workitemMock.Object, _roleMock.Object);

            _homeController.ControllerContext = new ControllerContext();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            _homeController.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProviderMock.Object
            };
        }
        /// <summary>
        /// 测试正确用户名密码登录
        /// </summary>
        [Fact]
        public void Login_Default_Return()
        {
            _userMock.Setup(u => u.Login("a", "b")).Returns(new UserRole() { ID = 1, Name = "张三", RoleName = "Leader", DepartmentID = 1, UserName = "a", Password = "b" });

            var result = _homeController.Login("a", "b", null);
            var redirectResult = Assert.IsType<RedirectResult>(result);

            Assert.Equal("/", redirectResult.Url);

        }

        [Fact]
        public void Login_NullUsert_ReturnView()
        {
            _userMock.Setup(u => u.Login("a", "b")).Returns(value: null);            
            var result = _homeController.Login("a", "b", null);
            var viewResult = Assert.IsType<ViewResult>(result);          
            Assert.NotNull(viewResult);
        } 
    }

}
