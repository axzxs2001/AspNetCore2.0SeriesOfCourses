using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionMiddlewareDemo.Models
{
    public class PermissionMiddleware
    {

        readonly RequestDelegate _next;
        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            //context.Request.Cookies
            var path = context.Request.Path;
            //处理权限问题
            //context.Response.Redirect("/denied");

            return _next(context);
        }
    }
}
