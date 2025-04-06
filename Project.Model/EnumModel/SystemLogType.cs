using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.EnumModel
{
    /// <summary>
    /// 写日志类型
    /// </summary>
    public enum SystemLogType
    {
        /// <summary>
        /// 写入数据
        /// </summary>
        Sql = 1,

        /// <summary>
        /// 写入本地
        /// </summary>
        Local = 2,

        /// <summary>
        /// 写入数据库和本地
        /// </summary>
        SqlAndLocal = 3,
    }
}
