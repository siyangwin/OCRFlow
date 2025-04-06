using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Project.MvcCore.Extension
{
	public static class LogExtension
	{
		/// <summary>
		/// 日志仓储对象
		/// </summary>
		public static ILoggerRepository repository { get; set; }

		/// <summary>
		/// 日志对象
		/// </summary>
		private static ILog Log { get; set; }
		/// <summary>
		/// 注冊日志
		/// </summary>
		public static ILog RegisterLog(string path)
		{
			//注入日志
			repository = LogManager.CreateRepository("LogRepository");
			XmlConfigurator.Configure(repository, new FileInfo(path));
			return LogManager.GetLogger("LogRepository", "log");
		}
		/// <summary>
		/// 添加日志
		/// </summary>
		/// <param name="services"></param>
		/// <param name="path"></param>
		public static ILog AddLogs(this IServiceCollection services, string path)
		{
			var log = RegisterLog(path);
			Log = log;
			services.AddTransient(x => log);
			return log;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public static void Debug(object data)
		{
			Log.Debug(data);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public static void Error(object data)
		{
			Log.Error(data);
		}
	}


}
