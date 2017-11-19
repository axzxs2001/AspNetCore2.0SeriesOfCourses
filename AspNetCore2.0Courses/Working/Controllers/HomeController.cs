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

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IDepartmentRepository departmentRepository, IWorkItemRepository workItemRepository,IRoleRepository roleRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _workItemRepository = workItemRepository;
            _roleRepository = roleRepository;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("这是HomeController下的Index Action");

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
                return View();
            }
        }
        #endregion


        #region 部门
        [HttpGet("departments")]
        public IActionResult Departments()
        {
            return View();
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
                var list = _departmentRepository.GetAllPDepartment();
                return ToJson(BackResult.Success, data: list);

            }
            catch (Exception exc)
            {
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
                var list = _departmentRepository.GetAllDepartment();
                return ToJson(BackResult.Success, data: list);

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [HttpPost("adddepartment")]
        public IActionResult AddDepartment(Department deparment)
        {
            try
            {
                var result = _departmentRepository.AddDepartment(deparment);
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "添加成功" : "添加失败");

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [HttpPut("modifydepartment")]
        public IActionResult ModifyDepartment(Department department)
        {
            try
            {
                var result = _departmentRepository.ModifyDepartment(department);
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "修改成功" : "修改失败");

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [HttpDelete("removedepartment")]
        public IActionResult RemoveDepartment(int departmentID)
        {
            try
            {
                var result = _departmentRepository.RemoveDepartment(departmentID);
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "删除成功" : "删除失败");

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        #endregion

        #region 我的工作
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
                if (!string.IsNullOrEmpty(UserID))
                {
                    var workItems = _workItemRepository.GetWorkItemByYearMonth(year, month, int.Parse(UserID));
                    return ToJson(BackResult.Success, data: workItems);
                }
                else
                {
                    return ToJson(BackResult.Error, message: "用户没有登录ID");
                }
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        [HttpPost("addworkitem")]
        public IActionResult AddWorkItem(WorkItem workItem)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserID))
                {
                    var result = _workItemRepository.AddWorkItem(workItem, int.Parse(UserID));
                    return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "编辑成功" : "编辑失败");
                }
                else
                {
                    return ToJson(BackResult.Error, message: "用户没有登录ID");
                }

            }
            catch (Exception exc)
            {
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
                if (!string.IsNullOrEmpty(DepartmentID))
                {
                    var departments = _departmentRepository.GetDeparmentByPID(int.Parse(DepartmentID));
                    return ToJson(BackResult.Success, data: departments);
                }
                else
                {
                    return ToJson(BackResult.Error, message: "没有登录用户的部门ID");
                }
            }
            catch (Exception exc)
            {
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
                var users = _userRepository.GetUsersByDepartmentID(departmentID);
                return ToJson(BackResult.Success, data: users);

            }
            catch (Exception exc)
            {
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

                var workItems = _workItemRepository.GetWorkItemByYearMonth(year, month, userID);
                return ToJson(BackResult.Success, data: workItems);

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }

        #endregion

        #region 用户管理
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
        [HttpGet("userroles")]
        public IActionResult GetDepartmentUsers(int departmentID)
        {
            try
            {
                var users = _userRepository.GetDepartmentUsers(departmentID);
                return ToJson(BackResult.Success, data: users);
            }
            catch (Exception exc)
            {
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
                var roles = _roleRepository.GetRoles();
                return ToJson(BackResult.Success, data: roles);
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }

        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [HttpPost("adduser")]
        public IActionResult AddUser(User user)
        {
            try
            {              
                var result = _userRepository.AddUser(user);
                return ToJson(result?BackResult.Success:BackResult.Fail,data:result?  "添加成功":"添加失败");
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [HttpPut("modifyuser")]
        public IActionResult ModifyUser(User user)
        {
            try
            {
                var result = _userRepository.ModifyUser(user);
                return ToJson(result ? BackResult.Success : BackResult.Fail, data: result ? "修改成功" : "修改失败");
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpDelete("removeuser")]
        public IActionResult RemoveUser(int userID)
        {
            try
            {
                var result = _userRepository.RemoveUser(userID);
                return ToJson(result ? BackResult.Success : BackResult.Fail, data: result ? "删除成功" : "删除失败");
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        #endregion
    }
}
