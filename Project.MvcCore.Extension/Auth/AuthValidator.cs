using Project.Core;
using Project.Model;
using Project.Model.EnumModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.MvcCore.Extension.Auth
{
    public class AuthValidator : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 重新自定義認證
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //沒有經過cookie驗證/api/appuser/loginout
            //if (!context.HttpContext.User.Identity.IsAuthenticated)
            //{
            //if (true)
            //{
            //是否需要授权的Api
            bool isAllowAnonymous = context.ActionDescriptor.FilterDescriptors.Any(x => x.Filter.ToString() == typeof(AllowAnonymousFilter).ToString());

            //获取token
            string token = context.HttpContext.Request.Headers["Token"];

            if (string.IsNullOrEmpty(token))
                token = context.HttpContext.Request.Query["Token"];

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    //获取Token缓存数据
                    var claims = MemoryCacheHelper.Get<ClaimsIdentity>(token);
                    //新添加逻辑：cms用token登录，并且不用保存到db中的判断
                    if (context.HttpContext.Request.Headers["ClientType"] == "CMS")
                    #region 新添加逻辑：cms用token登录，并且不用保存到db中的判断
                    {
                        if (claims != null)
                        {
                            context.HttpContext.Request.Headers.Add("UserId", claims.Claims.FirstOrDefault(a => a.Type == "UserId")?.Value);
                        }
                        else
                        {
                            if (isAllowAnonymous)
                                return;
                            if (context.HttpContext.Request.Path != "/api/appuser/loginout")
                            {
                                //不存在的token直接拒絕
                                context.Result = new JsonResult(new { success = false, Code = "401", Message = "沒有授權3" });
                                context.HttpContext.Response.StatusCode = 200;

                                //写入日志
                                NewMethod(context);
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        //APP，按原逻辑
                        //Token是否为空
                        if (claims != null)
                        {
                            //根據token獲取接口授權
                            //context.HttpContext.User.AddIdentity(claims);
                            context.HttpContext.Request.Headers.Add("UserId", claims.Claims.FirstOrDefault(a => a.Type == "UserId")?.Value);
                            //context.HttpContext.Request.Headers.Add("UserName", claims.Claims.FirstOrDefault(a => a.Type == "UserName")?.Value);
                        }
                        else
                        {
                            //查询数据库

                            //存在则写入缓存
                            var resultModel = GlobalConfig.AuthorizationTokenCoreService.CheckAuthorizationToken(token);
                            if (resultModel.success == true)
                            {
                                //写入缓存
                                //寫入身份信息到認證中心
                                var claimss = new[]
                                {
                                        new Claim("UserId",resultModel.data.UserId.ToString()),
                                        new Claim("Token",resultModel.data.Token)
                                 };

                                //先擠掉其它登錄用戶
                                context.HttpContext.SignOutByUserAsync(resultModel.data.UserId.ToString());

                                //token保存到緩存
                                context.HttpContext.SignInAsync(claimss, resultModel.data.Token, resultModel.data.UserId.ToString(), 12, true);
                                //向头部添加userid
                                context.HttpContext.Request.Headers.Add("UserId", resultModel.data.UserId.ToString());
                            }
                            else
                            {
                                if (isAllowAnonymous)
                                    return;
                                if (context.HttpContext.Request.Path != "/api/appuser/loginout")
                                {
                                    //不存在的token直接拒絕
                                    context.Result = new JsonResult(new { success = false, Code = "401", Message = "沒有授權3" });
                                    context.HttpContext.Response.StatusCode = 200;

                                    //写入日志
                                    NewMethod(context);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (isAllowAnonymous)
                        return;
                    if (context.HttpContext.Request.Path != "/api/appuser/loginout")
                    {
                        //不存在的token直接拒絕
                        context.Result = new JsonResult(new { success = false, Code = "401", ex });
                        context.HttpContext.Response.StatusCode = 200;

                        //写入日志
                        NewMethod(context);
                    }
                }
            }
            else
            {
                if (isAllowAnonymous)
                    return;
                if (context.HttpContext.Request.Path != "/api/appuser/loginout")
                {
                    //不存在的token直接拒絕
                    context.Result = new JsonResult(new { success = false, Code = "401", Message = "沒有授權5" });
                    context.HttpContext.Response.StatusCode = 200;
                    //写入日志
                    NewMethod(context);
                }
            }
            //}
            //else
            //{
            //    context.HttpContext.Request.Headers.Add("UserId", context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "UserId")?.Value);
            //    //写入日志
            //    //context.HttpContext.Request.Headers.Add("UserId", context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "UserId")?.Value);
            //    //context.HttpContext.Request.Headers.Add("UserName", context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "UserName")?.Value);
            //}
        }

        private void NewMethod(AuthorizationFilterContext context)
        {
            string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            //記錄參數日志
            var logData = new
            {
                RequestQurey = context.HttpContext.Request.QueryString.ToString(),
                RequestContextType = context.HttpContext.Request.ContentType,
                RequestHost = context.HttpContext.Request.Host.ToString(),
                RequestPath = context.HttpContext.Request.Path,
                RequestLocalIp = (context.HttpContext.Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + context.HttpContext.Request.HttpContext.Connection.LocalPort),
                RequestRemoteIp = (context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() + ":" + context.HttpContext.Request.HttpContext.Connection.RemotePort),
                RequestParam = GetParamString(context.HttpContext)
            };
            //写入日志
            GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = context.HttpContext.Request.Headers["GuidPwd"].ToString(), ClientType = context.HttpContext.Request.Headers["ClientType"].ToString(), APIName = context.HttpContext.Request.Path, UserId = context.HttpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(context.HttpContext.Request.Headers["UserId"]), DeviceId = context.HttpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : context.HttpContext.Request.Headers["DeviceId"].ToString(), Instructions = "请求-返回", ReqParameter = JsonConvert.SerializeObject(logData), ResParameter = JsonConvert.SerializeObject(context.Result), Time = "", IP = ip });
        }

        /// <summary>
        ///  獲取參數字符串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetParamString(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            if (context.Request.HasFormContentType && context.Request.Form != null)
                foreach (var key in context.Request.Form.Keys)
                {
                    builder.Append(key + ":" + context.Request.Form[key].ToString() + "|");
                }
            if (context.Request.Query != null)
                foreach (var key in context.Request.Query.Keys)
                {
                    builder.Append(key + ":" + context.Request.Query[key].ToString() + "|");
                }
            //验证是否存在Raw参数
            if (context.Request.Body.CanRead)
            {
                var memery = new System.IO.MemoryStream();
                context.Request.Body.CopyTo(memery);
                memery.Position = 0;
                //记录head
                string header = JsonConvert.SerializeObject(context.Request.Headers);
                //记录参数内容
                string content = new StreamReader(memery, UTF8Encoding.UTF8).ReadToEnd();
                builder.Append(JsonConvert.SerializeObject(new { header, content }));
                builder.Append(Environment.NewLine);
                memery.Position = 0;
                context.Request.Body = memery;
            }
            return builder.ToString();
        }

    }
}
