using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Working;
using Working.Controllers;
using Working.Models.DataModel;
using Working.Models.Repository;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
                       var _server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());
            var _client = _server.CreateClient();

            var request = "/adduser";
            var data = new Dictionary<string, string>();

            data.Add("ID", "1");
            data.Add("DepartmentID", "1");
            data.Add("RoleID", "1");
            data.Add("UserName", "test");
            data.Add("Password", "test");
            data.Add("Name", "test_Name");
            var content = new FormUrlEncodedContent(data);
            var response = _client.PostAsync(request, content).Result;
            response.EnsureSuccessStatusCode();
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var backJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseJson>(responseJson);

            //           var _server = new TestServer(new WebHostBuilder()
            //.UseStartup<Startup>());
            //           var _client = _server.CreateClient();
            //           var request = "/userroles?departmentID=1";
            //           var response = _client.GetAsync(request).Result;
            //           response.EnsureSuccessStatusCode();
            //           var responseJson = response.Content.ReadAsStringAsync().Result;

            //           var backJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseJson>(responseJson);

        }
    }
    public class ResponseJson
    {
        public int result
        { get; set; }
        public string message
        { get; set; }

        public dynamic data
        { get; set; }
    }
    //static void F()
    //{

    //    var _departmentMock = new Mock<IDepartmentRepository>();
    //    var _roleMock = new Mock<IRoleRepository>();
    //    var _userMock = new Mock<IUserRepository>();
    //    var _workitemMock = new Mock<IWorkItemRepository>();
    //    var _logMock = new Mock<ILogger<HomeController>>();


    //    var _homeController = new HomeController(_logMock.Object, _userMock.Object, _departmentMock.Object, _workitemMock.Object, _roleMock.Object);




    //    //_userMock.Setup(u => u.Login("a", "b")).Returns(new UserRole() { ID = 1, Name = "张三", RoleName = "Leader", DepartmentID = 1, UserName = "a", Password = "b" });
    //    _userMock.Setup(u => u.Login("a", "b")).Returns(value: null);



    //    _homeController.ControllerContext = new ControllerContext();
    //    var authServiceMock = new Mock<IAuthenticationService>();
    //    authServiceMock
    //        .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
    //        .Returns(Task.FromResult((object)null));

    //    var serviceProviderMock = new Mock<IServiceProvider>();
    //    serviceProviderMock
    //        .Setup(_ => _.GetService(typeof(IAuthenticationService)))
    //        .Returns(authServiceMock.Object);


    //    _homeController.ControllerContext.HttpContext = new DefaultHttpContext()
    //    {
    //        RequestServices = serviceProviderMock.Object
    //    };



    //    var viewMock = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionaryFactory>();



    //    var result = _homeController.Login("a", "b", null);
    //}
}

