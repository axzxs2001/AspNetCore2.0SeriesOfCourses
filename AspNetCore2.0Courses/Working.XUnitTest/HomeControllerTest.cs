using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;
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

            var claims = new Claim[]
              {                   
                    new Claim(ClaimTypes.Sid,"1"),
                   
              };
            _homeController.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProviderMock.Object,
                User=new ClaimsPrincipal (new ClaimsIdentity(claims))
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
        /// <summary>
        /// 测试空用户
        /// </summary>
        [Fact]
        public void Login_NullUsert_ReturnView()
        {
            _userMock.Setup(u => u.Login("a", "b")).Returns(value: null);            
            var result = _homeController.Login("a", "b", null);
            var viewResult = Assert.IsType<ViewResult>(result);          
            Assert.NotNull(viewResult);
        } 
        /// <summary>
        /// 修改密码单元测试
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="oldPassword">旧蜜码</param>
        /// <param name="result">结果</param>
        [Theory]
        [InlineData("a","b",true)]
        [InlineData("b", "a", false)]
        public void ModifyPassword_Default_Return(string newPassword,string oldPassword,bool result)
        {
            _userMock.Setup(u => u.ModifyPassword(newPassword, oldPassword, 1)).Returns(value: result);

            var actionResult = _homeController.ModifyPassword(oldPassword,newPassword);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);

            Assert.Contains(result?"修改密码成功":"修改密码失败",jsonResult.Value.ToString());
        }
    }

}
