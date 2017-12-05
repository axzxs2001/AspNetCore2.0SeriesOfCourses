using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Working.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Working.Models.Repository;
using Working.Models.DataModel;
using Newtonsoft.Json;

namespace Working.Controllers
{
    [Authorize(Roles = "Manager,Leader,Employee")]
    public class HomeController : BaseController
    {
        /// <summary>
        /// 日记类
        /// </summary>

        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 用户仓储
        /// </summary>
        readonly IUserRepository _userRepository;
        /// <summary>
        /// 部门仓储
        /// </summary>
        readonly IDepartmentRepository _departmentRepository;
        /// <summary>
        /// 工作仓储
        /// </summary>
        readonly IWorkItemRepository _workItemRepository;
        /// <summary>
        /// 角色仓储
        /// </summary>
        readonly IRoleRepository _roleRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IDepartmentRepository departmentRepository, IWorkItemRepository workItemRepository, IRoleRepository roleRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _workItemRepository = workItemRepository;
            _roleRepository = roleRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region 登录
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            _logger.LogInformation("登录");
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.returnUrl = returnUrl;
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string userName, string password, string returnUrl)
        {
            try
            {
                _logger.LogInformation($"登录：UserName={userName}");
                var userRole = _userRepository.Login(userName, password);
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role,userRole.RoleName),
                    new Claim(ClaimTypes.Name,userRole.Name),
                    new Claim(ClaimTypes.Sid,userRole.ID.ToString()),
                    new Claim(ClaimTypes.GroupSid,userRole.DepartmentID.ToString()),
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));
                return new RedirectResult(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);

            }
            catch (Exception exc)
            {
                ViewBag.error = exc.Message;
                _logger.LogCritical(exc, $"登录异常：{ exc.Message}");
                return new ViewResult();

            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"{User.Identity.Name}登出");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet("modifypassword")]
        public IActionResult ModifyPassword()
        {
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        [HttpPost("modifypassword")]
        public IActionResult ModifyPassword(string oldPassword, string newPassword)
        {
            try
            {
                var result = _userRepository.ModifyPassword(newPassword, oldPassword, UserID);
                _logger.LogInformation($"修改密码:{(result ? "修改密码成功" : "修改密码失败")}");
                return ToJson(result ? BackResult.Success:BackResult.Fail, message: result ? "修改密码成功" : "修改密码失败");
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"修改密码：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        #endregion

        #region 部门
        [Authorize(Roles = "Manager")]
        [HttpGet("departments")]
        public IActionResult Departments()
        {
            return new ViewResult();
        }
        /// <summary>
        /// 获取全部带父级部门的部门
        /// </summary>
        /// <returns></returns>

        [HttpGet("getallpdepartment")]
        public IActionResult GetAllPDepartments()
        {
            try
            {
                _logger.LogInformation("获取全部带父级部门的部门");
                var list = _departmentRepository.GetAllPDepartment();
                return ToJson(BackResult.Success, data: list);

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"获取全部带父级部门的部门：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        /// <summary>
        /// 获取全部带父级部门的部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldepartment")]
        public IActionResult GetAllDepartments()
        {
            try
            {
                _logger.LogInformation("获取全部带父级部门的部门");
                var list = _departmentRepository.GetAllDepartment();
                return ToJson(BackResult.Success, data: list);

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"获取全部带父级部门的部门：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpPost("adddepartment")]
        public IActionResult AddDepartment(Department deparment)
        {
            try
            {
                var result = _departmentRepository.AddDepartment(deparment);
                _logger.LogInformation($"添加部门:{(result ? "添加成功" : "添加失败")}");
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "添加成功" : "添加失败");

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"添加部门：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpPut("modifydepartment")]
        public IActionResult ModifyDepartment(Department department)
        {
            try
            {

                var result = _departmentRepository.ModifyDepartment(department);
                _logger.LogInformation($"修改部门:{(result ? "修改成功" : "修改失败")}");
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "修改成功" : "修改失败");

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"修改部门：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpDelete("removedepartment")]
        public IActionResult RemoveDepartment(int departmentID)
        {
            try
            {
                var result = _departmentRepository.RemoveDepartment(departmentID);
                _logger.LogInformation($"删除部门:departmentID={departmentID},{(result ? "删除成功" : "删除失败")}");
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "删除成功" : "删除失败");

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"删除部门：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        #endregion

        #region 我的工作
        [Authorize(Roles = "Manager")]
        [HttpGet("myworks")]
        public IActionResult MyWorks()
        {
            return View();
        }

        /// <summary>
        /// 按年月查询某人工作记录
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        [HttpGet("querymywork")]
        public IActionResult QueryMyWork(int year, int month)
        {
            try
            {
                _logger.LogInformation($"按年月查询某人工作记录:year={year},month={month}");

                var workItems = _workItemRepository.GetWorkItemByYearMonth(year, month, UserID);
                return ToJson(BackResult.Success, data: workItems);
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"按年月查询某人工作记录：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加工作记录
        /// </summary>
        /// <param name="workItem">工作记录</param>
        /// <returns></returns>
        [HttpPost("addworkitem")]
        public IActionResult AddWorkItem(WorkItem workItem)
        {
            try
            {
                _logger.LogInformation($"添加工作记录");

                var result = _workItemRepository.AddWorkItem(workItem, UserID);
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "编辑成功" : "编辑失败");

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"添加工作记录：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        #endregion

        #region 部门工作查询
        [HttpGet("querydepartmentworks")]
        public IActionResult QueryDepartmentWorks()
        {
            return View();
        }

        /// <summary>
        /// 获取登录用户的所有下属部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("getchilddepartments")]
        public IActionResult GetChildDepartments()
        {
            try
            {
                _logger.LogInformation($"获取登录用户的所有下属部门");
                var departments = _departmentRepository.GetDeparmentByPID(DepartmentID);
                return ToJson(BackResult.Success, data: departments);
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"获取登录用户的所有下属部门：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 按部门ID获取用户
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <returns></returns>
        [HttpGet("getuserbydepartmentid")]
        public IActionResult GetUserByDepartmentID(int departmentID)
        {
            try
            {
                _logger.LogInformation($"按部门ID获取用户:departmentID={departmentID}");
                var users = _userRepository.GetUsersByDepartmentID(departmentID);
                return ToJson(BackResult.Success, data: users);

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"按部门ID获取用户：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 按年月用户查询工作记录
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpGet("queryuserworks")]
        public IActionResult QueryUserWorks(int year, int month, int userID)
        {
            try
            {
                _logger.LogInformation($"按年月用户查询工作记录:year={year},month={month},userid={UserID}");
                var workItems = _workItemRepository.GetWorkItemByYearMonth(year, month, userID);
                return ToJson(BackResult.Success, data: workItems);

            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"按年月用户查询工作记录：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        #endregion

        #region 用户管理
        [Authorize(Roles = "Manager")]
        [HttpGet("userindex")]
        public IActionResult UserIndex()
        {
            return View();
        }
        /// <summary>
        /// 按部分获取用户
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("userroles")]
        public IActionResult GetDepartmentUsers(int departmentID)
        {
            try
            {
                _logger.LogInformation($"按部分获取用户");
                var users = _userRepository.GetDepartmentUsers(departmentID);
                return ToJson(BackResult.Success, data: users);
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"按部分获取用户：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 查询全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            try
            {
                _logger.LogInformation($"查询全部角色");
                var roles = _roleRepository.GetRoles();
                return ToJson(BackResult.Success, data: roles);
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"查询全部角色：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }

        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [AllowAnonymous]//[Authorize(Roles = "Manager")]
        [HttpPost("adduser")]
        public IActionResult AddUser(User user)
        {
            try
            {
                var result = _userRepository.AddUser(user);
                _logger.LogInformation($"添加用户:{(result ? "添加成功" : "添加失败")}");
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "添加成功" : "添加失败");
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"添加用户：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpPut("modifyuser")]
        public IActionResult ModifyUser(User user)
        {
            try
            {
                var result = _userRepository.ModifyUser(user);
                _logger.LogInformation($"修改用户{ (result ? "修改成功" : "修改失败")}");
                return ToJson(result ? BackResult.Success : BackResult.Fail, data: result ? "修改成功" : "修改失败");
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"修改用户：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpDelete("removeuser")]
        public IActionResult RemoveUser(int userID)
        {
            try
            {

                var result = _userRepository.RemoveUser(userID);
                _logger.LogInformation($"删除用户： {(result ? "删除成功" : "删除失败")}");
                return ToJson(result ? BackResult.Success : BackResult.Fail, data: result ? "删除成功" : "删除失败");
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, $"删除用户：{ exc.Message}");
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        #endregion
    }
}
