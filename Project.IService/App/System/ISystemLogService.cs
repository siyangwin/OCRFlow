using Project.Core;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.IService
{
    public interface ISystemLogService: ILogService
	{
		///// <summary>
		///// 写入数据库日志
		///// </summary>
		///// <param name="log">日志内容</param>
		///// <returns></returns>
		//Task LogAdd(SystemLog log);
	}
}
