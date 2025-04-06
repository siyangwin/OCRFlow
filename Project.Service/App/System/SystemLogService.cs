﻿using Project.IService;
using Project.Model;
using Project.Model.EnumModel;
using Project.ViewModel;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service
{
	public class SystemLogServic : ISystemLogService
	{

		ILog logger;
		public SystemLogServic(ILog logger)
		{
			//最大连接数
			System.Net.ServicePointManager.DefaultConnectionLimit = 512;
			//日志
			this.logger = logger;
		}


		/// <summary>
		/// 写入数据库日志
		/// </summary>
		/// <param name="log">日志内容</param>
		/// <returns></returns>
		public async Task LogAdd(SystemLog log)
		{
			var repository = new Repository();
			log.CreateTime = DateTime.Now;
			log.CreateUser = "System";
			repository.CommandSet<SystemLog>().Insert(log);
		}

		/// <summary>
		/// 写入本地日志
		/// </summary>
		/// <param name="log">日志内容</param>
		/// <returns></returns>
		public async Task LocalLogAdd(SystemLog log)
		{
			//写入本地
			//记录请求日志
			logger.Debug(JsonConvert.SerializeObject(new
			{
				Guid = log.Guid,
				ClientType = log.ClientType,
				APIName = log.APIName,
				Instructions = log.Instructions,
				ReqParameter = log.ReqParameter,
				ResParameter = log.ResParameter,
				UserId = log.UserId,
				DeviceId = log.DeviceId,
				Time = log.Time,
				log.IP
			}));
		}

		/// <summary>
		/// 写入本地和数据库日志
		/// </summary>
		/// <param name="log">日志内容</param>
		/// <returns></returns>
		public async Task LocalAndSqlLogAdd(SystemLog log)
		{
			//写入本地
			//记录请求日志
			logger.Debug(JsonConvert.SerializeObject(new
			{
				Guid = log.Guid,
				ClientType = log.ClientType,
				APIName = log.APIName,
				Instructions = log.Instructions,
				ReqParameter = log.ReqParameter,
				ResParameter = log.ResParameter,
				UserId = log.UserId,
				DeviceId = log.DeviceId,
				Time = log.Time,
				log.IP
			}));

			////写入数据库
			var repository = new Repository();
			log.CreateTime = DateTime.Now;
			log.CreateUser = "System";
			repository.CommandSet<SystemLog>().Insert(log);

		}


		/// <summary>
		/// httpContext方式的时候一次写入日志
		/// </summary>
		/// <param name="httpContext">HTTP数据</param>
		/// <param name="instructions">操作说明</param>
		/// <param name="reqParameter">请求参数内容</param>
		/// <param name="resParameter">返回参数内容</param>
		/// <param name="type">类型 1：写数据库   2：写本地   3：同时写入数据库和本地</param>
		/// <returns></returns>
		public async Task AddLogByHttpContext(HttpContext httpContext, string instructions, string reqParameter, string resParameter, SystemLogType type)
		{
			string ip = httpContext.Connection.RemoteIpAddress.ToString();
			switch (type)
			{
				case SystemLogType.Sql:
					//写入数据库
					LogAdd(new SystemLog { Guid = httpContext.Request.Headers["GuidPwd"].ToString(), ClientType = httpContext.Request.Headers["ClientType"].ToString(), APIName = httpContext.Request.Path, UserId = httpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContext.Request.Headers["UserId"]), DeviceId = httpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContext.Request.Headers["DeviceId"].ToString(), Instructions = instructions, ReqParameter = reqParameter, ResParameter = resParameter, Time = "", IP = ip });
					break;
				case SystemLogType.Local:
					//写入本地日志
					LocalLogAdd(new SystemLog { Guid = httpContext.Request.Headers["GuidPwd"].ToString(), ClientType = httpContext.Request.Headers["ClientType"].ToString(), APIName = httpContext.Request.Path, UserId = httpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContext.Request.Headers["UserId"]), DeviceId = httpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContext.Request.Headers["DeviceId"].ToString(), Instructions = instructions, ReqParameter = reqParameter, ResParameter = resParameter, Time = "", IP = ip });
					break;
				case SystemLogType.SqlAndLocal:
					//写入数据库和本地日志
					LocalAndSqlLogAdd(new SystemLog { Guid = httpContext.Request.Headers["GuidPwd"].ToString(), ClientType = httpContext.Request.Headers["ClientType"].ToString(), APIName = httpContext.Request.Path, UserId = httpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContext.Request.Headers["UserId"]), DeviceId = httpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContext.Request.Headers["DeviceId"].ToString(), Instructions = instructions, ReqParameter = reqParameter, ResParameter = resParameter, Time = "", IP = ip });
					break;
			}
		}

		/// <summary>
		/// httpContext方式的时候一次写入日志  apiName不同
		/// </summary>
		/// <param name="httpContext">HTTP数据</param>
		/// <param name="aPIName">调用路径</param>
		/// <param name="instructions">操作说明</param>
		/// <param name="reqParameter">请求参数内容</param>
		/// <param name="resParameter">返回参数内容</param>
		/// <param name="type">类型 1：写数据库   2：写本地   3：同时写入数据库和本地</param>
		/// <param name="time"></param>
		/// <returns></returns>

		public async Task AddLogByHttpContext(HttpContext httpContext, string aPIName, string instructions, string reqParameter, string resParameter, SystemLogType type, string time)
		{
			string ip = httpContext.Connection.RemoteIpAddress.ToString();
			switch (type)
			{
				case SystemLogType.Sql:
					//写入数据库
					LogAdd(new SystemLog { Guid = httpContext.Request.Headers["GuidPwd"].ToString(), ClientType = httpContext.Request.Headers["ClientType"].ToString(), APIName = aPIName, UserId = httpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContext.Request.Headers["UserId"]), DeviceId = httpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContext.Request.Headers["DeviceId"].ToString(), Instructions = instructions, ReqParameter = reqParameter, ResParameter = resParameter, Time = time, IP = ip });
					break;
				case SystemLogType.Local:
					//写入本地日志
					LocalLogAdd(new SystemLog { Guid = httpContext.Request.Headers["GuidPwd"].ToString(), ClientType = httpContext.Request.Headers["ClientType"].ToString(), APIName = aPIName, UserId = httpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContext.Request.Headers["UserId"]), DeviceId = httpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContext.Request.Headers["DeviceId"].ToString(), Instructions = instructions, ReqParameter = reqParameter, ResParameter = resParameter, Time = time, IP = ip });
					break;
				case SystemLogType.SqlAndLocal:
					//写入数据库和本地日志
					LocalAndSqlLogAdd(new SystemLog { Guid = httpContext.Request.Headers["GuidPwd"].ToString(), ClientType = httpContext.Request.Headers["ClientType"].ToString(), APIName = aPIName, UserId = httpContext.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContext.Request.Headers["UserId"]), DeviceId = httpContext.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContext.Request.Headers["DeviceId"].ToString(), Instructions = instructions, ReqParameter = reqParameter, ResParameter = resParameter, Time = time, IP = ip });
					break;
			}
		}

	}
}
