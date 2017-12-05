using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Working.XUnitTest
{
    /// <summary>
    /// JsonResult扩展类
    /// </summary>
    public static class JsonResultExtension
    {
   
        /// <summary>
        /// 从JsonResult中获取属性
        /// </summary>
        /// <param name="jsonResult">JsonResult</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static dynamic GetValue(this JsonResult jsonResult,string propertyName)
        {
            return jsonResult.Value.GetType().GetProperty(propertyName).GetValue(jsonResult.Value);
        }
    }
}
