﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Core;
using Project.Model;
using System;
using Newtonsoft.Json;

namespace Project.MvcCore.Extension.Filter
{
	/// <summary>
	/// 異常攔截器
	/// </summary>
	public class ErrorFilterAttribute:  ExceptionFilterAttribute
	{
		/// <summary>
		/// 異步獲取異常
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task OnExceptionAsync(ExceptionContext context)
		{
			LogExtension.Error(context.Exception);
			//写入日志
			GlobalConfig.SystemLogService.LogAdd(new SystemLog { Guid = context.HttpContext.Request.Headers["GuidPwd"].ToString(), ClientType=context.HttpContext.Request.Headers["ClientType"].ToString(), APIName = context.HttpContext.Request.Path, UserId = context.HttpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(context.HttpContext.Request.Headers["UserId"]), DeviceId = context.HttpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : context.HttpContext.Request.Headers["DeviceId"].ToString(), Instructions = "错误", ReqParameter = JsonConvert.SerializeObject(context.Exception), ResParameter = "" });
			return base.OnExceptionAsync(context);
		}
		/// <summary>
		/// 返回異常信息
		/// </summary>
		/// <param name="context"></param>
		public override void OnException(ExceptionContext context)
		{
			context.ExceptionHandled = true;
			context.Result = new JsonResult(new
			{
				code = "402",
				success = false,
				message = "Server internal error"
			});
		}
	}
}
