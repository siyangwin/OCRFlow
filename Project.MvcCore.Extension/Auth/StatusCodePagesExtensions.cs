using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Project.MvcCore.Extension.Auth
{
	public static class StatusCodePagesExtensions
	{
		/// <summary>
		/// 加入身份認證,并設置拒絕授權的跳轉地址
		/// </summary>
		/// <param name="app"></param>
		/// <param name="path">拒絕的返回路徑</param>
		public static void AddUseAuthentications(this IApplicationBuilder app, string path)
		{
			app.UseCookiePolicy();
			app.UseAuthentication();
			app.UseStatusCodePages(async context =>
			{
				var request = context.HttpContext.Request;
				var response = context.HttpContext.Response;
				if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
				{
					//  "/api/appuser/denied",// 拒絕授權的跳轉地址
					response.Redirect(path);
				}
			});
		}
	}
}
