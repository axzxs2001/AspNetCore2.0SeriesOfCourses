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

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IDepartmentRepository departmentRepository, IWorkItemRepository workItemRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _workItemRepository = workItemRepository;
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
                    new Claim(ClaimTypes.Sid,userRole.ID.ToString())
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
                    var result = _workItemRepository.AddWorkItem(workItem,int.Parse(UserID));
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
    }
}
