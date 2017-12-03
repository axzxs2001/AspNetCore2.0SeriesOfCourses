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
using System.Text;
using Newtonsoft.Json;


namespace Working.XUnitTest
{
    #region 初始化和公共函数
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
        }

        #endregion

        #region 登录测试
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
        #endregion

        #region 修改密码测试
        /// <summary>
        /// 修改密码单元测试
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="oldPassword">旧蜜码</param>
        /// <param name="result">结果</param>
        [Theory]
        [InlineData("a", "b", 1)]
        [InlineData("b", "a", 0)]
        public void ModifyPassword_Default_Return(string newPassword, string oldPassword, int result)
        {
            _userMock.Setup(u => u.ModifyPassword(newPassword, oldPassword, 1)).Returns(value: result == 1);

            var actionResult = _homeController.ModifyPassword(oldPassword, newPassword);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            var jResult = jsonResult.GetValue("result");
            Assert.Equal(result, jResult);
        }
        /// <summary>
        /// 修改密码异常测试
        /// </summary>
        [Fact]
        public void ModifyPassword_Exception_ReturnMessage()
        {
            _userMock.Setup(u => u.ModifyPassword("a", "b", 1)).Throws(new Exception("异常"));
            var actionResult = _homeController.ModifyPassword("b", "a");
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Contains("异常", jsonResult.Value.ToString());
        }
        #endregion

        #region  获取带有父级部门的部门测试
        /// <summary>
        /// 获取带有父级部门的部门测试
        /// </summary>
        [Fact]
        public void GetAllPDepartments_Default_Return()
        {
            var list = new List<FullDepartment>() { new FullDepartment { ID = 1, DepartmentName = "a", PDepartmentID = 0, PDepartmentName = "ab" } };
            _departmentMock.Setup(d => d.GetAllPDepartment()).Returns(value: list);
            var actionResult = _homeController.GetAllPDepartments();
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            var result = jsonResult.GetValue("result");
            Assert.Equal(1, result);
        }

        /// <summary>
        /// 获取带有父级部门的部门异常测试
        /// </summary>
        [Fact]
        public void GetAllPDepartments_Exception_ReturnMessage()
        {
            _departmentMock.Setup(d => d.GetAllPDepartment()).Throws(new Exception("异常"));
            var actionResult = _homeController.GetAllPDepartments();
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Contains("异常", jsonResult.Value.ToString());
        }

        /// <summary>
        /// 获取全部部门测试
        /// </summary>
        [Fact]
        public void GetAllDepartments_Default_Return()
        {
            var list = new List<Department>() { new Department { ID = 1, DepartmentName = "a", PDepartmentID = 0 } };
            _departmentMock.Setup(d => d.GetAllDepartment()).Returns(value: list);
            var actionResult = _homeController.GetAllDepartments();
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            var result = jsonResult.GetValue("result");
            Assert.Equal(1, result);
        }

        /// <summary>
        /// 获取全部部门异常测试
        /// </summary>
        [Fact]
        public void GetAllDepartments_Exception_ReturnMessage()
        {
            _departmentMock.Setup(d => d.GetAllDepartment()).Throws(new Exception("异常"));
            var actionResult = _homeController.GetAllDepartments();
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Contains("异常", jsonResult.Value.ToString());
        }
        #endregion

        #region 部门管理
        /// <summary>
        /// 添加部门测试
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void AddDepartment_Default_Return(int result)
        {
            var department = new Department { ID = 1, DepartmentName = "a", PDepartmentID = 0 } ;
            _departmentMock.Setup(d => d.AddDepartment(department)).Returns(value: result==1);
            var actionResult = _homeController.AddDepartment(department);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            var jResult = jsonResult.GetValue("result");
            Assert.Equal(result, jResult);
        }

        /// <summary>
        /// 添加部门异常测试
        /// </summary>
        [Fact]
        public void AddDepartment_Exception_ReturnMessage()
        {
            _departmentMock.Setup(d => d.AddDepartment(null)).Throws(new Exception("异常"));
            var actionResult = _homeController.AddDepartment(null);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Contains("异常", jsonResult.Value.ToString());
        }



        /// <summary>
        /// 修改部门测试
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void ModifyDepartment_Default_Return(int result)
        {
            var department = new Department { ID = 1, DepartmentName = "a", PDepartmentID = 0 };
            _departmentMock.Setup(d => d.ModifyDepartment(department)).Returns(value: result == 1);
            var actionResult = _homeController.ModifyDepartment(department);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            var jResult = jsonResult.GetValue("result");
            Assert.Equal(result, jResult);
        }

        /// <summary>
        /// 修改部门异常测试
        /// </summary>
        [Fact]
        public void ModifyDepartment_Exception_ReturnMessage()
        {
            _departmentMock.Setup(d => d.ModifyDepartment(null)).Throws(new Exception("异常"));
            var actionResult = _homeController.ModifyDepartment(null);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Contains("异常", jsonResult.Value.ToString());
        }



        /// <summary>
        /// 删除部门测试
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void RemoveDepartment_Default_Return(int result)
        {
            var departmentID = 1;
            _departmentMock.Setup(d => d.RemoveDepartment(departmentID)).Returns(value: result == 1);
            var actionResult = _homeController.RemoveDepartment(departmentID);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            var jResult = jsonResult.GetValue("result");
            Assert.Equal(result, jResult);
        }

        /// <summary>
        /// 删除部门异常测试
        /// </summary>
        [Fact]
        public void RemoveDepartment_Exception_ReturnMessage()
        {
            _departmentMock.Setup(d => d.RemoveDepartment(1)).Throws(new Exception("异常"));
            var actionResult = _homeController.RemoveDepartment(1);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Contains("异常", jsonResult.Value.ToString());
        }
        #endregion
    }

}
