using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Working.Models;

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
        protected JsonResult ToJson(BackResult backResult,string message="",dynamic data=null,string dataFormat= "yyyy年MM月dd日")
        {
            return new JsonResult(new { result =(int)backResult,data=data, message = message }, new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver(),
                DateFormatString = dataFormat
            });
        }
    }
}