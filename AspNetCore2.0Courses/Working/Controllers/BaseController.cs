using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Working.Models;
using System.Security.Claims;

namespace Working.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 返回Json
        /// </summary>
        /// <param name="backResult">处理结果</param>
        /// <param name="message">消息</param>
        /// <param name="data">返回数据</param>
        /// <param name="dataFormat">日期格式</param>
        /// <returns></returns>
        protected JsonResult ToJson(BackResult backResult, string message = "", dynamic data = null, string dataFormat = "yyyy年MM月dd日")
        {
            return new JsonResult(new { result = (int)backResult, data = data, message = message }, new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver(),
                DateFormatString = dataFormat
            });
        }
        /// <summary>
        /// 登录人的用户ID
        /// </summary>
        protected int UserID
        {
            get
            {
                var userIDString = User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid)?.Value;
                if (string.IsNullOrEmpty(userIDString))
                {
                    throw new Exception("没有用户ID，请确认用户是否登录");
                }
                else
                {
                    int userID = 0;
                    var result = int.TryParse(userIDString, out userID);
                    if(result)
                    {
                        return userID;
                    }
                    else
                    {
                        throw new Exception("用户ID为非整型，请确认");
                    }
                }
            }
        }

        /// <summary>
        /// 登录人的部门ID
        /// </summary>
        protected int DepartmentID
        { 
            get
            {
                var userIDString = User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.GroupSid)?.Value;
                if (string.IsNullOrEmpty(userIDString))
                {
                    throw new Exception("没有用部门ID，请确认用户是否登录");
                }
                else
                {
                    int userID = 0;
                    var result = int.TryParse(userIDString, out userID);
                    if (result)
                    {
                        return userID;
                    }
                    else
                    {
                        throw new Exception("部门ID为非整型，请确认");
                    }
                }
            }
        }
    }
}