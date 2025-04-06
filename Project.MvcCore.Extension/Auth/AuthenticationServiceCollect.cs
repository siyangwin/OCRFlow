using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.MvcCore.Extension.Auth
{
	public static class AuthenticationServiceCollect
	{
		/// <summary>
		/// 加入身份認證空間
		/// </summary>
		/// <param name="services"></param>
		public static void AddAuthentications(this IServiceCollection services, string domain,string deniedPath)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
				options.Secure = CookieSecurePolicy.SameAsRequest;
			});
			services.AddAuthentication("CookieAuthenticationScheme")
				.AddCookie("CookieAuthenticationScheme", options =>
				{
					options.LoginPath = deniedPath;
					options.LogoutPath = deniedPath;
					options.AccessDeniedPath = deniedPath;//用戶嘗試訪問資源但沒有通過任何授權策略時，這是請求會重定向的相對路徑資源。
					options.ExpireTimeSpan = TimeSpan.FromMinutes(30);//指定Cookie的過期時間
					options.SlidingExpiration = true;//當Cookie過期時間已達一半時，是否重置為ExpireTimeSpan

					options.Events = new CookieAuthenticationEvents//可用于攔截和重寫Cookie身份驗證
					{
						//OnValidatePrincipal = TokenValidator.ValidateAsync
					};
					options.Cookie.Name = "token"; 
					options.Cookie.Domain = domain;
					options.Cookie.Path = "/";
					options.Cookie.HttpOnly = true;
					options.Cookie.SameSite = SameSiteMode.None;
					options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
				});
		}
	}
}
